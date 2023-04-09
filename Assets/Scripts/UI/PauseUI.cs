using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject Player;
    InputReader inputReader;
    void Start()
    {
        inputReader = Player.GetComponent<InputReader>();

    }
    void Update()
    {
        
    }
    public void OnClickPause()
    {
        inputReader.OnUIDisable();
    }
    public void OnQuitPause()
    {
        inputReader.OnUIEnable();
    }


}
