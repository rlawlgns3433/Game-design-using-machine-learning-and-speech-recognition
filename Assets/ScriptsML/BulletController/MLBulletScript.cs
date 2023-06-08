using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLBulletScript : MonoBehaviour
{
    //public GameObject player;
    public PlayAgent playAgent;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playAgent = player.GetComponent<PlayAgent>();

    }
    public void Start()
    {
        Invoke("DestroyBullet", 10f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("√—æÀ¿Ã ¿˚¿ª ∏¬√„");
            //playAgent.SetReward(+1.0f);
            playAgent.SetReward(true);
            playAgent.ChangeTargetPosition();
            Destroy(gameObject);

        }
        if (other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Wall")|| other.gameObject.CompareTag("OtherObject"))
        {
            Debug.Log("√—æÀ¿Ã »Ø∞Ê¿ª ∏¬√„");
            playAgent.SetReward(false);
            Destroy(gameObject);

        } 
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
