using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
	{
        if(other.CompareTag("Player"))
		{
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
            SceneManager.LoadScene("EndGameEND");
		}
	}
}

/*private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        LoadNextLevel();

    }
}

public void LoadNextLevel()
{
    Cursor.lockState = CursorLockMode.None;
    Time.timeScale = 1f;
    StartCoroutine(LoadLevel("EndGame"));
}

IEnumerator LoadLevel(string levelInd)
{
    transition.SetTrigger("Start");
    yield return new WaitForSeconds(1);
    SceneManager.LoadScene(levelInd);
} */
