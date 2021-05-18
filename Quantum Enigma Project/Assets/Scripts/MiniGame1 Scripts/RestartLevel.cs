using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartLevel : MonoBehaviour
{
    public void restartLevel()
    {
        //NumOfLevel.leveln = level_number;
        //LevelMovesLeft.movesl = 0 + BoardManager.Instance.moves_left;
        BoardHighlights.Instance.HideHighlights();
        BoardManager.Instance.selectedMarble = null;

        foreach (GameObject ob in BoardManager.Instance.activePiece)
        {
            Destroy(ob);
        }

        BoardManager.Instance.SpawnAllLevel();
    }
        
}
