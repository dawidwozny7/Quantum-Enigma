using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMarble : Marble
{
   public override bool[,] PossibleMove(){
       int i,j;
       Marble c = null;
       bool[,] r = new bool[8,8];
       i= CurrentX-1;
       j= CurrentY+1;
        
        if(CurrentY != 0){
            if (BoardManager.Instance.Marbles[CurrentX, CurrentY-1] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX, CurrentY-1];
            }
            else
            {
                r[CurrentX, CurrentY-1] = true;

            }
       }
        if(CurrentX != 7){
            if (BoardManager.Instance.Marbles[CurrentX, CurrentY+1] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX, CurrentY+1];
            }
            else
            {
                r[CurrentX , CurrentY+1] = true;
            }
        }
       if(CurrentX != 0){
            if (BoardManager.Instance.Marbles[CurrentX - 1, CurrentY] != null)
            {
                c = BoardManager.Instance.Marbles[CurrentX - 1, CurrentY];
            }
            else
            {
                r[CurrentX - 1, CurrentY] = true;

            }
       }
       if(CurrentX != 7){
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
