using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson3Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.isDead = false;
        playerInfo.hp = 100.2f;
        playerInfo.atk = 100;
        playerInfo.def = 200;
        playerInfo.speed = 300;
        playerInfo.name = "Decade";
        
        playerInfo.subInfo = new PlayerSubInfo();
        playerInfo.subInfo.PlayerName = "Dec";


        PlayerInfo playerInfod = new PlayerInfo();
        playerInfod.Read(playerInfo.Writing());
        print(playerInfod.name);
        print(playerInfod.atk);
        print(playerInfod.def);
        print(playerInfod.speed);
        print(playerInfod.hp);
        print(playerInfod.isDead);
        print(playerInfod.subInfo.PlayerName);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
