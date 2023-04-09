using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Bullet() { }
    public Bullet(Vector3 pos)
    {
        this.transform.position = pos;
    }
    public Bullet(Vector3 pos, Vector3 direc) {
        this.transform.position = pos;
        this.transform.eulerAngles = direc;
    }
    
    public void SetBulletDirec(Vector3 targetBulletDir)
    {
        this.transform.eulerAngles = targetBulletDir;
    }
    public void ShootBullet(float bulletSpeed)
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }
    public void BulletDestroy()
    {
        Destroy(this);
    }
}
