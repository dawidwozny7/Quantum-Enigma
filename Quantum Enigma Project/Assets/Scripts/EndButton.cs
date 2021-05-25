using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
	{
        if(ClearBoards.won4 == true){
            ClearBoards.won5 = true;
        }
        if(ClearBoards.won3 == true){
            ClearBoards.won4 = true;
        }
        if(ClearBoards.won2 == true){
            ClearBoards.won3 = true;
        }
        if(ClearBoards.won1 == true){
            ClearBoards.won2 = true;
        }
        else{   
        ClearBoards.won1 = true;
        }
        //GameObject.Find("Player").GetComponent<SaveLoadPosition>().Load();
        SceneManager.LoadScene(1);
    }
}
