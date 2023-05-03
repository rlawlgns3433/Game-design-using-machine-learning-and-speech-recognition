using StackExchange.Redis;
using System;
using UnityEngine;
using TMPro;

// Redis 스크립트 싱글톤 구성
public class Redis : Singleton<Redis>
{
    struct ServerInfo
    {
        public string host;
        public int port;
        public string password;

        public ServerInfo(string host, int port, string password) : this()
        {
            this.host = host;
            this.port = port;
            this.password = password;
        }
    }

    ServerInfo svr = new("127.0.0.1", 6006, "password1!");
    private ConnectionMultiplexer redisConnection;
    private ISubscriber sub;

    // Redis 초기화
    public bool Init(string host, int port, string password)
    {
        // Redis로 연결 시도
        try
        {
            ConfigurationOptions option = new ConfigurationOptions
            {
                EndPoints = { $"{host}:{port}" },
                Password = password,
            };

            redisConnection = ConnectionMultiplexer.Connect(option);
        }
        catch(Exception e)
        {
            Debug.Log(e);
            return false;
        }

        // Redis Connection 확인
        if(redisConnection.IsConnected)
        {
            sub = redisConnection.GetSubscriber();
            Debug.Log("Connection Success");
            return true;
        }
        return false;
    }

    private void Start()
    {
        
        Init(svr.host, svr.port, svr.password);
    }

    private void Update()
    {
        
    }

    public void Publish(string channel, string message)
    {
        Debug.Log("subscribers : " + sub.ToString());
        if (sub != null)
        {
            sub.Publish(channel, message);
        }

    }
    public void Subscribe(string channel, Action<RedisChannel, RedisValue> action)
    {
        sub.Subscribe(channel, action);
    }
}
