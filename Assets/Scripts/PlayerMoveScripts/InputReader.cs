using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;

    public bool disable = false;

    private Controls controls;
    [SerializeField]
    GameObject player;

    public void OnEnable()
    {
        if (controls != null)
            return;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }
    public void OnUIEnable()
    {
        controls.Player.Enable();
        disable = false;
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }
    
    public void OnUIDisable()
    {
        controls.Player.Disable();
        disable = true;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (disable)
            return;
        OnJumpPerformed?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (disable)
            return;
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (disable)
            return;
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (disable)
            return;
        Debug.Log("Shoot");
        BulletManager bulletManager = player.GetComponent<BulletManager>();
        bulletManager.GetBulletPrefeb();
    }
}
