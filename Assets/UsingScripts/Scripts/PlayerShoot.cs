using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPoint;
    [SerializeField]
    private GameObject playerCameraRoot;

    private void Update()
    {
        CheckBulletPoint();
    }

    void CheckBulletPoint()
    {
        Debug.DrawRay(bulletPoint.transform.position, (playerCameraRoot.transform.position - Camera.main.transform.position) * 15, Color.red);

    }
    public void Shoot()
    {
        Debug.Log("Shoot");
        BulletController bulletController = bulletPoint.GetComponent<BulletController>();
        bulletController.fireBullet();
        CheckHitTag();
    }
    void CheckHitTag()
    {
        RaycastHit hit;
        if (Physics.Raycast(bulletPoint.transform.position, playerCameraRoot.transform.position - Camera.main.transform.position, out hit, 30))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Ray hit Enemy");
            }
            if (hit.collider.CompareTag("Environment"))
            {
                Debug.Log("Ray hit Environment");
            }
        }
    }
}
