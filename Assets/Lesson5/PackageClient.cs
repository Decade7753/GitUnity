using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class PackageClient : MonoBehaviour
{
    private Socket socket;
    // Start is called before the first frame update
    void Start()
    {
        socket  = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            ThreadPool.QueueUserWorkItem(SendToServer);
            
            socket.Send(Encoding.UTF8.GetBytes("Hello World"));
             
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendToServer(object message)
    {
        socket.Connect("127.0.0.1", 8080);
        while (true)
        {
            socket.Send(Encoding.UTF8.GetBytes("Hello World"));
        }
    }
}
