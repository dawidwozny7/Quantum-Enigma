using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMarble : Marble
{
    /*public override bool[,] PossibleMove()
    {
        int i, j;
        Marble c = null;
        bool[,] r = new bool[8, 8];
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 2; k++)
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
                i+=2;
            }
        }

        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 2; k++)
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
                i+=2;
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
    }*/

    public override bool[,] PossibleMove()
    {
        Marble c = null;
        bool[,] r = new bool[8, 8];

        if (CurrentY >= 2)
        {
            if (BoardManager.Instance.Marbles[CurrentX, CurrentY - 2] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX, CurrentY - 2];
            }
            else
            {
                r[CurrentX, CurrentY - 2] = true;

            }
        }
        if (CurrentY <= 5 )
        {
            if (BoardManager.Instance.Marbles[CurrentX, CurrentY + 2] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX, CurrentY + 2];
            }
            else
            {
                r[CurrentX, CurrentY + 2] = true;
            }
        }
        if (CurrentX >= 2)
        {
            if (BoardManager.Instance.Marbles[CurrentX - 2, CurrentY] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX - 2, CurrentY];
            }
            else
            {
                r[CurrentX - 2, CurrentY] = true;

            }
        }
        if (CurrentX <= 5)
        {
            if (BoardManager.Instance.Marbles[CurrentX + 2, CurrentY] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX + 2, CurrentY];
            }
            else
            {
                r[CurrentX + 2, CurrentY] = true;
            }
        }

        if(CurrentX > 0 && CurrentY > 0)
        {
            if (BoardManager.Instance.Marbles[CurrentX - 1, CurrentY - 1] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX - 1, CurrentY - 1];
            }
            else
            {
                r[CurrentX - 1, CurrentY - 1] = true;
            }
        }

        if (CurrentX > 0 && CurrentY < 7)
        {
            if (BoardManager.Instance.Marbles[CurrentX - 1, CurrentY + 1] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX - 1, CurrentY + 1];
            }
            else
            {
                r[CurrentX - 1, CurrentY + 1] = true;
            }
        }

        if (CurrentX < 7 && CurrentY > 0)
        {
            if (BoardManager.Instance.Marbles[CurrentX + 1, CurrentY - 1] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX + 1, CurrentY - 1];
            }
            else
            {
                r[CurrentX + 1, CurrentY - 1] = true;
            }
        }

        if (CurrentX < 7 && CurrentY < 7)
        {
            if (BoardManager.Instance.Marbles[CurrentX + 1, CurrentY + 1] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX + 1, CurrentY + 1];
            }
            else
            {
                r[CurrentX + 1, CurrentY + 1] = true;
            }
        }

        return r;
    }

}
