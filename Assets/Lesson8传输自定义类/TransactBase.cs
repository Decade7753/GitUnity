using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactBase : DataBase
{
    public override byte[] Writing()
    {
        throw new System.NotImplementedException();
    }

    public override int GetLength()
    {
        throw new System.NotImplementedException();
    }

    public override int Read(byte[] buffer, int nowIndex = 0)
    {
        throw new System.NotImplementedException();
    }

    public virtual int GetId()
    {
        return 0;
    }
}
