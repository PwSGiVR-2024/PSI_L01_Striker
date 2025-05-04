using UnityEngine;
using Fleck;
using System.Collections.Generic;
using System.Threading;

public class WebsocketConnector : MonoBehaviour
{
    public static WebsocketConnector Instance { get; private set; }

    WebSocketServer server;
    List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();

    public bool DeviceConnected { get; private set; } = false;

    private readonly Queue<HeartRateData> _incoming = new Queue<HeartRateData>();
    private readonly object _lock = new object();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        server = new WebSocketServer("ws://0.0.0.0:7373");
        server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                Debug.Log("connected --> " + socket.ConnectionInfo.ClientIpAddress);
                allSockets.Add(socket);
                DeviceConnected = true;
            };
            socket.OnClose = () =>
            {
                Debug.Log("disconnected");
                allSockets.Remove(socket);
            };
            socket.OnMessage = msg =>
            {
                try
                {
                    var data = JsonUtility.FromJson<HeartRateData>(msg);
                    lock (_lock)
                        _incoming.Enqueue(data);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("THROW." + ex);
                }
            };
        });

        Debug.Log("server started");
    }

    void Update()
    {
        HeartRateData hr;
        lock (_lock)
        {
            if (_incoming.Count == 0) return;
            hr = _incoming.Dequeue();
        }

        Debug.Log("HR ---> " + hr.heartRate);
        SanityManager.instance.PushHeartRate(hr.heartRate);
    }

    void OnApplicationQuit()
    {
        foreach (var sock in allSockets.ToArray()) sock.Close();
        server.Dispose();
    }
}