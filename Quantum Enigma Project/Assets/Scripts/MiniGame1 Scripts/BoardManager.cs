using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public GameObject gob;

    public static BoardManager Instance{set;get;}
    private bool[,] allowedMoves{set;get;}
    public Marble[,] Marbles {set;get;}
    public Marble selectedMarble;


    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const float SF_TILE_OFFSET = 0.2f;

    private int selectionX = -1;
    private int selectionY = -1;

    private Quaternion tileOrientation = Quaternion.Euler(90, 0, 0);

    public int moves_left = 20;

    public int level_number = 1;

    public static int GridSize = 8;

    public List<GameObject> boardPiecesPrefabs;
    public List<GameObject> activePiece;
    private int strtx = 0;
    private int strty = 0;

    private int finx = 0;
    private int finy = 0;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GridSize = 8;
        if(ClearBoards.won1){
            level_number+=1;
        }
        if(ClearBoards.won2){
            level_number+=1;
        }
        if(ClearBoards.won3){
            level_number+=1;
        }
        if(ClearBoards.won4){
            level_number+=1;
        }
        NumOfLevel.leveln = level_number;
        LevelMovesLeft.movesl = 0 + BoardManager.Instance.moves_left;
        //Cursor.lockState = CursorLockMode.None;
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




    bool isPath(Marble[,] matrix,int x, int y, int fx, int fy, int n)
{
     
    // Defining visited array to keep
    // track of already visited indexes
    bool[,] visited = new bool[n, n];
     
    // Flag to indicate whether the
    // path exists or not
    bool flag = false;
 
    for(int i = 0; i < n; i++)
    {
        for(int j = 0; j < n; j++)
        {
             
            // If matrix[i][j] is source
            // and it is not visited
            if (matrix[i, j] != null && i == x && j==y &&
              !visited[i, j])
               
                // Starting from i, j and
                // then finding the path
                if (isPath(matrix,fx,fy, i, j,
                           visited))
                {
                     
                    // If path exists
                    flag = true;
                    break;
                }
        }
    }
    if (flag){
            //Debug.Log("YES");
            Cursor.lockState = CursorLockMode.Locked;
            //SceneManager.LoadScene(1);
            return true;
        }
    else{
        //Debug.Log("NO");
        return false;
        }
}
 
// Method for checking boundaries
public bool isSafe(int i, int j,
                          Marble[,] matrix)
{
    if (i >= 0 && i < matrix.GetLength(0) &&
        j >= 0 && j < matrix.GetLength(1))
        return true;
         
    return false;
}
 
