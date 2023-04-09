using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPos;
    [SerializeField]
    private GameObject bulletPref;
    public GameObject bullet;
    bool shoot;
    private void Start()
    {
        shoot = false;
    }
    public void Update()
    {
        if (bullet.activeInHierarchy&&shoot)
        {
            bullet.transform.Translate(Vector3.forward * 1f);
        }
    }
    public void GetBulletPrefeb()
    {
        bullet = Instantiate(bulletPref, bulletPos.transform);
        //Bullet bulletClass = bullet.GetComponent<Bullet>();
        //bulletClass.SetBulletDirec(Camera.main.transform.eulerAngles);
        //bulletClass.ShootBullet(1.0f);
        
        bullet.transform.eulerAngles = Camera.main.transform.eulerAngles;
        shoot = true;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            Debug.Log("Hit Border");
            Destroy(bullet);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            Destroy(bullet);
        }
    }

}
