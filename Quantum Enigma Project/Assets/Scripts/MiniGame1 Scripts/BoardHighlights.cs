using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public static BoardHighlights Instance{set;get;}

    public GameObject highlightPrefab;
    private List<GameObject> highlights;
    private void Start() {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetHighlightObject(){
        GameObject go = highlights.Find( g => !g.activeSelf);
        if(go == null){
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }
        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves){
        for(int i = 0 ; i<(BoardManager.GridSize) ;i++){
            for(int j = 0 ; j<(BoardManager.GridSize) ;j++){
                if(moves[i,j]){
                    GameObject goo = GetHighlightObject();
                    goo.SetActive(true);
                    goo.transform.position = new Vector3(i+0.5f,0,j+0.5f);
                }
            }
        }
    }

    public void HideHighlights(){
        foreach(GameObject go in highlights){
            go.SetActive(false);
        }
    }

}
