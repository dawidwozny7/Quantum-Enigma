using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnterOnClick1 : MonoBehaviour
{
   private void OnMouseDown()
    {
        SceneManager.LoadScene(2);
    }
}