// Returns true if there is a path from
// a source (a cell with value 1) to a
// destination (a cell with value 2)
public bool isPath(Marble[,] matrix,int fx,int fy, int i,
                          int j, bool[,] visited)
{
     
    // Checking the boundaries, walls and
    // whether the cell is unvisited
    if (isSafe(i, j, matrix) &&
           matrix[i, j] != null &&
         !visited[i, j])
    {
         
        // Make the cell visited
        visited[i, j] = true;
 
        // If the cell is the required
        // destination then return true
        if (matrix[i, j] != null && i == fx && j==fy)
            return true;
 
        // Traverse up
        bool up = isPath(matrix, fx,fy, i - 1,
                         j, visited);
 
        // If path is found in up
        // direction return true
        if (up)
            return true;
 
        // Traverse left
        bool left = isPath(matrix,fx,fy, i,
                           j - 1, visited);
 
        // If path is found in left
        // direction return true
        if (left)
            return true;
 
        // Traverse down
        bool down = isPath(matrix,fx,fy, i + 1,
                           j, visited);
 
        // If path is found in down
        // direction return true
        if (down)
            return true;
 
        // Traverse right
        bool right = isPath(matrix, fx,fy,i, j + 1,
                            visited);
 
        // If path is found in right
        // direction return true
        if (right)
            return true;
    }
     
    // No path has been found
    return false;
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
        if (allowedMoves[x, y] == false)
        {
            BoardHighlights.Instance.HideHighlights();
            selectedMarble = null; 
            return;
        }
        if(selectedMarble.GetType() == typeof(EntangledMarble)){
            Marble c = null;
            List<Marble> existingOnes = new List<Marble>();
            existingOnes.Add(selectedMarble);
            foreach ( Marble mar in Marbles)
            {
                if (mar != null && mar!=selectedMarble) existingOnes.Add(mar);
                //existingOnes.Reverse();
            }
            if (addx == 1)
            {
                existingOnes.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
            }
            if (addx == -1)
            {
                existingOnes.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
                existingOnes.Reverse();
            }
            if (addy == 1)
            {
                existingOnes.Sort((a, b) => a.CurrentY.CompareTo(b.CurrentY));
            }
            if (addy == -1)
            {
                existingOnes.Sort((a, b) => a.CurrentY.CompareTo(b.CurrentY));
                existingOnes.Reverse();
            }
            foreach (Marble mar in existingOnes)
            {
                if(mar!=null && mar != c){
                if(mar.GetType()== typeof(EntangledMarble)){
                    int newx = mar.CurrentX-addx;
                    int newy = mar.CurrentY-addy;
                    if(newx>=0 && newx <GridSize && newy >= 0 && newy< GridSize && leveldesign[(GridSize-1)-newy,newx]!= 3){
                    if(mar.PossibleMove()[newx , newy]){
                        Marbles[mar.CurrentX, mar.CurrentY] = null;
                        mar.transform.position = GetTileCenter(newx,newy);
                        mar.SetPosition(newx,newy);
                        Marbles[newx,newy] = mar;
                        if(leveldesign[(GridSize-1)-newy,newx]== 2){
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
       else if (selectedMarble.GetType() == typeof(MagneticMarbleRed))
        {
            Marble c = null;
            List<Marble> existingOnes = new List<Marble>();
            existingOnes.Add(selectedMarble);
            foreach (Marble mar in Marbles)
            {
                if (mar != null && mar != selectedMarble) existingOnes.Add(mar);
            }
            if (addx == 1)
            {
                existingOnes.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
            }
            if (addx == -1)
            {
                existingOnes.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
                existingOnes.Reverse();
            }
            if (addy == 1)
            {
                existingOnes.Sort((a, b) => a.CurrentY.CompareTo(b.CurrentY));
            }
            if (addy == -1)
            {
                existingOnes.Sort((a, b) => a.CurrentY.CompareTo(b.CurrentY));
                existingOnes.Reverse();
            }
            foreach (Marble mar in existingOnes)
            {
                if (mar != null && mar != c)
                {
                    if (mar.GetType() == typeof(MagneticMarbleRed))
                    {
                        int newx = mar.CurrentX - addx;
                        int newy = mar.CurrentY - addy;
                        if (newx >= 0 && newx < GridSize && newy >= 0 && newy < GridSize && leveldesign[(GridSize-1) - newy, newx] != 3)
                        {
                            if (mar.PossibleMove()[newx, newy])
                            {
                                Marbles[mar.CurrentX, mar.CurrentY] = null;
                                mar.transform.position = GetTileCenter(newx, newy);
                                mar.SetPosition(newx, newy);
                                Marbles[newx, newy] = mar;
                                if (leveldesign[(GridSize-1) - newy, newx] == 2)
                                {
                                    activePiece.Remove(mar.gameObject);
                                    Marbles[newx, newy] = null;
                                    Destroy(mar.gameObject);
                                }
                            }
                        }
                    }
                    if (mar.GetType() == typeof(MagneticMarbleBlue))
                    {
                        int newx = mar.CurrentX + addx;
                        int newy = mar.CurrentY + addy;
                        if (newx >= 0 && newx < GridSize && newy >= 0 && newy < GridSize && leveldesign[(GridSize-1) - newy, newx] != 3)
                        {
                            if (mar.PossibleMove()[newx, newy])
                            {
                                Marbles[mar.CurrentX, mar.CurrentY] = null;
                                mar.transform.position = GetTileCenter(newx, newy);
                                mar.SetPosition(newx, newy);
                                Marbles[newx, newy] = mar;
                                if (leveldesign[(GridSize-1) - newy, newx] == 2)
                                {
                                    activePiece.Remove(mar.gameObject);
                                    Marbles[newx, newy] = null;
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
        else if (selectedMarble.GetType() == typeof(MagneticMarbleBlue))
        {
            Marble c = null;
            List<Marble> existingOnes = new List<Marble>();
            existingOnes.Add(selectedMarble);
            foreach (Marble mar in Marbles)
            {
                if (mar != null && mar != selectedMarble) existingOnes.Add(mar);
            }
            if (addx == 1)
            {
                existingOnes.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
            }
            if (addx == -1)
            {
                existingOnes.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
                existingOnes.Reverse();
            }
            if (addy == 1)
            {
                existingOnes.Sort((a, b) => a.CurrentY.CompareTo(b.CurrentY));
            }
            if (addy == -1)
            {
                existingOnes.Sort((a, b) => a.CurrentY.CompareTo(b.CurrentY));
                existingOnes.Reverse();
            }
            foreach (Marble mar in existingOnes)
            {
                if (mar != null && mar != c)
                {
                    if (mar.GetType() == typeof(MagneticMarbleBlue))
                    {
                        int newx = mar.CurrentX - addx;
                        int newy = mar.CurrentY - addy;
                        if (newx >= 0 && newx < GridSize && newy >= 0 && newy < GridSize && leveldesign[(GridSize-1) - newy, newx] != 3)
                        {
                            if (mar.PossibleMove()[newx, newy])
                            {
                                Marbles[mar.CurrentX, mar.CurrentY] = null;
                                mar.transform.position = GetTileCenter(newx, newy);
                                mar.SetPosition(newx, newy);
                                Marbles[newx, newy] = mar;
                                if (leveldesign[(GridSize-1) - newy, newx] == 2)
                                {
                                    activePiece.Remove(mar.gameObject);
                                    Marbles[newx, newy] = null;
                                    Destroy(mar.gameObject);
                                }
                            }
                        }
                    }
                    if (mar.GetType() == typeof(MagneticMarbleRed))
                    {
                        int newx = mar.CurrentX + addx;
                        int newy = mar.CurrentY + addy;
                        if (newx >= 0 && newx < GridSize && newy >= 0 && newy < GridSize && leveldesign[(GridSize-1) - newy, newx] != 3)
                        {
                            if (mar.PossibleMove()[newx, newy])
                            {
                                Marbles[mar.CurrentX, mar.CurrentY] = null;
                                mar.transform.position = GetTileCenter(newx, newy);
                                mar.SetPosition(newx, newy);
                                Marbles[newx, newy] = mar;
                                if (leveldesign[(GridSize-1) - newy, newx] == 2)
                                {
                                    activePiece.Remove(mar.gameObject);
                                    Marbles[newx, newy] = null;
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
        else if(allowedMoves[x,y] && leveldesign[(GridSize-1)-y,x]!= 3 )
        {
            Marbles[selectedMarble.CurrentX, selectedMarble.CurrentY] = null;
            selectedMarble.transform.position = GetTileCenter(x, y);
            selectedMarble.SetPosition(x,y);
            Marbles[x, y] = selectedMarble;
            if(leveldesign[(GridSize-1)-y,x]== 2){
                activePiece.Remove(selectedMarble.gameObject);
                Destroy(selectedMarble.gameObject);
            }
            moves_left -= 1;
            
        }
        BoardHighlights.Instance.HideHighlights();
        selectedMarble = null;
        if(isPath(Marbles,strtx,strty,finx,finy,GridSize)){
            if(level_number<=5){
            level_number+=5;
            GridSize = 10;
            Switchboard.HideEight();
            gob.SetActive(true);    
            foreach (GameObject ob in activePiece)
            {
                Destroy(ob);
            }
            }
            else if (level_number == 6)
            {
                SceneManager.LoadScene(3);
            }
            else if(level_number == 7){
                SceneManager.LoadScene(4);
            }
            else if(level_number == 8){
                SceneManager.LoadScene(5);
            }
            else if(level_number == 9){
                SceneManager.LoadScene(6);
            }
            else if(level_number == 10){
                SceneManager.LoadScene(12);
            }
                LevelMovesLeft.movesl += 111;
                NumOfLevel.leveln = level_number;
                SpawnAllLevel();
        }
        LevelMovesLeft.movesl = moves_left;
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
    6 = double marble
    7 = magnetic marble red
    8 = magnetic marble blue
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

        moves_left = Int16.Parse(fileLines[0]);
        LevelMovesLeft.movesl = moves_left;

        for (int i = 0; i < GridSize; ++i)
        {
            for(int j = 0; j < GridSize; ++j)
            {
                leveldesign[i,j] = (int)fileLines[i+1][j*2]-'0';
            }
        }
    }

    public void SpawnAllLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        activePiece = new List<GameObject>();
        Marbles = new Marble[GridSize, GridSize];
        leveldesign = new int[GridSize, GridSize];
        ReadLevel(level_number);
        int itx = 0;
        int ity = 0;
        foreach (int obj in leveldesign)
        {
            if(obj==0){
                strtx = itx;
                strty = (GridSize-1)-ity;
                SpawnSFTile(obj, GetTileSFCenter(itx ,(GridSize-1)- ity+1));
            }
            else if(obj==1){
                finx = itx;
                finy = (GridSize-1)-ity;
                SpawnSFTile(obj, GetTileSFCenter(itx ,(GridSize-1)- ity+1));
                if(obj == 0)
                {
                    strtx = itx;
                    strty = (GridSize-1) - ity;
                }
                else
                {
                    finx = itx;
                    finy = (GridSize-1) - ity;
                }
            }
            else if((obj==2)||(obj==3)){
                SpawnLBTile(obj, GetTileCenter(itx,(GridSize-1)- ity));
            }
            else if( obj > 3 && obj < 9){
                SpawnPiece(obj,itx,(GridSize-1)- ity);
            }
            itx +=1;
            if (itx%GridSize == 0){
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
        Vector3 widthLine = Vector3.right * GridSize;
        Vector3 heightLine = Vector3.forward * GridSize;

        for (int i = 0; i <= GridSize; ++i)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= GridSize; j++)
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
