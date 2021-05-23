using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(ClearBoards.won1){
            GameObject.Find("Room1Board").SetActive(false);
        }
        if(ClearBoards.won2){
            GameObject.Find("Room2Board").SetActive(false);
        }
        if(ClearBoards.won3){
            GameObject.Find("Room3Board").SetActive(false);
        }
        if(ClearBoards.won4){
            GameObject.Find("Room4Board").SetActive(false);
        }
        if(ClearBoards.won5){
            GameObject.Find("Room5Board").SetActive(false);
        }
        
    }

}
