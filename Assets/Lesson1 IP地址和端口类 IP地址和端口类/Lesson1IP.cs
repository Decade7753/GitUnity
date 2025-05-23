using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson1IP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IPHostEntry ipHostEntry = Dns.GetHostEntry("www.taocaiyu.work");
        for (int i = 0; i < ipHostEntry.AddressList.Length; i++)
        {
            Debug.Log(ipHostEntry.AddressList[i]);
        }
        Debug.Log(ipHostEntry.HostName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
