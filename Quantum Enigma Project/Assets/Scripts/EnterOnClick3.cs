using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnterOnClick3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject.Find("Player").GetComponent<SaveLoadPosition>().Save();
        SceneManager.LoadScene(9);
    }
}
