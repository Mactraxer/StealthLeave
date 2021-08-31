using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

enum TilemapType
{
    block, road, door
}

public class LevelTilemapAssembler : MonoBehaviour
{
    [SerializeField]
    GameObject hero;

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
    
    // Start is called before the first frame update
    void Start()
    {
        LevelGenerator.OnAction += LevelGenerated;
    }

    private void LevelGenerated(List<Leaf> level, int width, int height)
    {
        CreateTileMaps();
        AssembleLevelByLeafs(level, width, height);
        
    }

    private void CreateTileMaps()
    {
        roadTilemap = CreateTilemap("roadTilemap", TilemapType.road);
        doorsTilemap = CreateTilemap("doorsTilemap", TilemapType.door);
        blockTilemap = CreateTilemap("blockTilemap", TilemapType.block);
    }

    private Tilemap CreateTilemap(string name, TilemapType type)
    {
        GameObject tilemapGameObject = new GameObject(name);
        Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapGameObject.AddComponent<TilemapRenderer>();

        tilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0);
        tilemapGameObject.transform.SetParent(this.transform);
        tilemapRenderer.sortingLayerName = "Main";

        switch (type)
        {
            case TilemapType.block:
                tilemapGameObject.AddComponent<TilemapCollider2D>();
                tilemapGameObject.layer = 6;//Notmovement layer
                break;
            case TilemapType.door:
                tilemapRenderer.sortingOrder = 1;
                break;
            default:
                break;
        }
        

        return tilemap;
    }

    private void AssembleLevelByLeafs(List<Leaf> leafs, int levelWidth, int levelHeight)
    {
        foreach (var item in leafs)
        {
            SetTilesToRectagle(item.size);

        }

        foreach (var item in leafs)
        {
            SetTileForHalls(item.hall);
        }

        GenerateDoor(levelWidth, levelHeight);
        FillBackgroundTile(levelWidth, levelHeight);
    }

    private void FillBackgroundTile(int levelWidth, int levelHeight)
    {
        for (int y = 1; y < levelHeight; y++)
        {
            for (int x = 1; x < levelWidth; x++)
            {
                if (blockTilemap.GetTile(new Vector3Int(y, x, 0)) == null)
                {
                    roadTilemap.SetTile(new Vector3Int(y, x, 0), roadTile);
                }

            }
        }
    }

    private void SetTileForHalls(Rectangle hall)
    {
        if (hall == null)
        {
            return;
        }


        for (int y = hall.y; y < hall.y + hall.height; y++)
        {
            for (int x = hall.x; x < hall.x + hall.width; x++)
            {
                blockTilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }


    }

    private void SetTilesToRectagle(Rectangle bounds)
    {
        if (bounds == null)
        {
            return;
        }

        for (int y = bounds.y; y <= bounds.y + bounds.height; y++)
        {
            for (int x = bounds.x; x <= bounds.x + bounds.width; x++)
            {
                if (y == bounds.y || y == bounds.y + bounds.height || x == bounds.x || x == bounds.x + bounds.width)
                {
                    blockTilemap.SetTile(new Vector3Int(x, y, 0), blockTile);
                }

            }
        }
    }


    private void GenerateDoor(int levelWidth, int levelHeight)
    {
        bool isVertical = Random.Range(0, 2) == 0;

        if (isVertical)
        {
            int topDoorXPosition = Random.Range(1, levelWidth);
            int bottomDoorXPosition = Random.Range(1, levelWidth);
            doorsTilemap.SetTile(new Vector3Int(topDoorXPosition, levelHeight, 0), enterDoorTile);
            doorsTilemap.SetTile(new Vector3Int(bottomDoorXPosition, 0, 0), enterDoorTile);

            TileBase mayBlockTile = blockTilemap.GetTile(new Vector3Int(topDoorXPosition, levelHeight - 1, 0));

            if (mayBlockTile == blockTile)
            {
                blockTilemap.SetTile(new Vector3Int(topDoorXPosition, levelHeight - 1, 0), null);
            }

            mayBlockTile = blockTilemap.GetTile(new Vector3Int(bottomDoorXPosition, 1, 0));

            if (mayBlockTile == blockTile)
            {
                blockTilemap.SetTile(new Vector3Int(bottomDoorXPosition, 1, 0), null);
            }

            GameObject instantiatedGameObjecHero = Instantiate(hero, new Vector3(bottomDoorXPosition + 0.5f, 1 + 0.5f, 0), Quaternion.identity);
            SpriteRenderer heroSpriteRenderer = instantiatedGameObjecHero.GetComponent<SpriteRenderer>();
            if (heroSpriteRenderer != null)
            {
                heroSpriteRenderer.sortingOrder = 2;
            }
        }
        else
        {
            int leftDoorYPosition = Random.Range(1, levelHeight);
            int rightDoorYPosition = Random.Range(1, levelHeight);
            doorsTilemap.SetTile(new Vector3Int(levelWidth, rightDoorYPosition, 0), enterDoorTile);
            doorsTilemap.SetTile(new Vector3Int(0, leftDoorYPosition, 0), enterDoorTile);

            TileBase mayBlockTile = blockTilemap.GetTile(new Vector3Int(1, leftDoorYPosition, 0));

            if (mayBlockTile == blockTile)
            {
                blockTilemap.SetTile(new Vector3Int(1, leftDoorYPosition, 0), null);

            }

            mayBlockTile = blockTilemap.GetTile(new Vector3Int(levelWidth - 1, rightDoorYPosition, 0));

            if (mayBlockTile == blockTile)
            {
                blockTilemap.SetTile(new Vector3Int(levelWidth - 1, rightDoorYPosition, 0), null);
            }

            GameObject instantiatedGameObjecHero = Instantiate(hero, new Vector3(1f + 0.5f, leftDoorYPosition + 0.5f, 0), Quaternion.identity);
            SpriteRenderer heroSpriteRenderer = instantiatedGameObjecHero.GetComponent<SpriteRenderer>();
            if (heroSpriteRenderer != null)
            {
                heroSpriteRenderer.sortingOrder = 2;
            }
        }



    }

}
