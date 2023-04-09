using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPos;
    [SerializeField]
    private GameObject bulletPref;
    GameObject bullet;
    bool shoot;
    Vector3 dir;
    private Ray ray;
    //private Vector3 screenCenter;
    public void Start()
    {
        dir = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction;
        ray = new Ray(bulletPos.transform.position, dir);


        //screenCenter = new Vector3(Camera.main.pixelWidth/2,Camera.main.pixelHeight/2);
        shoot = false;

    }
    public void Update()
    {
        if (shoot)
        {
            //bullet.transform.position += Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,0f)) * -0.1f;
            bullet.transform.position += ray.direction * 50 * Time.deltaTime;
        }
    }
    public void GetBulletPrefeb()
    {
        bullet = Instantiate(bulletPref, bulletPos.transform);
        bullet.transform.position = bulletPos.transform.position;
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
}
