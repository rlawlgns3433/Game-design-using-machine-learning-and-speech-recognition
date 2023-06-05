using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    public GameObject obj;
    private void Start()
    {
        Vector3 btwVector = (obj.transform.position - transform.position).normalized;
        //float angle = Mathf.Acos(Vector3.Dot(transform.forward.normalized, btwVector));
        float angle = Vector3.SignedAngle(transform.forward, btwVector, transform.up);
        Debug.Log(angle);
    }
}
