using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Marble[,] Marbles { set; get; }
    private Marble selectedMarble;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const float SF_TILE_OFFSET = 0.2f;

    private int selectionX = -1;
    private int selectionY = -1;

    private Quaternion tileOrientation = Quaternion.Euler(90, 0, 0);

    public int moves_left = 10;

    public List<GameObject> boardPiecesPrefabs;
    private List<GameObject> activePiece;

    private void Start()
    {
        SpawnAllLevel();
    }

    private void Update()
    {
        UpdateSelection();
        DrawBoard();

        if( Input.GetMouseButtonDown(0))
        {
            if(selectionX>=0 && selectionY>=0)
            {
                if(selectedMarble == null)
                {
                    SelectMarble(selectionX, selectionY);
                }
                else
                {
                    MoveMarble(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectMarble(int x,int y)
    {
        if(Marbles[x,y] == null)
        {
            return;
        }
        if (moves_left <= 0)
        {
            return;
        }
        selectedMarble = Marbles[x, y];
    }

    private void MoveMarble(int x, int y)
    {
        if(selectedMarble.PossibleMove(x,y))
        {
            Marbles[selectedMarble.CurrentX, selectedMarble.CurrentY] = null;
            selectedMarble.transform.position = GetTileCenter(x, y);
            Marbles[x, y] = selectedMarble;
            moves_left -= 1;
        }
        selectedMarble = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("BoardPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnPiece(int index, int x, int y)
    {
        GameObject go = Instantiate(boardPiecesPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Marbles[x, y] = go.GetComponent<Marble>();
        Marbles[x, y].SetPosition(x,y);
        activePiece.Add(go);
    }

    private void SpawnSFTile(int index, Vector3 position)
    {
        GameObject go = Instantiate(boardPiecesPrefabs[index], position, tileOrientation) as GameObject;
        go.transform.SetParent(transform);
        activePiece.Add(go);
    }

    private void SpawnLBTile(int index, Vector3 position)
    {
        GameObject go = Instantiate(boardPiecesPrefabs[index], position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activePiece.Add(go);
    }

    private void SpawnAllLevel()
    {
        activePiece = new List<GameObject>();
        Marbles = new Marble[8, 8];
        SpawnSFTile(0, GetTileSFCenter(0, 6)); //start tile in position (0,6) 
        SpawnSFTile(1, GetTileSFCenter(7, 2)); //finish tile in position (7,2)
        SpawnLBTile(2, GetTileCenter(0, 2)); //lava tile in position (0,2)
        SpawnLBTile(3, GetTileCenter(7, 0)); //blockade in position (7,0)
        SpawnPiece(4, 4, 3); //basic marble in position (4,3)
        SpawnPiece(4, 4, 4); //basic marble in position (4,4)
        SpawnPiece(4, 6, 5); //basic marble in position (6,5)
        SpawnPiece(5, 1, 3); //entangled marble in position (1,3)
        SpawnPiece(5, 5, 0); //entangled marble in position (5,0)
        SpawnPiece(5, 2, 7); //entangled marble in position (2,7)

    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    private Vector3 GetTileSFCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + SF_TILE_OFFSET;
        origin.z += (TILE_SIZE * y);
        return origin;
    }

    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; ++i)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        //Draw Selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                           Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                           Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
}
