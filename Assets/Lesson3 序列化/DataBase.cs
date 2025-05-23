using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.WSA;

public abstract class DataBase
{
    public abstract byte[] Writing();
    public abstract int GetLength();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffer">目标数组</param>
    /// <param name="nowIndex">起始点位置 不能使用ref</param>
    /// <returns></returns>
    public abstract int Read(byte[] buffer , int nowIndex = 0);

    #region 写入的单个方法
    /// <summary>
    /// 
    /// </summary>
    /// <param name="summaryData">存储的数组</param>
    /// <param name="data">写入的数据</param>
    /// <param name="index">存储的索引值</param>


    protected virtual void WriteInt(byte[] summaryData , int data , ref int index)
    {
        //将int数据转换成二进制字节数组 并且将它拷贝至我们的目标数组中
        BitConverter.GetBytes(data).CopyTo(summaryData, index);
        index += 4;
    }

    protected virtual void WriteFloat(byte[] summaryData, float data, ref int index)
    {
        //将float数据转换成二进制字节数组 并且将它拷贝至我们的目标数组中
        BitConverter.GetBytes(data).CopyTo(summaryData, index);
        index += sizeof(float);
    }
    
    protected virtual void WriteBool(byte[] summaryData, bool data, ref int index)
    {
        //将float数据转换成二进制字节数组 并且将它拷贝至我们的目标数组中
        BitConverter.GetBytes(data).CopyTo(summaryData, index);
        index += sizeof(bool);
    }
    
    protected virtual void WriteString(byte[] summaryData, string data, ref int index)
    {
        //先存储字符串长度的字节数组
        BitConverter.GetBytes(Encoding.UTF8.GetBytes(data).Length).CopyTo(summaryData, index);
        index += 4;
        //再存储对应的字符串值
        Encoding.UTF8.GetBytes(data).CopyTo(summaryData, index);
        index += Encoding.UTF8.GetBytes(data).Length;
    }

    #endregion

    #region 读取的单个方法

    /// <summary>
    /// 
    /// </summary>
    /// <param name="summaryData">需要去读取的目标字节数组</param>
    /// <param name="index">读取的起点位置</param>
    /// <returns></returns>
    protected virtual int ReadInt(byte[] summaryData, ref int index)
    {
        int result = BitConverter.ToInt32(summaryData, index);
        index += sizeof(int);
        return result;
    }
    protected virtual float ReadFloat(byte[] summaryData, ref int index)
    {
        float result = BitConverter.ToSingle(summaryData, index);
        index += sizeof(float);
        return result;
    }
    protected virtual bool ReadBoolean(byte[] summaryData, ref int index)
    {
        bool result = BitConverter.ToBoolean(summaryData, index);
        index += sizeof(bool);
        return result;
    }
    protected virtual string ReadString(byte[] summaryData, ref int index)
    {
        int length = ReadInt(summaryData, ref index);
        string result = Encoding.UTF8.GetString(summaryData, index, length);
        index += length;
        return result;
    }
    
    

    #endregion
   

    protected virtual void WriteUserData(byte[] summaryData, DataBase data, ref int index)
    {
        //存储我们自定义数据类型的方法 我们定义一个规则 该自定义数据类型必须继承于该父类
        //因为我们写的父类中的Writing的返回值本身就是一个二进制字节数组
        //实际上在调用该方法时会自动接上我们自定义数据结构类的writing方法 进行这个类的完全序列化 相当于两个类的序列化相加
        data.Writing().CopyTo(summaryData, index);
        index += data.GetLength();

    }
    
    /// <summary>
    /// 该方法通过调用子类中的Read方法去反序列化对应的值
    /// </summary>
    /// <param name="summaryData">需要反序列化的数组</param>
    /// <param name="index">反序列化起点位置</param>
    /// <typeparam name="T">子类类型</typeparam>
    /// <returns></returns>
    
    //如果该类进行反序列化 首先会去实例化一个新的类对象 使用该类对象自己实现的read方法 最终返回值也是表示来类对象存储长度的int值
    protected virtual T ReadUserData<T>(byte[] summaryData, ref int index) where T : DataBase , new()
    {
        T result = new T();
        index += result.Read(summaryData, index);
        return result;
    }
    
}
