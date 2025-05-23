using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 这个类算是一个用于传输的中间对象类
/// 负责将我们要的类对象转成二进制数组发送到对应的位置
/// </summary>
public class TransactPlayer : TransactBase
{
    public int gameID;
    public PlayerInfomation playerInfo;
    public override int GetLength()
    {
        return 4 + 4 + 4 + playerInfo.GetLength();
    }

    public override byte[] Writing()
    {
        byte[] bytesData = new byte[GetLength()];
        int index = 0;
        //先发送头部类标注信息
        WriteInt(bytesData, GetId(), ref index);
        //写入该类型所占字节长度
        WriteInt(bytesData,GetLength()-8, ref index);
        //写入具体信息
        WriteInt(bytesData,gameID,ref index);
        WriteUserData(bytesData,playerInfo,ref index);
        return bytesData;
        
    }

    public override int Read(byte[] buffer, int nowIndex = 0)
    {
        //服务器解析时已经设置index了
        int index = nowIndex;
        gameID = ReadInt(buffer, ref index);
        playerInfo = ReadUserData<PlayerInfomation>(buffer, ref index);
        
        return index - nowIndex;
    }

    //在服务器解析的前四个字节代表我们发送的数据类型
    public override int GetId()
    {
        return 101;
    }
}
