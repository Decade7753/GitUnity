using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerSubInfo : DataBase
{
   
    public string PlayerName;
    
    public override byte[] Writing()
    {
        byte[] data = new byte[GetLength()];
        int index = 0;
        WriteString(data,PlayerName,ref index);
        return data;
    }

    public override int GetLength()
    {
        return 4 + Encoding.UTF8.GetBytes(PlayerName).Length;
    }

    public override int Read(byte[] buffer, int nowIndex = 0)
    {
        int index = nowIndex;
        PlayerName = ReadString(buffer,ref index);
        return index - nowIndex;
    }
}
public class PlayerInfo : DataBase
{
    public int atk;
    public int def;
    public int speed;
    public string name;
    public bool isDead;
    public float hp;
    public PlayerSubInfo subInfo;
    //public PlayerSubInfo pp2;
    //public PlayerSubInfo pp3;
    
    public override byte[] Writing()
    {
        byte[] sumData = new byte[GetLength()];
        int index = 0;
        WriteBool(sumData, isDead, ref index);
        WriteFloat(sumData, hp, ref index);
        WriteInt(sumData, atk, ref index);
        WriteInt(sumData, def, ref index);
        WriteInt(sumData, speed, ref index);
        WriteString(sumData, name, ref index);
        WriteUserData(sumData, subInfo, ref index);
        //WriteUserData(sumData, pp2, ref index);
        //WriteUserData(sumData, pp3, ref index);
        return sumData;
    }

    public override int GetLength()
    {
        int length = 0;
        length += sizeof(bool);
        length += sizeof(float);
        length += sizeof(int) * 3;
        int nameLen = sizeof(int) + Encoding.UTF8.GetBytes(name).Length;
        length += nameLen;
        length += subInfo.GetLength();
        return length;
    }

    public override int Read(byte[] buffer, int nowIndex = 0)
    {
        int index = nowIndex;
        isDead = ReadBoolean(buffer, ref index);
        hp = ReadFloat(buffer, ref index);
        atk = ReadInt(buffer, ref index);
        def = ReadInt(buffer, ref index);
        speed = ReadInt(buffer, ref index);
        name = ReadString(buffer, ref index);
        subInfo = ReadUserData<PlayerSubInfo>(buffer, ref index);
        return index - nowIndex;
    }
}
