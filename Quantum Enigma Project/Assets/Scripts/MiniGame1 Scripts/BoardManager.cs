using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance{set;get;}
    private bool[,] allowedMoves{set;get;}
    public Marble[,] Marbles {set;get;}
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

    
    private void Awake()
    {
        Instance = this;
    }

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
        allowedMoves = Marbles[x,y].PossibleMove();
        selectedMarble = Marbles[x, y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveMarble(int x, int y)
    {
         int addx = selectedMarble.CurrentX-x;
        int addy = selectedMarble.CurrentY-y;
        if(selectedMarble.GetType() == typeof(EntangledMarble)){
            Marble c = null;
            
            foreach (Marble mar in Marbles)
            {
                if(mar!=null && mar != c){
                if(mar.GetType()== typeof(EntangledMarble)){
                    int newx = mar.CurrentX-addx;
                    int newy = mar.CurrentY-addy;
                    if(newx>=0 && newx <8 && newy >= 0 && newy< 8 && leveldesign[7-newx,newy]!= 3){
                    if(mar.PossibleMove()[newx , newy]){
                        Marbles[mar.CurrentX, mar.CurrentY] = null;
                        mar.transform.position = GetTileCenter(newx,newy);
                        mar.SetPosition(newx,newy);
                        Marbles[newx,newy] = mar;
                        if(leveldesign[7-newy,newx]== 2){
                            activePiece.Remove(mar.gameObject);
                            Marbles[newx,newy] = null;
                            Destroy(mar.gameObject);
                        }
                    }
                    }
                }
                c = mar;
                }
            }
            moves_left -= 1;
        }
       else if(allowedMoves[x,y] && leveldesign[7-x,y]!= 3 )
        {
            Marbles[selectedMarble.CurrentX, selectedMarble.CurrentY] = null;
            selectedMarble.transform.position = GetTileCenter(x, y);
            selectedMarble.SetPosition(x,y);
            Marbles[x, y] = selectedMarble;
            if(leveldesign[7-y,x]== 2){
                activePiece.Remove(selectedMarble.gameObject);
                Destroy(selectedMarble.gameObject);
            }
            moves_left -= 1;
            
        }
        BoardHighlights.Instance.HideHighlights();
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


    /*
    0 = start tile
    1 = finish tile
    2 = lava
    3 = blockade
    4 = basic marble
    5 = entangled marble
    9 = empty
    */

    /*    int[,] leveldesign = new int[,]{       //visualization here correlates to visualization in the game
        {9 , 9 , 2 , 9 , 9 , 9 , 0 , 9},       //left-top = {0,7} --- right-top = {7,7}
        {9 , 9 , 9 , 9 , 5 , 9 , 9 , 9},
        {9 , 9 , 9 , 9 , 9 , 9 , 9 , 5},
        {9 , 9 , 9 , 9 , 9 , 9 , 9 , 9},
        {9 , 9 , 9 , 4 , 4 , 9 , 9 , 9},
        {5 , 9 , 9 , 9 , 9 , 9 , 9 , 9},
        {9 , 9 , 9 , 9 , 9 , 4 , 9 , 9},
        {3 , 9 , 1 , 9 , 9 , 9 , 9 , 9}         //left-bottom = {0,0} --- bottom-right = {7,0}
    };*/

    public int[,] leveldesign { set; get; }

    private void ReadLevel(int number)
    {
        //get file from its directory or path
        string readFromFilePath = Application.dataPath + "/MiniGame1Levels/Level" + number + ".txt";

        //int c_i = 0;

        List<int> list = new List<int>();

        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        for (int i = 0; i < 8; ++i)
        {
            for(int j = 0; j < 8; ++j)
            {
                leveldesign[i,j] = (int)fileLines[i][j*2]-'0';
            }
        }
    }

    private void SpawnAllLevel()
    {
        activePiece = new List<GameObject>();
        Marbles = new Marble[8, 8];
        leveldesign = new int[8, 8];
        ReadLevel(1);
        int itx = 0;
        int ity = 0;
        foreach (int obj in leveldesign)
        {
            if((obj==0)||(obj==1)){
                SpawnSFTile(obj, GetTileSFCenter(itx ,7- ity+1));
            }
            else if((obj==2)||(obj==3)){
                SpawnLBTile(obj, GetTileCenter(itx,7- ity));
            }
            else if((obj==4)||(obj==5)){
                SpawnPiece(obj,itx,7- ity);
            }
            itx +=1;
            if (itx%8 == 0){
                ity+=1;
                itx = 0;
            }
        }
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
