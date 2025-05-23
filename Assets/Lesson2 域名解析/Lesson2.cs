using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class Lesson2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetIpConifg();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }



    public async void GetIpConifg()
    {
        Task<IPHostEntry> ipHostEntry = Dns.GetHostEntryAsync("www.taocaiyu.work");
        await ipHostEntry;
        Debug.Log(ipHostEntry.Result.HostName);

        



    }
}
