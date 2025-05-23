using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public string input;
    // Start is called before the first frame update
    public InputField inputField;
    //1.建立socket通道
    public Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
   
    void Start()
    {
        inputField.onSubmit.AddListener((a) =>
        {
            input = a.ToString();
            if (client.Connected)
            {
                client.Send(Encoding.ASCII.GetBytes(input));
            }
        });
        //connect服务端
        try
        {
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
        }
        catch (SocketException e)
        {
            Console.WriteLine("服务器拒绝了你的连接");
            Console.WriteLine(e.Message);
            throw;
        }
        
        //send发送与receive接受数据
        byte[] bytes = new byte[1024];
        int receiveCount =  client.Receive(bytes);
        Debug.Log(Encoding.UTF8.GetString(bytes, 0, receiveCount));
        client.Send(Encoding.UTF8.GetBytes("你好！成功连接服务器!"));

        Thread receive = new Thread(SolveRequest);
        receive.Start();
        
        // client.Shutdown(SocketShutdown.Both);
        // client.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            client.Send(Encoding.UTF8.GetBytes("用户输入"));
        }
    }


    public void SolveRequest()
    {
        byte[] bytes = new byte[1024];
        while (true)
        {
            if (client.Available > 0)
            {
                int numm = client.Receive(bytes);
                Debug.Log(Encoding.UTF8.GetString(bytes, 0, numm));
            }
            
            
        }
    }
}
