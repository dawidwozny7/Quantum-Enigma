using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnterOnClick5 : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject.Find("Player").GetComponent<SaveLoadPosition>().Save();
        SceneManager.LoadScene(11);
    }
}
