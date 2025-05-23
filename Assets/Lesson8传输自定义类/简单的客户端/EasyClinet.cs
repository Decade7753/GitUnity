using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class EasyClinet : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Socket localSocket;
    
    void Start()
    {
        localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            localSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
            if (localSocket.Connected)
            {

            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e.Message);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
