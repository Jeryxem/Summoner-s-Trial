using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public const int ROW = 3;
    public const int COL = 6;
    public const float WIDTH = 3f;
    public const float HEIGHT = 2f;

    public static GridGenerator Instance;

    //Starting position of first generated tile
    private float startXpos = -7.5f;
    private float startYpos = 1.5f;
    

    public List<Tile> generatedTiles = new List<Tile>();
    [field: SerializeField] public Tile tilePrefab { get; set; }
    public List<CustomTileData> tilesDataList = new List<CustomTileData>();
    public struct CustomTileData
    {
        public TileOwner tileOwner;
        public bool canMoveLeft;
        public bool canMoveRight;
        public bool canMoveUp;
        public bool canMoveDown;
        public bool isOccupied;
        public int row, col;

        public CustomTileData(TileOwner _tileOwner, bool cmLeft, bool cmRight, bool cmUp, bool cmDown, bool _isOccupied, int _row, int _col)  
        {  
            tileOwner = _tileOwner;
            canMoveLeft = cmLeft;
            canMoveRight = cmRight;
            canMoveUp = cmUp;
            canMoveDown = cmDown;
            isOccupied = _isOccupied;
            row = _row;
            col = _col;
        } 
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (tilesDataList.Count == 0) { AddTileDataToList(); }
    }

    private void AddTileDataToList() {
        CustomTileData newTileData;

        //Row 1-3 Col 1
        newTileData = new CustomTileData(TileOwner.Player, false, true, false, true, false, 1, 1);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Player, false, true, true, true, false, 2, 1);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Player, false, true, true, false, false, 3, 1);
        tilesDataList.Add(newTileData);
        
        //Row 1-3 Col 2
        newTileData = new CustomTileData(TileOwner.Player, true, true, false, true, false, 1, 2);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Player, true, true, true, true, false, 2, 2);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Player, true, true, true, false, false, 3, 2);
        tilesDataList.Add(newTileData);

        //Row 1-3 Col 3
        newTileData = new CustomTileData(TileOwner.Player, true, true, false, true, false, 1, 3);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Player, true, true, true, true, false, 2, 3);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Player, true, true, true, false, false, 3, 3);
        tilesDataList.Add(newTileData);

        //Row 1-3 Col 4
        newTileData = new CustomTileData(TileOwner.Enemy, true, true, false, true, false, 1, 4);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Enemy, true, true, true, true, false, 2, 4);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Enemy, true, true, true, false, false, 3, 4);
        tilesDataList.Add(newTileData);

        //Row 1-3 Col 5
        newTileData = new CustomTileData(TileOwner.Enemy, true, true, false, true, false, 1, 5);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Enemy, true, true, true, true, false, 2, 5);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Enemy, true, true, true, false, false, 3, 5);
        tilesDataList.Add(newTileData);

        //Row 1-3 Col 6
        newTileData = new CustomTileData(TileOwner.Enemy, true, false, false, true, false, 1, 6);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Enemy, true, false, true, true, false, 2, 6);
        tilesDataList.Add(newTileData);
        newTileData = new CustomTileData(TileOwner.Enemy, true, false, true, false, false, 3, 6);
        tilesDataList.Add(newTileData);

        GenerateNewTileMap();
    }

    public void GenerateNewTileMap() {
        if (generatedTiles != null) { generatedTiles.Clear(); }
        
        float currentXpos = startXpos;
        float currentYpos = startYpos;
        int i = 0;
        for (int x = 0; x < COL; x++) {
            for (int y = 0; y < ROW; y++) {
                
                if (y % 3 == 0) { currentYpos = startYpos; }

                // print("xPos=" + currentXpos + " yPos=" + currentYpos);
                GenerateTile(currentXpos, currentYpos, tilesDataList[i].tileOwner, tilesDataList[i].canMoveLeft, tilesDataList[i].canMoveRight, 
                    tilesDataList[i].canMoveUp, tilesDataList[i].canMoveDown, tilesDataList[i].isOccupied, tilesDataList[i].row, tilesDataList[i].col);
                i++;
                currentYpos -= HEIGHT;

                // print("x=" + x + " y=" + y + " i=" + i);
            }
            
            if (x % 9 == 0) { currentXpos = startXpos; }
            currentXpos += WIDTH;
        }
    }

    private void GenerateTile(float xPos, float yPos, TileOwner tileOwner, bool cmLeft, bool cmRight, bool cmUp, bool cmDown, bool isOccupied, int row, int col){
        Tile newtile = Instantiate(tilePrefab, new Vector2(xPos, yPos), Quaternion.identity);
        newtile.SetTileData(tileOwner, cmLeft, cmRight, cmUp, cmDown, isOccupied, row, col);
        newtile.name = "Tile(" + row + "," + col + ")";
        generatedTiles.Add(newtile);
    }
}
