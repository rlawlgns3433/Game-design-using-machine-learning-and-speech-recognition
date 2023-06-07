using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MLPlayerControllerNew : MonoBehaviour
{
    public void OnShoot()
    {
            MLPlayerShoot playerShoot = transform.GetComponent<MLPlayerShoot>();
            playerShoot.CheckShoot();
    }
}
