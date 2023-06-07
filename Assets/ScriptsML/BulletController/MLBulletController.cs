using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLBulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject playerCameraRoot;
    [SerializeField]
    private GameObject playerFollowCamera;
    [SerializeField]
    private float delay;
    private Vector3 bulletDirec;

    public void Start()
    {
    }

    public void FireBullet()
    {
        Invoke(nameof(FireDelay), delay);
    }

    void FireDelay()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bulletDirec = playerCameraRoot.transform.position - playerFollowCamera.transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(bulletDirec, ForceMode.Impulse);
    }

}
