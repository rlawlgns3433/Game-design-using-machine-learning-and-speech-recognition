using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public HPBar hpBar;

    void Start()
    {
        currentHP = maxHP;
        hpBar.SetMaxHP(maxHP);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            TakeDamage(20);
        }
        if(currentHP ==0){
            Debug.Log("게임 오버");
        }
    }

    void TakeDamage(int damage){
        currentHP -= damage;

        hpBar.SetHP(currentHP);

    }
}
