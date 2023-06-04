using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerNew : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 moveDirection;
    private bool enShoot = true;

    public float input_x;
    public float input_y;
    [Header("Move Value Adjust")]
    [SerializeField]
    private float moveSpeed = 4f;
    float targetRotation;
    float rotationVelocity;
    float rotationSmoothTime;


    [Header("Jump Value Adjust")]
    [SerializeField]
    private float jumpDelay = 1f;
    [SerializeField]
    private float jumpTimer;
    [SerializeField]
    private float jumpPower = 3f;

    [Header("Look Value Adjust")]

    [SerializeField]
    [Tooltip("마우스 감도")]
    private float mouseLookSensitivity = 1.0f;

    [Header("Cinemachine")]

    [SerializeField]
    private GameObject CinemachineCameraTarget;

    [SerializeField]
    private bool cursorInputForLook = true;

    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField]
    private float TopClamp;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField]
    private float BottomClamp;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField]
    private float CameraAngleOverride = 0.0f;
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }
    private void Update()
    {
        if (moveDirection == Vector3.zero)
        {
            enShoot = true;
        }
        JumpDelayTimer();
        PlayerMove();
        OnClickESC();
    }
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f,input.y);
            enShoot = false;

        }
    }
    void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            Vector2 input = value.Get<Vector2>();
            if (input.sqrMagnitude >= 0.01f)
            {
                cinemachineTargetYaw += input.x * mouseLookSensitivity;
                cinemachineTargetPitch += input.y * mouseLookSensitivity;

            }
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(-cinemachineTargetPitch - CameraAngleOverride, cinemachineTargetYaw, 0.0f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, cinemachineTargetYaw, transform.rotation.y);
        }
    }
    void OnJump()
    {
        if (jumpTimer <= 0)
        {
            PlayerJump();
            jumpTimer = jumpDelay;
        }

    }
    public void OnShoot()
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
            moveDirection = moveDirection.normalized;
            transform.localPosition = transform.localPosition + (transform.rotation*moveDirection * moveSpeed * Time.deltaTime);
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
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
    }
    private void OnClickESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;

        }
    }
}
