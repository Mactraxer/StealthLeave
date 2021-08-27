using UnityEngine.Tilemaps;
using UnityEngine;

public class LevelTilemapAssembler : MonoBehaviour
{
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

    private Tilemap blockTilemap;
    private Tilemap doorsTilemap;
    private Tilemap roadTilemap;
    private Tilemap wallTilemap;
    //TODO объеденить в один tilemap двери
    private Tilemap enterDoorTilemap;
    private Tilemap exitDoorTilemap;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelMatrix.OnAction += LevelMatrix_OnAction;
    }

    private void LevelMatrix_OnAction(int[,] level)
    {
        roadTilemap = CreateTilemap(level, LevelSign.road, "roadTilemap", roadTile);
        wallTilemap = CreateTilemap(level, LevelSign.wall, "wallTilemap", wallTile);
        enterDoorTilemap = CreateTilemap(level, LevelSign.enterDoor, "enterDoorTilemap", enterDoorTile);
        exitDoorTilemap = CreateTilemap(level, LevelSign.exitDoor, "exitDoorTilemap", exitDoorTile);
        blockTilemap = CreateTilemap(level, LevelSign.block, "blockTilemap", blockTile);
    }

    private Tilemap CreateTilemap(int[,] level,LevelSign levelSign, string name, TileBase tile)
    {
        GameObject tilemapGameObject = new GameObject(name);
        Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapGameObject.AddComponent<TilemapRenderer>();

        tilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0);
        tilemapGameObject.transform.SetParent(this.transform);
        tilemapRenderer.sortingLayerName = "Main";

        for (int y = 0; y < level.GetUpperBound(0) + 1; y++)
        {
            for (int x = 0; x < level.GetUpperBound(1) + 1; x++)
            {
                if (level[y, x] == (int)levelSign)
                {
                    int xCoordinate = x - (level.GetUpperBound(0) + 1) / 2;
                    int yCoordinate = level.GetUpperBound(1) / 2 - y;
                    tilemap.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), tile);
                }

            }
        }

        return tilemap;
    }

}
