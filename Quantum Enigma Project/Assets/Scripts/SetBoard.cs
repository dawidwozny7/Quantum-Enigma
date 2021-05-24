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
            GameObject.Find("Door/door_inner/Door Trigger").SetActive(true);
            GameObject.Find("Door (1)/door_inner/Door Trigger").SetActive(true);
        }
        if(ClearBoards.won2){
            GameObject.Find("Room2Board").SetActive(false);
            GameObject.Find("Door (2)/door_inner/Door Trigger").SetActive(true);
            GameObject.Find("Door (3)/door_inner/Door Trigger").SetActive(true);
        }
        if(ClearBoards.won3){
            GameObject.Find("Room3Board").SetActive(false);
            GameObject.Find("Door (4)/door_inner/Door Trigger").SetActive(true);
            GameObject.Find("Door (5)/door_inner/Door Trigger").SetActive(true);
        }
        if(ClearBoards.won4){
            GameObject.Find("Room4Board").SetActive(false);
            GameObject.Find("Door (6)/door_inner/Door Trigger").SetActive(true);
            GameObject.Find("Door (7)/door_inner/Door Trigger").SetActive(true);
        }
        if(ClearBoards.won5){
            GameObject.Find("Room5Board").SetActive(false);
            GameObject.Find("EndGameTrigger").SetActive(true);
        }
        
    }

}
