using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPosition : MonoBehaviour
{
    public float x, y, z;
    
    void Start()
    {

    }

    void Update()
    {

    }

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
        if (ClearBoards.won1 == true)
        {
            x = PlayerPrefs.GetFloat("x");
            y = PlayerPrefs.GetFloat("y");
            z = PlayerPrefs.GetFloat("z");

            Vector3 LoadPosition = new Vector3(x, y, z);
            transform.position = LoadPosition;
        }
    }
}
