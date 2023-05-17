using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigid;
    PlayerInput.ActionEvent actionEvent;
    private Vector3 moveDirection;
    [SerializeField]
    private float moveSpeed = 4f;
    [SerializeField]
    private float jumpDelay = 1f;
    [SerializeField]
    private float jumpTimer;
    [SerializeField]
    private float jumpPower = 3f;
    [SerializeField]
    private bool enShoot = true;
    void Awake()
    {
        actionEvent = new();
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (moveDirection == Vector3.zero)
        {
            enShoot = true;
        }
        JumpDelayTimer();
        PlayerMove();
    }
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);
            enShoot = false;
        }
    }
    void OnLook(InputValue value)
    {

    }
    void OnJump()
    {
        if (jumpTimer <= 0)
        {
            PlayerJump();
            jumpTimer = jumpDelay;
        }

    }
    void OnShoot()
    {
        if (enShoot)
        {
            PlayerShoot playerShoot = transform.GetComponent<PlayerShoot>();
            playerShoot.CheckShoot();
        }
    }
    void PlayerMove()
    {
        bool hasControl = (moveDirection != Vector3.zero);
        if (hasControl)
        {
            transform.position += (moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    void PlayerJump()
    {
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    void JumpDelayTimer()
    {
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
    }
}
