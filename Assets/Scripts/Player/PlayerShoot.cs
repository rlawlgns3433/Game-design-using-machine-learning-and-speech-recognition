using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPoint;

    [SerializeField]
    private GameObject playerCameraRoot;

    [SerializeField]
    private float shootDelay = 0.7f;

    //build 전 private로 변경
    public float delayTimer = 0;
    PlayerAnimation playerAnimation;

    public void Start()
    {
        playerAnimation = transform.GetComponent<PlayerAnimation>();
    }
    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
        //CheckShoot();
        CheckBulletPoint();

    }
    public void CheckShoot()
    {
        if (delayTimer <= 0)
        {
            StartCoroutine(WaitforShoot());

            delayTimer = shootDelay;
        }

    }
    IEnumerator WaitforShoot()
    {
        playerAnimation.ShootingAnim();
        Shoot();
        yield return new WaitForEndOfFrame();
    }
    void CheckBulletPoint()
    {
        Vector3 screenMiddlePoint = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.7f, 0.0f));
        //Debug.DrawRay(bulletPoint.transform.position, (playerCameraRoot.transform.position - Camera.main.transform.position) * 15, Color.red);
        Debug.DrawRay(bulletPoint.transform.position, (playerCameraRoot.transform.position - screenMiddlePoint) * 15, Color.green);

    }
    public void Shoot()
    {
        Debug.Log("Shoot");
        BulletController bulletController = bulletPoint.GetComponent<BulletController>();
        bulletController.FireBullet();
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
