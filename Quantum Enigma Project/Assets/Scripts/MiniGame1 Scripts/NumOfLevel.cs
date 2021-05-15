using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumOfLevel : MonoBehaviour
{
     public static int leveln;

    public Text lvl;
    // Start is called before the first frame update
    void Start()
    {
        lvl.text = "Level: " + BoardManager.Instance.level_number.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        lvl.text = "Level: " + leveln.ToString();
    }
}
