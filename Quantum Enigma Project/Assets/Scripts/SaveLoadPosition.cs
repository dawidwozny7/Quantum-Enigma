using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPosition : MonoBehaviour
{
    public GameObject player;
    public float x, y, z;
    
    public void Save()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        PlayerPrefs.SetFloat("x", x);
        PlayerPrefs.SetFloat("y", y);
        PlayerPrefs.SetFloat("z", z);
    }

    public void Load()
    {
        x = PlayerPrefs.GetFloat("x");
        y = PlayerPrefs.GetFloat("y");
        z = PlayerPrefs.GetFloat("z");

        Vector3 LoadPosition = new Vector3(x, y, z);
        player.transform.position = LoadPosition;
    }
}
