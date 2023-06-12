using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

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
    float timer = 0;
    float timer_max = 5.0f;
    float minX, maxX, minZ, maxZ;
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
        MaxStep = 5000;
        rayPerceptionSensorComponent3D = GetComponent<RayPerceptionSensorComponent3D>();
        minX = transform.position.x;
        maxX = transform.position.x;
        minZ = transform.position.z;
        maxZ = transform.position.z;
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
        //Target위치를 직접 받아와서 핵처럼 위치를 아는건 아닌가?
        //sensor.AddObservation(Target.localPosition.x);
        //sensor.AddObservation(Target.localPosition.z);
        sensor.AddObservation(RayCastInfo(rayPerceptionSensorComponent3D));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var discAction = actions.DiscreteActions;
        var contAction = actions.ContinuousActions;
        // 이동을 구현
        Vector3 rot = Vector3.zero;
        Vector3 frontback = Vector3.zero;
        Vector3 leftright = Vector3.zero;

        if (!(RayCastInfo(rayPerceptionSensorComponent3D) == null))
        {

            if (CheckDistance(RayCastInfo(rayPerceptionSensorComponent3D)) < 3.0f || CheckDistance(RayCastInfo(rayPerceptionSensorComponent3D)) > 15.0f)
            {
                AddReward(-1.0f / (float)MaxStep);
            }
            else
            {
                AddReward(+0.1f);
                float angle_btw_v = CheckAngle(RayCastInfo(rayPerceptionSensorComponent3D));
                if (Mathf.Abs(angle_btw_v) <= 10)
                {
                    AddReward(+1.0f);
                    PlayerController.OnShoot();
                }
            }
            AddReward(+0.3f);
        }
        else
        {
            if (MinMaxDistance())
            {
                if (Vector3.Distance(new Vector3(minX, 0, minZ), new Vector3(maxX, 0, maxZ)) >= 20)
                {
                    AddReward(+0.3f);
                }
                else
                {
                    AddReward(-0.3f);
                }
            }
        }


        switch (discAction[0])
        {
            case 0: break;
            case 1: frontback = -transform.forward; break;
            case 2: frontback = transform.forward; break;
            case 3: frontback = transform.forward; break;
        }

        switch (discAction[1])
        {
            case 0: break;
            case 1: leftright = transform.right; break;
            case 2: leftright = -transform.right; break;
        }

        //switch (discAction[2])
        //{
        //    case 0: break;
        //    case 1: rot = -transform.up; break;
        //    case 2: rot = transform.up; break;
        //}

        //Continuous Action
        Vector3 rotationDir = transform.up * Mathf.Clamp(contAction[0], -1f, 1f);

        Vector3 direction = frontback + leftright;

        transform.Rotate(rotationDir, Time.deltaTime * 150f);
        StartCoroutine(PlayerMoveML(direction));



    }

    IEnumerator PlayerMoveML(Vector3 dir)
    {
        transform.localPosition = transform.localPosition + (transform.localRotation * dir * 7.0f * Time.deltaTime);
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


    private GameObject RayCastInfo(RayPerceptionSensorComponent3D rayComponent)
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


                    //// Ray -> Enemy
                    //// 현재 True 반환
                    //// Angle이 절대값 10이내 -> +1.0
                    if (goHit.tag == "Enemy")
                    {
                        //return CheckAngle(goHit);
                        return goHit;
                    }
                    else return null;
                    //return goHit;
                }
                else return null;
            }
        }
        return null;
    }

    public void ChangeTargetPosition()
    {
        float rangeX = 46.0f;
        float rangeY = -46.0f;
        float randX = Random.Range(rangeX, rangeY);
        float randZ = Random.Range(rangeX, rangeY);
        //float rangeX = 31.0f;
        //float rangeY = -31.0f;
        //float randX = Random.Range(rangeX, rangeY);
        //float randZ = Random.Range(rangeX, rangeY);
        //if (Mathf.Abs(randZ) <= 22 && Mathf.Abs(randZ) > 16)
        //{
        //    do
        //    {
        //        randX = Random.Range(rangeX, rangeY);
        //    } while (Mathf.Abs(randX) <= 20.5 && Mathf.Abs(randX) > 4);
        //}
        //else if (Mathf.Abs(randZ) <= 16 && Mathf.Abs(randZ) > 11)
        //{
        //    do
        //    {
        //        randX = Random.Range(rangeX, rangeY);
        //    } while (Mathf.Abs(randX) <= 20.5 && Mathf.Abs(randX) > 11.5);
        //}
        //else if (Mathf.Abs(randZ) <= 11 && Mathf.Abs(randZ) > 3)
        //{
        //    do
        //    {
        //        randX = Random.Range(rangeX, rangeY);
        //    } while (Mathf.Abs(randX) <= 20.5 && Mathf.Abs(randX) > 15.5);
        //}
        Target.transform.localPosition = new Vector3(randX, 1.0f, randZ);
    }

    public float CheckAngle(GameObject goHit)
    {
        Vector3 btwV = (goHit.transform.position - transform.position);
        Vector3 vvv = btwV - transform.forward;
        float ag = Vector3.SignedAngle(transform.forward, vvv, transform.up);

        return ag;
    }

    public float CheckDistance(GameObject enemy)
    {
        float dis = Vector3.Distance(enemy.transform.position, transform.position);
        return dis;
    }

    public bool MinMaxDistance()
    {

        if (timer <= 0)
        {
            timer = timer_max;
            minX = transform.position.x;
            maxX = transform.position.x;
            minZ = transform.position.z;
            maxZ = transform.position.z;
            return true;

        }
        else
        {
            timer -= Time.deltaTime;
            minX = Mathf.Min(minX, transform.position.x);
            minZ = Mathf.Min(minZ, transform.position.z);
            maxX = Mathf.Max(maxX, transform.position.x);
            maxZ = Mathf.Max(maxZ, transform.position.z);
            return false;
        }


    }
}