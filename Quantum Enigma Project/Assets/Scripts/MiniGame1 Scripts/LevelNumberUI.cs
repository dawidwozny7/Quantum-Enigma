using UnityEngine;
using System.Collections;
using UnityEngine.UI;  // IMPORTANT!!!!!!!!

public class LevelNumberUI : MonoBehaviour
{

    public Text LevelText;  // public if you want to drag your text object in there manually
    int LevelCounter; 

    void Start()
    {
        LevelCounter = GameObject.Find("Board").GetComponent<BoardManager>().level_number;
        LevelText = GetComponent<Text>();  // if you want to reference it by code - tag it if you have several texts
    }

    void Update()
    {
        LevelCounter = GameObject.Find("Board").GetComponent<BoardManager>().level_number;
        LevelText.text = LevelCounter.ToString();  // make it a string to output to the Text object
    }
}