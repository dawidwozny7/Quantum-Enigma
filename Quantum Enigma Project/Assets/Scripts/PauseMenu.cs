using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject cursor;
    public GameObject playerSounds;

    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                playerSounds.SetActive(true);
                cursor.SetActive(true);
                Resume();
            }else
            {
                playerSounds.SetActive(false);
                cursor.SetActive(false);
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;
        
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        ClearBoards.won1 = false;
        ClearBoards.won2 = false;
        ClearBoards.won3 = false;
        ClearBoards.won4 = false;
        ClearBoards.won5 = false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
