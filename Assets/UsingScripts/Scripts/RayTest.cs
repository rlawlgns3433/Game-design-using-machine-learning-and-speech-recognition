using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RayTest : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPoint;
    [SerializeField]
    private GameObject playerCameraRoot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.DrawRay(bulletPoint.transform.position, -Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)) * 15, Color.red);
        Debug.DrawRay(bulletPoint.transform.position,(playerCameraRoot.transform.position-Camera.main.transform.position)*15, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(bulletPoint.transform.position, playerCameraRoot.transform.position - Camera.main.transform.position,out hit, 30))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit");
            }
            if(hit.collider.CompareTag("Environment"))
            {
                Debug.Log("Environment hit");
            }
        }
    }
}
