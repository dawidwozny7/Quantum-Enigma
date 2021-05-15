using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelMovesLeft : MonoBehaviour
{
   public static int movesl;
    public Text moves;

    // Start is called before the first frame update
    void Start()
    {
        moves.text = "Moves: " + BoardManager.Instance.moves_left.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        moves.text = "Moves: " + movesl.ToString();
    }
}
