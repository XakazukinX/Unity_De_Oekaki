using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TCPReceiver : TCPSocketServer
{
#pragma warning disable 0649
    // ポート指定（他で使用していないもの、使用されていたら手元の環境によって変更）
    [SerializeField]private int _port = 22223;
#pragma warning restore 0649

    [SerializeField] private RawImage targetImage;
    private Encoding encorder = Encoding.UTF8;

    public GameObject targetGameObject;

    private int w;
    private int h;

    private byte[] textureData;
    private bool isDone;
    
    private void Start()
    {
        // 接続中のIPV4を取得
        var ipAddress = IPAddressManager.GetIP(ADDRESSFAMILYTYPE.IPv4);
        // 指定したポートを開く
        Listen("127.0.0.1", _port);
    }

    private void Update()
    {
        if (isDone)
        {
            isDone = false;
            Texture2D texture = new Texture2D(w,h,TextureFormat.RGBA32, false);
            texture.LoadImage(textureData);
            targetImage.texture = texture;
            Debug.Log(checkIsPNG(textureData));

        }
    }


    protected override void OnReceiveData(byte[] receiveData)
    {
        base.OnReceiveData(receiveData);

        string signature = checkIsPNG(receiveData);

        if (signature != "PNG")
        {
            if (isDebug)
            {
                Debug.Log("Not PNG");
            }
            return;
        }
        
        var size = checkSize(receiveData);
        
        setTexture(size[0],size[1]);

        textureData = new byte[receiveData.Length];
        for (int i = 0; i < receiveData.Length; i++)
        {
            textureData[i] = receiveData[i];
        }

        isDone = true;

    }

    private void setTexture(int width,int height)
    {
        Debug.Log("Start Set Texture");
        
        //byteからTexture2D作成
        //Texture2D texture = new Texture2D(width,height,TextureFormat.RGBA32, false);
        
/*        Debug.Log(height);*/

        w = width;
        h = height;
        /*texture.LoadImage(receiveData);

        targetImage.texture = texture;*/
    }




    //送信されてきた画像がPNGかどうかを調べるための文字列を返す関数
    //1~3バイト目をエンコードしてPNGの文字列が帰ってくればpngファイルである
    private string checkIsPNG(byte[] checkData)
    {
        
        string dataSignature = "";

        var sigData = new byte[3];
        for (int i = 1; i < 4; i++)
        {
            sigData[i-1] = checkData[i];
        }
        
        dataSignature = encorder.GetString(sigData);
        
        return dataSignature;
    }
    
    
    //送信されてきた画像のサイズを返す関数
    //16~19バイト目に横幅が、20~23バイト目に縦幅の情報がそれぞれしまってあるらしい
    private int[] checkSize(byte[] checkData)
    {
        int[] ret = new int[2];
        //横サイズの判定
        int pos = 16;
        int width = 0;
        for (int i = 0; i < 4; i++)
        {
            width = width * 256 + checkData[pos++];
        }
        //縦サイズの判定
        int height = 0;
        for (int i = 0; i < 4; i++)
        {
            height = height * 256 + checkData[pos++];
        }
        
        Debug.Log(width+"*"+height);

        ret[0] = width;
        ret[1] = height;

        return ret;
    }
}
