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
    List<string> MsgList;
    TMP_Text textUI;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollParent;
    [SerializeField] GameObject canv;
    [SerializeField] InputField chatInput;
    [SerializeField] Scrollbar scrollbarVertical;
    GameObject previousComment;


    #region MonoBehaviour

    private void Awake()
    {

    }

    private void Update()
    {
        try
        {
            // 채널 메시지 항상 수신중
            Redis.Instance.Subscribe("test", SubAction);
            // 수신한 메시지가 있으면
            if (MsgList.Count > 0)
            { 
                string[] messages = MsgList[0].Split(",", 2);
                MsgList.RemoveAt(0);
                // 메시지를 id, comment로 분리해서 추가
                AddComment(messages[0], messages[1]);
                StartCoroutine("Waitmillisec");
            }
        }
        catch (Exception e) { }
        

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 캔버스 꺼져 있을 때
            if (!canv.active)
            {
                canv.active = true;
                chatInput.Select();
            }
            // 캔버스 켜져 있을 때
            else if (canv.active)
            {
                chatInput.Select();
                if (chatInput.text == "") canv.SetActive(!canv.active);
                else
                {
                    Redis.Instance.Publish(m_channel, Redis.Instance.entireMessage);
                    AddComment("지훈", chatInput.text);
                    chatInput.text = "";
                    StartCoroutine("Waitmillisec");
                }
            }
        }
    }

    #endregion


    #region CommentAction
    public void AddComment(string id, string comment)
    {
        // 기존에 저장된 메시지가 있다면 삭제하고 다시 생성
        if(Redis.Instance.entireMessage != "") Destroy(previousComment);
        GameObject prefab = Instantiate(commentPrefab);
        prefab.transform.SetParent(scrollParent.transform);
        prefab.transform.SetAsLastSibling();
        textUI = prefab.GetComponent<TMP_Text>();
        textUI.horizontalAlignment = HorizontalAlignmentOptions.Left;
        scrollbarVertical.value = 0;

        // 기존 저장된 메시지에 id와 comment를 추가
        Redis.Instance.entireMessage += id + " : " + comment + '\n';
        // 추가된 메시지를 보여줌
        textUI.text = GetEntireMessage();
        scrollRect.verticalNormalizedPosition = 0;
        previousComment = prefab;

        Debug.Log(GetEntireMessage());
    }

    IEnumerator Waitmillisec()
    {
        chatInput.Select();
        yield return new WaitForSeconds(0.001f);
    }

    public void SubAction(RedisChannel ch, RedisValue va)
    {
        MsgList.Add(va);
    }

    string GetEntireMessage()
    {
        return Redis.Instance.entireMessage;
    }

    #endregion
}