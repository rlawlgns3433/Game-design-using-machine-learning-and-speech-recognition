using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;
using StackExchange.Redis;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Comment : Singleton<Comment>
{
    private static string EntireChat = "";
    private const char eol = '\n';


    [SerializeField]
    TMP_Text comment_roll;

    Text inputField;

    string MessageLog = "";
    List<string> msg;
    // Start is called before the first frame update
    void Start()
    {

        comment_roll = GameObject.Find("Comment").GetComponent<TMP_Text>();
        inputField = GameObject.Find("TextInput").GetComponent<Text>();
        StartCoroutine("SubChannel");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                string message = inputField.text;
                EntireChat += message + eol;
                Redis.Instance.Publish("test", message);
                StartCoroutine("InitInputField");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void SubAction(RedisChannel ch, RedisValue va)
    {
        comment_roll.text += va.ToString() + "\n";
        Instantiate(comment_roll);
    }

    public void OnClickSendButton()
    {
        try
        {
            string message = inputField.text;
            Redis.Instance.Publish("test", message);
            StartCoroutine("InitInputField");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator SubChannel()
    {
        try
        {
            Redis.Instance.Subscribe("test", SubAction);
            
        }
        catch (Exception e)
        {

        }
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.001f);
    }

    IEnumerator InitInputField()
    {
        inputField.text = "";

        yield return new WaitForSeconds(0.001f);
    }
}
