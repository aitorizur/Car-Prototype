using System.Collections;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class SocketManager : MonoBehaviour
{
    Socket socket;
    string serverIP = "127.0.0.1";
    int socketPort = 16384;

    byte[] obuffer = new byte[2048];
    byte[] ibuffer = new byte[2048];

    OnlineManager onlineMng;

    private void Awake()
    {
        onlineMng = GetComponent<OnlineManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(serverIP), socketPort);
        socket.Connect(remoteEP);
        StartCoroutine(OnReceived());
        Send("JOIN|" + onlineMng.playerName);
    }

    public void Send(string str)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(str + "$");
        bytes.CopyTo(obuffer, 0);

        if (socket.Connected)
            socket.Send(obuffer, bytes.Length, SocketFlags.None);
    }

    private IEnumerator OnReceived()
    {
        while (true)
        {
            if (socket.Available > 0)
            {
                int bytesReceived = socket.Available;

                byte[] strBuffer = new byte[bytesReceived];

                socket.Receive(ibuffer, socket.Available, SocketFlags.None);

                Buffer.BlockCopy(ibuffer, 0, strBuffer, 0, bytesReceived);
                string str = Encoding.ASCII.GetString(strBuffer);
                Debug.Log(str);

                onlineMng.ParseMessages(str);
            }

            yield return null;
        }
    }
}
