using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Marble : MonoBehaviour
{
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isBasic;
    public bool isEntangled;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool PossibleMove(int x, int y)
    {
        return true;
    }
}
