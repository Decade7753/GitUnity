using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerInfomation : DataBase
{
    
    public string playerName;
    public int atk;
    public int def;
    public float speed;
    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytesData = new byte[GetLength()];
        WriteString(bytesData,playerName,ref index);
        WriteInt(bytesData,atk,ref index);
        WriteInt(bytesData,def,ref index);
        WriteFloat(bytesData,speed,ref index);
        return bytesData;
    }

    public override int GetLength()
    {
        return 4*4 + Encoding.UTF8.GetBytes(playerName).Length;
    }

    //new一个类对象直接调用该方法即可完成赋值操作
    public override int Read(byte[] buffer, int nowIndex = 0)
    {
        int index = nowIndex;
        playerName = ReadString(buffer, ref index);
        atk = ReadInt(buffer, ref index);
        def = ReadInt(buffer, ref index);
        speed = ReadFloat(buffer, ref index);
        return index - nowIndex;
    }
}
