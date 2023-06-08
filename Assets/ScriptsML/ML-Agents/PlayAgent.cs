using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayAgent : Agent
{
    // Field x : -10 ~ 1, z : -8 ~ 3
    #region Declaration Part

    public Transform Target;
    public List<Transform> Obstacles;
    public MLPlayerControllerNew PlayerController;
    public RayPerceptionSensorComponent3D rayPerceptionSensorComponent3D;
    public bool rw;
    public Vector3 vel, angle_vel;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        vel = this.GetComponent<Rigidbody>().velocity;
        angle_vel = this.GetComponent<Rigidbody>().angularVelocity;
    }

    #endregion

    #region Agent

    public override void Initialize()
    {
        MaxStep = 1000;
        rayPerceptionSensorComponent3D = GetComponent<RayPerceptionSensorComponent3D>();
    }

    public override void OnEpisodeBegin()
    {
        vel = angle_vel = Vector3.zero;
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        ChangeTargetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(rw);
        sensor.AddObservation(transform.localRotation.y);
        sensor.AddObservation(Target.localPosition.x);
        sensor.AddObservation(Target.localPosition.z);
        sensor.AddObservation(RayCastInfo(rayPerceptionSensorComponent3D));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;

        // 이동을 구현
        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        float angle_btw_v = RayCastInfo(rayPerceptionSensorComponent3D);
        if (angle_btw_v != 0.0)
        {
            AddReward(+1.0f / (float)MaxStep);
            if (Mathf.Abs(angle_btw_v) <= 10)
            {
                AddReward(+0.3f);
                PlayerController.OnShoot();
            }
        }

        switch (action[0])
        {
            case 1: dir = transform.forward; break;
            case 2: dir = -transform.forward; break;
        }

        switch (action[1])
        {
            case 1: rot = -transform.up; break;
            case 2: rot = transform.up; break;
        }

        transform.Rotate(rot, Time.deltaTime * 200.0f);
        //this.GetComponent<Rigidbody>().AddForce(dir * 2.0f, ForceMode.VelocityChange);
        //this.transform.TransformPoint((transform.position + dir) * Time.deltaTime);
        StartCoroutine(PlayerMoveML(dir));
        //transform.localPosition = transform.localPosition + (transform.localRotation * dir * 4.0f * Time.deltaTime);
        this.GetComponent<Rigidbody>().velocity = dir * 2.0f;
        AddReward(-1.0f / (float)(MaxStep));

    }
    IEnumerator PlayerMoveML(Vector3 dir) {
        transform.localPosition = transform.localPosition + (transform.localRotation * dir * 4.0f * Time.deltaTime);
        yield return new WaitForEndOfFrame();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.DiscreteActions;
        //actionsOut.Clear();

        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            action[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            action[1] = 2;
        }
    }

    #endregion



    public void SetReward(bool re)
    {

        rw = re;
        if (re)
            SetReward(+1.0f);
        else
            AddReward(-1.0f / MaxStep);
    }


    private float RayCastInfo(RayPerceptionSensorComponent3D rayComponent)
    {
        var rayOutputs = RayPerceptionSensor
                .Perceive(rayComponent.GetRayPerceptionInput())
                .RayOutputs;

        if (rayOutputs != null)
        {
            var lengthOfRayOutputs = RayPerceptionSensor
                    .Perceive(rayComponent.GetRayPerceptionInput())
                    .RayOutputs
                    .Length;

            for (int i = 0; i < lengthOfRayOutputs; i++)
            {
                GameObject goHit = rayOutputs[i].HitGameObject;
                if (goHit != null)
                {
                    // Found some of this code to Denormalized length
                    // calculation by looking trough the source code:
                    // RayPerceptionSensor.cs in Unity Github. (version 2.2.1)
                    var rayDirection = rayOutputs[i].EndPositionWorld - rayOutputs[i].StartPositionWorld;
                    var scaledRayLength = rayDirection.magnitude;
                    float rayHitDistance = rayOutputs[i].HitFraction * scaledRayLength;

                    // Print info:
                    string dispStr;
                    dispStr = "__RayPerceptionSensor - HitInfo__:\r\n";
                    dispStr = dispStr + "GameObject name: " + goHit.name + "\r\n";
                    dispStr = dispStr + "GameObject tag: " + goHit.tag + "\r\n";
                    dispStr = dispStr + "Hit distance of Ray: " + rayHitDistance + "\r\n";
                    //Debug.Log(dispStr);


                    // Ray -> Enemy
                    // 현재 True 반환
                    // Angle이 절대값 10이내 -> +1.0
                    if (goHit.tag == "Enemy")
                    {
                        return CheckAngle(goHit);       
                    }
                    else return 0.0f;
                }
                else return 0.0f;
            }
        }
        return 0.0f;
    }

    public void ChangeTargetPosition()
    {
        Target.transform.localPosition = new Vector3(Random.Range(10.0f, -5.0f), 1.0f, Random.Range(10.0f, -5.0f));
    }

    public float CheckAngle(GameObject goHit)
    {
        Vector3 btwV = (goHit.transform.position - transform.position);
        Vector3 vvv = btwV - transform.forward;
        float ag = Vector3.SignedAngle(transform.forward, vvv, transform.up);

        return ag;
    }
}