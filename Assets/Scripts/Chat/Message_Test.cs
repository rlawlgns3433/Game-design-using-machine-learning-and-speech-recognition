using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Message_Test : MonoBehaviour
{
    Queue<UserInfo> messageQ = new Queue<UserInfo>();
    class UserInfo
    {
        string u_id;
        
        public UserInfo(string id)
        {
            u_id = id;
        }
        public void SetUserID(string id)
        {
            try
            {
                u_id = id;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
        public string GetUserID()
        {
            return u_id;
        }
    }

    void ManageSub(RedisChannel ch, RedisValue val)
    {
        Debug.Log(val);
        List<string> values = ((val.ToString()).Split(',')).ToList();
        UserInfo user = new(values[0]);
        messageQ.Enqueue(user);
    }

    public void SendMessage()
    {
        Redis.Instance.Publish("test", "hi");
    }

}
