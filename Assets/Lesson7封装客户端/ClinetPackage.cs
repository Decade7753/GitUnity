using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ClinetPackage : MonoBehaviour
{
    private static ClinetPackage instance;
    public static ClinetPackage Instance => instance;

    public Socket clinetSocket;
    private Queue<byte[]> sendQueue = new Queue<byte[]>();
    //接受服务端消息的数组
    private byte[] bytes = new byte[1024 * 1024];
    //缓存
    private byte[] cacheBytes = new byte[1024 * 1024];
    //缓存当前存储的数量
    private int cachenumber = 0;
    

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //声明socket类对象
        Connect("127.0.0.1", 8080);
        
        //收消息
        ThreadPool.QueueUserWorkItem(ReceiveFromServer);
    }

    // Update is called once per frame
    void Update()
    {
        if (sendQueue.Count > 0)
        {
            ThreadPool.QueueUserWorkItem(SendToServer);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TransactPlayer p1 = new TransactPlayer();
            PlayerInfomation p2 = new PlayerInfomation();
            p2.playerName = "Clinet";
            p2.atk = 100;
            p2.def = 100;
            p2.speed = 100;
            p1.playerInfo = p2;
            p1.gameID = 001;
            AddSend(p1);
        }
    }

    public void Connect(string ip, int port)
    {
        if (clinetSocket == null)
        {
            clinetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clinetSocket.Connect(ip, port);
            }
            catch (SocketException socketException)
            {
                Console.WriteLine(socketException.Message);
                throw;
            }
            
        }
        
    }

    public void AddSend<T>(T customClass) where T : TransactBase , new()
    {
        //因为socket的send是阻塞方式的所以此方法仅仅是将二进制数组传入队列中
        sendQueue.Enqueue(customClass.Writing());
    }
    
    //线程池发送对象
    public void SendToServer(object obj)
    {
        //继承了该消息传递类的类会自己实现对应的方法
        clinetSocket.Send(sendQueue.Dequeue());
    }
    
    //线程池收消息
    public void ReceiveFromServer(object obj)
    {
        while (true)
        {
            if (clinetSocket.Available > 0)
            {
                // int len = clinetSocket.Receive(bytes);
                // int code = BitConverter.ToInt32(bytes, 0);
                // switch (code)
                // {
                //     case 101:
                //         TransactPlayer p3 = new TransactPlayer();
                //         p3.Read(bytes,4);
                //         Debug.Log(p3.playerInfo.playerName);
                //         break;
                // }
                int len = clinetSocket.Receive(bytes);
                HandleData(bytes, len);
            }
        }
    }

    private void HandleData(byte[] data, int len)
    {
        int index = 0;
        //整个类对象的长度
        int length = 0;
        int code = 0;
        
        data.CopyTo(cacheBytes,cachenumber);
        cachenumber += len;
        
        //这个循环只是对粘包数据进行依次遍历 遇到分包时退出循环 等待下一次数据的发送
        while (true)
        {
            length = -1;
            
            if (cachenumber - index >= 8)
            {
                //字节长度大于等于8才能解析最基本的消息
                //前4个字节代表该数据的类型后四个代表整个类的长度
                code = BitConverter.ToInt32(cacheBytes, index);
                index += 4;
                length = BitConverter.ToInt32(cacheBytes, index);
                index += 4;
            }

            if (cachenumber - index >= length && length != -1)
            {
                TransactPlayer p = new TransactPlayer();
                switch (code)
                {
                    case 101:
                        TransactPlayer p1 = new TransactPlayer();
                        //单纯的粘包不需要length参数
                        p.Read(cacheBytes, index);
                        break;
                }
        
                index += length;
                if (index == cachenumber)
                {
                    cachenumber = 0;
                    break;
                }
            }
            else
            {
                //data.CopyTo(cacheBytes, 0);
                //cachenumber = len;
                if (length != -1)
                {
                    index -= 8;
                }
                
                Array.Copy(cacheBytes, index, cacheBytes, 0, cachenumber - index);
                cachenumber  = cachenumber - index;
                break;
            }
            

            
        }
        
        
    }
}
