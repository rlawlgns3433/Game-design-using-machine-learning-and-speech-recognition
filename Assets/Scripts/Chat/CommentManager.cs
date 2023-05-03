using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CommentManager : MonoBehaviour
{
    string recentId = "";
    string chatLog = "";
    TMP_Text textUI;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollParent;
    [SerializeField] GameObject canv;
    [SerializeField] InputField chatInput;
    GameObject previousComment;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
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
                    Redis.Instance.Publish("test", chatInput.text);
                    AddComment("jh1", chatInput.text);
                    chatInput.text = "";
                    StartCoroutine("Waitmillisec");
                }
            }
        }
    }
    IEnumerator Waitmillisec()
    {
        chatInput.Select();
        yield return new WaitForSeconds(0.001f);
    }
}