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
       if(CurrentY != 7){
           for (int k = 0; k < 3; k++)
           {
               if(i>=0 || i<8 ){
                   c = BoardManager.Instance.Marbles[i,j];
                   if(c==null){
                       r[i,j] = true;
                   }
               }
               i++;
           }
       }
    
        i=CurrentX-1;
        j=CurrentY-1;
        if(CurrentY != 0){
           for (int k = 0; k < 3; k++)
           {
               if(i>=0 || i<8 ){
                   c = BoardManager.Instance.Marbles[i,j];
                   if(c==null){
                       r[i,j] = true;
                   }
               }
               i++;
           }
       }

       if(CurrentX != 0){
           c = BoardManager.Instance.Marbles[CurrentX-1,CurrentY];
           if(c == null){
               r[CurrentX-1, CurrentY] = true;

           }
       }
       if(CurrentX != 0){
           c = BoardManager.Instance.Marbles[CurrentX+1,CurrentY];
           if(c == null){
               r[CurrentX+1, CurrentY] = true;
               
           }
       }


    return r;
   }

}
