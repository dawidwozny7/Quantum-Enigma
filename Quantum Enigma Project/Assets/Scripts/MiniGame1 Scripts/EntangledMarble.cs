using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntangledMarble : Marble
{
    public override bool[,] PossibleMove()
    {
        int i, j;
        Marble c = null;
        bool[,] r = new bool[8, 8];
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    if (BoardManager.Instance.Marbles[i, j] != null)
                    {
                        c = BoardManager.Instance.Marbles[i, j];
                    }
                    else
                    {
                        r[i, j] = true;
                    }
                }
                i++;
            }
        }

        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    if (BoardManager.Instance.Marbles[i, j] != null)
                    {
                        c = BoardManager.Instance.Marbles[i, j];
                    }
                    else
                    {
                        r[i, j] = true;
                    }
                }
                i++;
            }
        }

        if (CurrentX != 0)
        {
            if (BoardManager.Instance.Marbles[CurrentX - 1, CurrentY] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX - 1, CurrentY];
            }
            else
            {
                r[CurrentX - 1, CurrentY] = true;

            }
        }
        if (CurrentX != 7)
        {
            if (BoardManager.Instance.Marbles[CurrentX + 1, CurrentY] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX + 1, CurrentY];
            }
            else
            {
                r[CurrentX + 1, CurrentY] = true;
            }
        }


        return r;
    }

}
