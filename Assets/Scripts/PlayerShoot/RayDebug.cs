using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDebug : MonoBehaviour
{
    Transform firePos;
    Vector3 dir;
    Ray ray;

    private void Start()
    {
        firePos = GameObject.Find("Bullet Pos").transform;
        dir = Camera.main.ScreenPointToRay(new Vector3(0.5f,0.5f,0f)).direction;
        ray = new Ray(firePos.position, dir);
    }
}
