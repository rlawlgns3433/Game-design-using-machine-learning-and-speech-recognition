using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //Rigidbody rigid;

    public GameObject bulletPos;
    [SerializeField]
    private GameObject bulletPref;
    GameObject bullet;
    bool shoot;

    private Vector3 ScreenCenter;
    private void Start()
    {
        //ScreenCenter = new Vector3(Camera.main.pixelWidth);
        //rigid = bullet.GetComponent<Rigidbody>();
        shoot = false;

    }
    public void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0f, 0.5f));
        Debug.DrawRay(transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0.5f)) * 10f, Color.red);
        if (shoot)
        {
            bullet.transform.Translate(Vector3.forward * 1f);
            //bullet.transform.position += bullet.transform.forward * 1.0f;
        }
    }
    public void GetBulletPrefeb()
    {
        bullet = Instantiate(bulletPref, bulletPos.transform);
        //Bullet bulletClass = bullet.GetComponent<Bullet>();
        //bulletClass.SetBulletDirec(Camera.main.transform.eulerAngles);
        //bulletClass.ShootBullet(1.0f);
        bullet.transform.position = bulletPos.transform.position;
        Vector3 vecCam = Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0,0.5f));

        bullet.transform.eulerAngles = vecCam;
        shoot = true;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            Debug.Log("Hit Border");
            Destroy(bullet, 0.1f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            Destroy(bullet, 0.1f);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (other.gameObject.CompareTag("Border"))
    //    //{
    //    //    Debug.Log("Hit Border");
    //    //    Destroy(bullet);
    //    //}
    //    //if (other.gameObject.CompareTag("Player"))
    //    //{
    //    //    Debug.Log("Hit Player");
    //    //    Destroy(bullet);
    //    //}
    //    if (other.tag == "Border")
    //    {
    //        Debug.Log("Hit Border");
    //        Destroy(bullet,0.1f);
    //    }
    //    if (other.tag == "Player")
    //    {
    //        Debug.Log("Hit Player");
    //        Destroy(bullet,0.1f);
    //    }
    //}

}
