using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPSocketServer : MonoBehaviour
{
    [SerializeField] private int bufferSize = 256;
    public bool isDebug = false;
    
    private static TcpListener tcpServer;
    private Thread serverThread;
    
    private readonly List<TcpClient> _clients = new List<TcpClient>();
    


    // ソケット接続準備、待機
    protected void Listen(string host, int port)
    {
        Debug.Log("ipAddress:" + host + " port:" + port);
        var ip = IPAddress.Parse(host);
        tcpServer = new TcpListener(ip, port);
        
        //接続を受け付け始める
        tcpServer.Start();
        //接続要求があった場合にDoAcceptTCPClientCallback()を実行する。
        tcpServer.BeginAcceptSocket(DoAcceptTcpClientCallback, tcpServer);
    }
    
    
    // クライアントからの接続処理
    private void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        //AsyncResultに受け渡されたリスナを変数に格納する。
        var listener = (TcpListener) ar.AsyncState;
        //リスナが受け入れた接続から送受信用のクライアントを作成する。
        var client = listener.EndAcceptTcpClient(ar);
        //クライアントのリストに作成したクライアントを追加する
        _clients.Add(client);
        
        Debug.Log("Connect: " + client.Client.RemoteEndPoint);

        // 接続が確立したら次のクライアントの受付を開始する。
        listener.BeginAcceptSocket(DoAcceptTcpClientCallback, listener);

        // 今接続した人とのネットワークストリームを取得
        var stream = client.GetStream();
        var reader = new StreamReader(stream,Encoding.UTF8);
        

        
        // 接続が切れるまで送受信を繰り返す
        while (client.Connected)
        {   
            //読み込んだデータを格納するためのバッファ。
            byte[] buffer = new byte[bufferSize];
            
            //streamからバッファの最大値まで読み取る
            var resSize = stream.Read(buffer, 0, buffer.Length);
            
            byte[] getData = new byte[resSize];

            for (int i = 0; i < resSize; i++)
            {
                getData[i] = buffer[i];
            }
            
            if (resSize != 0)
            {
                //Debug.Log(stream.Length);
                if (isDebug)
                {
                    Debug.Log("Receive！");
                }
                
                OnReceiveData(getData);
                
            }
            
            //OnMessage();
            
            // クライアントの接続が切れたら(送られてくる情報が途絶えたら)
            if (client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0))
            {
                Debug.Log("Disconnect: " + client.Client.RemoteEndPoint);
                client.Close();
                _clients.Remove(client);
            }
        }

    }


    // メッセージ受信
    protected virtual void OnReceiveData(byte[] receiveData)
    {
        if (isDebug)
        {
            Debug.Log("get "+receiveData.Length+" byte data");
        }
    }

    // クライアントにメッセージ送信
    protected void SendMessageToClient(string msg)
    {
        if (_clients.Count == 0)
        {
            return;
        }

        var body = Encoding.UTF8.GetBytes(msg);

        // 全員に同じメッセージを送る
        foreach (var client in _clients)
        {
            try
            {
                var stream = client.GetStream();
                stream.Write(body, 0, body.Length);
            }
            catch
            {
                _clients.Remove(client);
            }
        }
    }

    // 終了処理
    protected virtual void OnApplicationQuit()
    {
        if (tcpServer == null)
        {
            return;
        }

        if (_clients.Count != 0)
        {
            foreach (var client in _clients)
            {
                client.Close();
            }
        }

        tcpServer.Stop();
    }
}
