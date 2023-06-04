using System.Collections;
using System.Collections.Generic;
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
    public PlayerControllerNew PlayerController;
    public RayPerceptionSensorComponent3D rayPerceptionSensorComponent3D;

    Vector3 vel, angle_vel;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        vel = this.GetComponent<Rigidbody>().velocity;
        angle_vel = this.GetComponent<Rigidbody>().angularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
    }
    #endregion

    #region Agent

    public override void Initialize()
    {
        MaxStep = 5000;
    }

    public override void OnEpisodeBegin()
    {
        vel = angle_vel = Vector3.zero;
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        Target.transform.position = new Vector3(Random.Range(10.0f, -5.0f), 1.0f, Random.Range(10.0f, -5.0f));
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.z);

        sensor.AddObservation(transform.rotation.y);

        sensor.AddObservation(Target.position.x);
        sensor.AddObservation(Target.position.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;

        // 이동을 구현
        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;


        switch (action[0])
        {
            case 1: PlayerController.OnShoot(); break;
        }

        switch(action[1])
        {
            case 1: dir = transform.forward; break;
            case 2: dir = -transform.forward; break;
        }

        switch (action[2])
        {
            case 1: rot = -transform.up; break;
            case 2: rot = transform.up; break;
        }

        transform.Rotate(rot, Time.deltaTime * 200.0f);
        this.GetComponent<Rigidbody>().AddForce(dir * 2.0f, ForceMode.VelocityChange);
        SetReward(-1.0f / (float)(MaxStep));

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.DiscreteActions;
        //actionsOut.Clear();

        if(Input.GetMouseButtonDown(0))
        {
            action[0] = 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            action[1] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[1] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            action[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            action[2] = 2;
        }
    }

    #endregion
}
