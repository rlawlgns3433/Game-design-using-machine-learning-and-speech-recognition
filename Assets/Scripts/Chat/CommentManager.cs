using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using StackExchange.Redis;
using System.Linq;
using System.Collections.Generic;
using System;

public class CommentManager : MonoBehaviour
{

    private string m_channel = "test";
    string recentId = "";
    string chatLog = "";
    List<string> MsgList;
    TMP_Text textUI;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollParent;
    [SerializeField] GameObject canv;
    [SerializeField] InputField chatInput;
    GameObject previousComment;


    #region MonoBehaviour

    private void Awake()
    {
        StartCoroutine("SubChannel");
    }

    private void Update()
    {
        try
        {
            if (MsgList.Count > 0)
            {
                string[] messages = MsgList[0].Split(",", 2);
                MsgList.RemoveAt(0);
                AddComment(messages[0], messages[1]);
            }
        }
        catch (Exception e) { }
        

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Äµ¹ö½º ²¨Á® ÀÖÀ» ¶§
            if (!canv.active)
            {
                canv.active = true;
                chatInput.Select();
            }
            // Äµ¹ö½º ÄÑÁ® ÀÖÀ» ¶§
            else if (canv.active)
            {
                chatInput.Select();
                if (chatInput.text == "") canv.SetActive(!canv.active);
                else
                {
                    Redis.Instance.Publish(m_channel, chatInput.text);
                    AddComment("ÁöÈÆ", chatInput.text);
                    chatInput.text = "";
                    StartCoroutine("Waitmillisec");
                }
            }
        }
    }

    #endregion

    public void AddComment(string id, string comment)
    {
        if(chatLog != "") Destroy(previousComment);
        Debug.Log(comment);
        GameObject prefab = Instantiate(commentPrefab);
        prefab.transform.SetParent(scrollParent.transform);
        prefab.transform.SetAsLastSibling();
        textUI = prefab.GetComponent<TMP_Text>();
        textUI.horizontalAlignment = HorizontalAlignmentOptions.Left;

        chatLog += id + " : " + comment + '\n';
        textUI.text = chatLog;
        scrollRect.verticalNormalizedPosition = 0;
        previousComment = prefab;
    }

    public void SubAction(RedisChannel ch, RedisValue va)
    {
        MsgList.Add(va);
    }


    IEnumerator Waitmillisec()
    {
        chatInput.Select();
        yield return new WaitForSeconds(0.001f);
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

        yield return new WaitForSeconds(0.001f);
    }
}