using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Marble : MonoBehaviour
{
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isBasic;
    public bool isEntangled;
    public bool isDouble;
    public bool isMagneticRed;
    public bool isMagneticBlue;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8,8];
    }
}
