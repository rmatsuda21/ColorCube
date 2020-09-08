using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public enum Move {UP, DOWN, LEFT, RIGHT};

    public static Manager instance;
    private FileManager fileManager;

    private GameObject tilePrefab;

    private GameObject playerPrefab;
    private GameObject[] player;

    private GameObject boardParent;

    public Color[] cols;
    private Color defaultColor = Color.white;

    public List<int> levelCols;

    private List<int[][]> boards;
    private List<int[][]> playerLocations;

    private List<GameObject> currentTiles;

    private bool[][] activeTiles;

    private int currBoardIndex = 3;

    public float cellSize = 1f;

    private List<int[]> playerCols;
    // private int[] playerCols = { -1, -1, -1, -1, -1, -1 }; //Front, Right, Back, Left, Top, Bottom
    // private int[] playerCols = { 0, 0, 0, 0, 0, 0 }; //Front, Right, Back, Left, Top, Bottom
    public int playerX, playerY;

    private bool levelStart = false;

    //PS to show player location
    public GameObject playerPS;

    //UI
    public MainMenu mainMenu;
    public MainUI mainUI;
    public GameObject gameBtns;

    public GameObject colorSelector;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        playerPrefab = Resources.Load("Player", typeof(GameObject)) as GameObject;
        tilePrefab = Resources.Load("Tile", typeof(GameObject)) as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        setupGame();
    }

    private void Update()
    {
        gameBtns.transform.GetChild(0).GetComponent<Button>().interactable = canStartLevel();
    }

    public void setupGame()
    {
        mainUI.setActive(MainUI.Elements.None);

        fileManager = GetComponent<FileManager>();

        boardParent = new GameObject("BoardParent");
        boardParent.transform.position = Vector3.zero;

        //boards = getBoards();
        //playerLocations = getLocations();

        //Hard code for now

        boards = new List<int[][]>();
        playerLocations = new List<int[][]>();
        playerCols = new List<int[]>();

        boards.Add(new int[][]{
            new int[]{0,0,0},
            new int[]{0,-1,0},
            new int[]{0,0,0}
        });
        boards.Add(new int[][]
        {
            new int[]{ -1, -1, -1,  1,  -1, -1, -1 },
            new int[]{ -1,  0,  0,  0,  0,  0, -1 },
            new int[]{ -1,  0,  0, -1,  0,  0, -1 },
            new int[]{ -1,  0, -1, -1, -1,  0,  1 },
            new int[]{  1,  0,  0, -1,  0,  0, -1 },
            new int[]{ -1,  0,  0,  0,  0,  0, -1 },
            new int[]{ -1, -1, -1,  1, -1, -1, -1 }
        });
        boards.Add(new int[][]
        {
            new int[]{ -1, -1, -1, 0, 0, 0, 5 },
            new int[]{ 0, 0, 0, 0, 0, 0, 5 },
            new int[]{ 0, 3, 0, 4, 4, 4, 0 },
            new int[]{ 0, 3, 0, 0, 0, 0, 0 },
            new int[]{ 0, 3, 0, -1, -1, 0, -1 },
            new int[]{ 0, 0, 0, 2, -1, 0, -1 },
            new int[]{ 0, 0, 0, -1, -1, 0, -1 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 }
        });
        boards.Add(new int[][]
        {
            new int[]{ -1, -1, -1, 0, 0, 0, 0 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 },
            new int[]{ 0, 0, 0, 0, 0, 0, 0 },
            new int[]{ -1, -1, -1, -1, -1, -1, -1 }
        });
        boards.Add(new int[][]
        {
            new int[]{ -1, -1, -1, 2, 2, 2, 2 },
            new int[]{ -1, -1, -1, 1, -1, -1, -1 }
        });

        playerLocations.Add(new int[][] { new int[] { 0, 0 } });
        playerLocations.Add(new int[][] { new int[] { 1, 1 } });
        playerLocations.Add(new int[][] { new int[] { 2, 2 } });
        playerLocations.Add(new int[][] { new int[] { 4, 4 } });
        playerLocations.Add(new int[][] { new int[] { 0, 3 } });

        for(int i=0;i<boards.Count;++i){
            playerCols.Add(new int[] { -1, -1, -1, -1, -1, -1 });
        }

        fileManager.saveAllData(boards, fileManager.boardPath);
        fileManager.saveAllData(playerLocations, fileManager.locationPath);
        fileManager.saveAllData(playerCols, fileManager.colorPath);
    }

    private bool canStartLevel()
    {
        foreach (int i in playerCols[currBoardIndex])
            if (i == -1)
                return false;
        return true;
    }

    public void startLevel(int level)
    {
        currentTiles = new List<GameObject>();

        mainMenu.levelSelect.SetActive(false);
        mainUI.setActive(MainUI.Elements.LvlBtns);
        currBoardIndex = level;

        levelCols = new List<int>();

        player = new GameObject[playerLocations[currBoardIndex].Length];

        playerX = playerLocations[currBoardIndex][0][0];
        playerY = playerLocations[currBoardIndex][0][1];

        showPlayerLocation();

        activeTiles = new bool[boards[currBoardIndex].Length][];
        for (int j = 0; j < boards[currBoardIndex].Length; ++j)
        {
            activeTiles[j] = new bool[boards[currBoardIndex][j].Length];
        }
        activeTiles[playerX][playerY] = true;
        showBoard(currBoardIndex);

        colorSelector.GetComponent<ColorSelector>().createPallete();

        createPlayer();
        setPlayerActive(false);
    }

    public Color getColor(int index)
    {
        if (index == -1)
            return defaultColor;
        return cols[index];
    }

    void showPlayerLocation()
    {
        playerPS.transform.position = new Vector3((playerY - boards[currBoardIndex][0].Length / 2) * cellSize, 0, (boards[currBoardIndex].Length / 2 - playerX) * cellSize);
        ParticleSystem ps = playerPS.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        Color col = getColor(boards[currBoardIndex][playerX][playerY]);
        col.a = 1;
        main.startColor = col;

        foreach(Renderer rend in playerPS.GetComponentsInChildren<Renderer>())
        {
            rend.material.SetColor("_EmissionColor", col * Mathf.Pow(2, 1.5f));
            rend.material.EnableKeyword("_EMISSION");
        }

        playerPS.SetActive(true);
    }

    void createPlayer()
    {
        int i = 0;
        foreach (int[] location in playerLocations[currBoardIndex])
        {
            player[i] = Instantiate(playerPrefab);
            player[i++].GetComponent<CreateMesh>().CreateCube(playerCols[currBoardIndex], location[1], location[0], boards[currBoardIndex][0].Length, boards[currBoardIndex].Length, cellSize);
        }
    }

    void updatePlayerCols()
    {
        foreach(GameObject p in player)
        {
            p.GetComponent<CreateMesh>().setColors();
        }
    }

    void setPlayerActive(bool show)
    {
        foreach (GameObject p in player)
            p.SetActive(show);
    }

    List<int[][]> getBoards()
    {
        List<int[][]> boards = fileManager.loadAllData<int[][]>(fileManager.boardPath);
        return boards;
    }

    List<int[][]> getLocations()
    {
        List<int[][]> locations = fileManager.loadAllData<int[][]>(fileManager.locationPath);
        return locations;
    }

    void showBoard(int index)
    {
        if (index > boards.Count) return;

        int[][] board = boards[index];

        float boardSize = board.GetLength(0) * cellSize;

        createBoard(board);
        getBoardCols(board);
    }

    void createBoard(int[][] board)
    {
        for (int i = 0; i < board.Length; ++i)
        {
            for (int j = 0; j < board[i].Length; ++j)
            {
                if (board[i][j] != -1)
                {
                    //Create tile
                    GameObject tile = Instantiate(tilePrefab);
                    tile.transform.position = new Vector3((j - board[0].Length / 2) * cellSize, -cellSize, (board.Length / 2 - i) * cellSize);
                    tile.GetComponent<MeshRenderer>().material.color = getColor(board[i][j]);
                    tile.transform.parent = boardParent.transform;

                    currentTiles.Add(tile);
                }
                else
                {
                    GameObject tile = new GameObject("Empty");
                    tile.transform.parent = boardParent.transform;
                    activeTiles[i][j] = true;

                    currentTiles.Add(tile);
                }
            }
        }
    }

    void getBoardCols(int[][] board)
    {
        for (int i = 0; i < board.Length; ++i)
            for (int j = 0; j < board[i].Length; ++j)
                if (board[i][j] != -1 && levelCols.IndexOf(board[i][j]) == -1)
                        levelCols.Add(board[i][j]);
    }

    void clearTiles()
    {
        foreach (GameObject o in currentTiles)
        {
            Destroy(o);
        }
    }

    public void backToLvlSelect()
    {
        clearTiles();
        setPlayerActive(false);
        mainUI.setActive(MainUI.Elements.None);

        colorSelector.GetComponent<ColorSelector>().clearPallete();

        mainMenu.goToLevelSelect();
    }

    public void resetLevel()
    {
        playerX = playerLocations[currBoardIndex][0][0];
        playerY = playerLocations[currBoardIndex][0][1];

        foreach (GameObject p in player)
            Destroy(p);

        createPlayer();

        resetBoard();
    }

    public void resetBoard()
    {
        clearTiles();
        createBoard(boards[currBoardIndex]);
        showPlayerLocation();
        setPlayerActive(false);
        mainUI.setActive(MainUI.Elements.LvlBtns);
    }

    public void lightCurrentBlock()
    {
        lightBlock(playerX, playerY);
    }

    public void lightBlock(int x, int y)
    {
        GameObject tile = boardParent.transform.GetChild(y + x * boards[currBoardIndex][0].Length).gameObject;

        tile.GetComponent<Renderer>().material.SetColor("_EmissionColor", getColor(boards[currBoardIndex][x][y]) * Mathf.Pow(2, .3f));
        tile.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void checkLoss(int faceIndex)
    {
        int[][] board = boards[currBoardIndex];
        if (playerCols[currBoardIndex][faceIndex] != board[playerX][playerY])
        {
            foreach(GameObject p in player)
            {
                p.AddComponent<Rigidbody>();
                p.GetComponent<Rigidbody>().mass = Random.Range(.1f, 1f) * 10;
                p.GetComponent<Rigidbody>().useGravity = true;
                p.GetComponent<Rigidbody>().AddForce(new Vector3(0, Random.Range(50, 100), 0));
            }
            for(int i = 0; i < boardParent.transform.childCount; ++i)
            {
                Transform t = boardParent.transform.GetChild(i);
                if(t.name != "Empty")
                {
                    t.gameObject.AddComponent<Rigidbody>();
                    t.GetComponent<Rigidbody>().mass = Random.Range(.1f, 2f)*10;
                    t.GetComponent<Rigidbody>().useGravity = true;
                    t.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-50, 50), Random.Range(400, 500), Random.Range(-50, 50)));
                }
            }
            mainUI.setActive(MainUI.Elements.Lose);
        }
    }

    public void checkWin()
    {
        if (win())
            mainUI.setActive(MainUI.Elements.Win);
    }

    public bool win()
    {
        int[][] board = boards[currBoardIndex];
        for(int i = 0; i < board.Length; ++i)
        {
            for(int j = 0; j < board[i].Length; ++j)
            {
                if (!activeTiles[i][j])
                    return false;
            }
        }
        return true;
    }

    public void movePlayer(Move move, Player_Controller pc)
    {
        if (canMove(move)){
            activeTiles[playerX][playerY] = true;
            pc.movePlayer(move);
        }
    }

    bool canMove(Move move)
    {
        if (!levelStart) return false;

        if (move == Move.UP)
        {
            if (playerX == 0)
                return false;
            playerX--;
            if(boards[currBoardIndex][playerX][playerY] == -1)
            {
                playerX++;
                return false;
            }
        }
        if (move == Move.DOWN) {
            if(playerX == boards[currBoardIndex].Length - 1)
                return false;
            playerX++;
            if (boards[currBoardIndex][playerX][playerY] == -1)
            {
                playerX--;
                return false;
            }
        }
        if (move == Move.LEFT)
        {
            if (playerY == 0)
                return false;
            playerY--;
            if (boards[currBoardIndex][playerX][playerY] == -1)
            {
                playerY++;
                return false;
            }
        }
        if (move == Move.RIGHT)
        {
            if (playerY == boards[currBoardIndex][0].Length - 1)
                return false;
            playerY++;
            if (boards[currBoardIndex][playerX][playerY] == -1)
            {
                playerY--;
                return false;
            }
        }

        return true;
    }

    public int[][] getCurrentBoard()
    {
        return boards[currBoardIndex];
    }

    public GameObject getPlayerPrefab()
    {
        return playerPrefab;
    }

    public int[] getPlayerCols() { return playerCols[currBoardIndex]; }
    public void setPlayerCol(int face, int value) { playerCols[currBoardIndex][face] = value; }

    public void playLevel() { levelStart = true; mainUI.setActive(MainUI.Elements.None); setPlayerActive(true); playerPS.SetActive(false);
        playerX = playerLocations[currBoardIndex][0][0];
        playerY = playerLocations[currBoardIndex][0][1];
        clearActiveTiles();

        activeTiles[playerX][playerY] = true;

        checkLoss(5);
        checkWin();
        foreach(int[] loc in playerLocations[currBoardIndex])
        {
            lightBlock(loc[0], loc[1]);
        }
    }

    void clearActiveTiles(){
        int[][] board = boards[currBoardIndex];
        for (int i = 0; i < board.Length; ++i)
            for (int j = 0; j < board[i].Length; ++j)
                   activeTiles[i][j] = (board[i][j]==-1);
    }

    public void beginPallete(){
        StartCoroutine(palleteCamera(1, startPallete()));
    }

    public void endPallete() {
        StartCoroutine(palleteCamera(-1, end()));
    }

    public IEnumerator startPallete() { mainUI.setActive(MainUI.Elements.ColorSelector); setPlayerActive(false); yield return new WaitForEndOfFrame(); }
    public IEnumerator end() { mainUI.setActive(MainUI.Elements.LvlBtns); setPlayerActive(false); updatePlayerCols(); yield return new WaitForEndOfFrame(); }

    public AnimationCurve a;

    IEnumerator palleteCamera(int direction, IEnumerator c){
        Transform camera = GameObject.Find("Camera").transform;
        float animTime = .8f;
        float totTime = 0;

        Vector3 origPos = camera.position;
        Vector3 newPos = origPos + Vector3.left*4*direction;

        while(totTime<animTime) {
            totTime+=Time.deltaTime;

            float t = a.Evaluate(map(totTime, 0, animTime, 0, 1));

            camera.position = Vector3.Lerp(origPos, newPos, t);
            yield return new WaitForEndOfFrame();
        }
        
        StartCoroutine(c);
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
