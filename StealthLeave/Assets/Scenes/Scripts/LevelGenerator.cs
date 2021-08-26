using UnityEngine;
using UnityEngine.Tilemaps;

enum LevelSing: int
{
    block = 1, road = 0, enterDoor = -2, exitDoor = 2
}

public class LevelGenerator : MonoBehaviour
{

    [SerializeField]
    int levelWidth;

    [SerializeField]
    int levelHeight;

    [SerializeField]
    TileBase wallTile;

    [SerializeField]
    TileBase enterDoorTile;

    [SerializeField]
    TileBase exitDoorTile;

    [SerializeField]
    TileBase blockTile;

    [SerializeField]
    TileBase roadTile;

    private int blockSetPercent = 70;

    private Tilemap blockTilemap;
    private Tilemap doorsTilemap;

    private int[,] level = new int[10, 10];

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
        PrintLevel();
        CreateBackgroundTileSet();
        CreateBlockTileSet(level);
        CreateDoorsTileSet(level);
        //ApplyLevelToTileMap(GetComponent<Tilemap>(), level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyLevelToTileMap(Tilemap map, int[,] level)
    {
        map.size = new Vector3Int(10,10,0);
        for (int x = 0; x < level.GetUpperBound(0) + 1; x++)
        {
            for (int y = 0; y < level.GetUpperBound(1) + 1; y++)
            {
                switch ((LevelSing)level[x, y])
                {
                    case LevelSing.road:
                        map.SetTile(new Vector3Int(y - 5, level.GetUpperBound(1) - x - 5, 0), roadTile);
                        break;
                    case LevelSing.block:
                        map.SetTile(new Vector3Int(y - 5, level.GetUpperBound(1) - x - 5, 0), blockTile);
                        break;
                    case LevelSing.enterDoor:
                        map.SetTile(new Vector3Int(y - 5, level.GetUpperBound(1) - x - 5, 0), enterDoorTile);
                        break;
                    case LevelSing.exitDoor:
                        map.SetTile(new Vector3Int(y - 5, level.GetUpperBound(1) - x - 5, 0), exitDoorTile);
                        break;
                } 
            }
        }
        
    }

    private void GenerateEnterDoor(ref int[,] level)
    {
        int lockCoordinate = Random.Range(0, 1);

        if (lockCoordinate == 0)
        {
            int sideXCoodinate = Random.Range(0, 1);

            if (sideXCoodinate == 0)
            {
                level[0, Random.Range(0, level.GetUpperBound(1) + 1)] = (int)LevelSing.enterDoor;
            }
            else
            {
                level[level.GetUpperBound(0) + 1, Random.Range(0, level.GetUpperBound(1) + 1)] = (int)LevelSing.enterDoor;
            }
        } 
        else
        {
            int sideYCoodinate = Random.Range(0, 1);

            if (sideYCoodinate == 0)
            {
                level[Random.Range(0, level.GetUpperBound(0) + 1), 0] = (int)LevelSing.enterDoor;
            }
            else
            {
                level[Random.Range(0, level.GetUpperBound(0) + 1), level.GetUpperBound(1) + 1] = (int)LevelSing.enterDoor;
            }
        }
    }

    private void GenerateExitDoor(ref int[,] level)
    {
        int lockCoordinate = Random.Range(0, 1);

        if (lockCoordinate == 0)
        {
            int sideXCoodinate = Random.Range(0, 1);

            if (sideXCoodinate == 0)
            {
                level[0, Random.Range(0, level.GetUpperBound(1) + 1)] = (int)LevelSing.exitDoor;
            }
            else
            {
                level[level.GetUpperBound(0) + 1, Random.Range(0, level.GetUpperBound(1) + 1)] = (int)LevelSing.exitDoor;
            }
        }
        else
        {
            int sideYCoodinate = Random.Range(0, 1);

            if (sideYCoodinate == 0)
            {
                level[Random.Range(0, level.GetUpperBound(0) + 1), 0] = (int)LevelSing.exitDoor;
            }
            else
            {
                level[Random.Range(0, level.GetUpperBound(0) + 1), level.GetUpperBound(1) + 1] = (int)LevelSing.exitDoor;
            }
        }
    }

    private void GenerateLevel()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                int percent = Random.Range(1,100);

                if (x == 0 || y == 0 || x == levelWidth - 1 || y == levelHeight - 1)
                {
                    level[x, y] = (int)LevelSing.block;
                    continue;
                }

                if (percent > blockSetPercent)
                {
                    level[x, y] = (int)LevelSing.block;
                }
                else
                {
                    level[x, y] = (int)LevelSing.road;
                }
            }
        }

        GenerateEnterDoor(ref level);
        GenerateExitDoor(ref level);
    }

    private void CreateDoorsTileSet(int[,] level)
    {
        GameObject doorsTilemapGameObject = new GameObject("doorsTilemap");
        Tilemap doorsTilemap = doorsTilemapGameObject.AddComponent<Tilemap>();
        TilemapRenderer doorsTilemapRenderer = doorsTilemapGameObject.AddComponent<TilemapRenderer>();

        doorsTilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0);
        doorsTilemapGameObject.transform.SetParent(this.transform);
        doorsTilemapRenderer.sortingLayerName = "Main";

        this.doorsTilemap = doorsTilemap;

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (level[x, y] == (int)LevelSing.enterDoor)
                {
                    doorsTilemap.SetTile(new Vector3Int(y - 5, x - 5, 0), enterDoorTile);
                }
                else if (level[x, y] == (int)LevelSing.exitDoor)
                {
                    doorsTilemap.SetTile(new Vector3Int(y - 5, x - 5, 0), exitDoorTile);
                }

            }
        }
    }

    private void CreateBlockTileSet(int[,] level)
    {
        GameObject blockTilemapGameObject = new GameObject("blockTilemap");
        Tilemap blockTilemap = blockTilemapGameObject.AddComponent<Tilemap>();
        TilemapRenderer blockTilemapRenderer = blockTilemapGameObject.AddComponent<TilemapRenderer>();

        blockTilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0);
        blockTilemapGameObject.transform.SetParent(this.transform);
        blockTilemapRenderer.sortingLayerName = "Main";

        this.blockTilemap = blockTilemap;

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (level[x, y] == (int)LevelSing.block)
                {
                    blockTilemap.SetTile(new Vector3Int(y - 5, x - 5, 0), blockTile);
                }
                
            }
        }
    }

    private void CreateBackgroundTileSet()
    {
        GameObject backgroundTilemapGameObject = new GameObject("backgroundTilemap");
        Tilemap backgroundTilemap = backgroundTilemapGameObject.AddComponent<Tilemap>();
        TilemapRenderer backgroundTilemapRenderer = backgroundTilemapGameObject.AddComponent<TilemapRenderer>();

        backgroundTilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0);
        backgroundTilemapGameObject.transform.SetParent(this.transform);
        backgroundTilemapRenderer.sortingLayerName = "Main";

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++) 
            {
                backgroundTilemap.SetTile(new Vector3Int(y - 5, x - 5, 0), roadTile);
            }
        }
    }

    private void CreateNotMovementTileSet()
    {

    }

    private void PrintLevel()
    {
        string row = "";
        for (int x = 0; x < levelWidth; x++)
        {
            
            for (int y = 0; y < levelHeight; y++)
            {
                row += $"{level[x, y]} ";
            }
            row += "\n";
        }

        Debug.Log(row);
    }
}
