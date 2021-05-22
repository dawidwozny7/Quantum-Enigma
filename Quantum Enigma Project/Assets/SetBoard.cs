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
            GameObject.Find("Room1Board").SetActive(false);
        }
        
    }

}
