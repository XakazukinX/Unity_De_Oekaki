using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Es.InkPainter;
using UnityEngine;
using UnityEngine.UI;

public class TCPSender : MonoBehaviour
{
    private TcpClient tcpClient;
    [SerializeField] private string targetIPAddress = "127.0.0.1";
    [SerializeField] private int targetPORT = 22223;
    [SerializeField] private InkCanvas targetCanvas;

    [SerializeField] private InputField ipAddressInputField;
    [SerializeField] private InputField portInputField;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //おそらくレイヤーの話。今回は一枚に絞るので基本ループは一回
            for(int i = 0; i < targetCanvas.paintSet.Count; ++i)
            {
                var paintSet = targetCanvas.paintSet[i];
                if(paintSet.paintMainTexture != null)
                    SendRenderTextureToPNG(paintSet.paintMainTexture);
            }
        }
    }

    public void OnClick()
    {
        targetIPAddress = ipAddressInputField.text;
        targetPORT = Convert.ToInt32(portInputField.text);
        
        //おそらくレイヤーの話。今回は一枚に絞るので基本ループは一回
        for(int i = 0; i < targetCanvas.paintSet.Count; ++i)
        {
            var paintSet = targetCanvas.paintSet[i];
            if(paintSet.paintMainTexture != null)
                SendRenderTextureToPNG(paintSet.paintMainTexture);
        }
    }
    

    public async void Send(byte[] sendBytes)
    {
        try
        {
            tcpClient = new TcpClient(targetIPAddress, targetPORT);
        }
        catch
        {
            Debug.LogError("Error!送信に失敗しました");
            return;
        }

        NetworkStream nStream = tcpClient.GetStream();
        nStream.ReadTimeout = 15000;
        nStream.WriteTimeout = 15000;

        //データを送信する
        await nStream.WriteAsync(sendBytes, 0, sendBytes.Length);
        nStream.Close();
        tcpClient.Close();
    }
    
    private void SendRenderTextureToPNG(RenderTexture renderTexture)
    {
        var newTex = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;
        newTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTex.Apply();

        byte[] pngData = newTex.EncodeToPNG();
        if(pngData == null)
        {
            Debug.LogError("pngData is null");
            return;
        }
        
        Send(pngData);
        Debug.Log("Send!");
    }
}
