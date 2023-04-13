using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject playerCameraRoot;
    private Vector3 bulletDirec;
    
    public void fireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bulletDirec = playerCameraRoot.transform.position - Camera.main.transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(bulletDirec, ForceMode.Impulse);
    }

}
