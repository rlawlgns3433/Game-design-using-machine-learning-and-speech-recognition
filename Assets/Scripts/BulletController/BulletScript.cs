using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("√—æÀ¿Ã ¿˚¿ª ∏¬√„");
            DestroyBullet();
            //Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("OtherObject"))
        {
            Debug.Log("√—æÀ¿Ã »Ø∞Ê¿ª ∏¬√„");
            DestroyBullet();
            //Destroy(gameObject);
        }
        else
        {
            DestroyBullet();
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Debug.Log("√—æÀ¿Ã ¿˚¿ª ∏¬√„");
    //        //playAgent.SetReward(+1.0f);
    //        Destroy(gameObject);
    //    }
    //    if (collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("OtherObject"))
    //    {
    //        Debug.Log("√—æÀ¿Ã »Ø∞Ê¿ª ∏¬√„");
    //        //playAgent.SetReward(-1.0f /playAgent.MaxStep);
    //        Destroy(gameObject);
    //    }
    //}
    private void Awake()
    {
        Invoke("DestroyBullet", 10f);

    }
    public void Start()
    {
    }
    void DestroyBullet()
    {
        Debug.Log("destroyed bullet");
        Destroy(gameObject);
    }
}
