using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField]
    int levelWidth;

    [SerializeField]
    int levelHeight;

    [SerializeField]
    TileBase blockTile;

    [SerializeField]
    TileBase roadTile;

    private int blockSetPercent = 70;

    [SerializeField]
    private int[,] level = new int[10, 10];

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
        PrintLevel();
        ApplyLevelToTileMap(GetComponent<Tilemap>(), level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyLevelToTileMap(Tilemap map, int[,] level)
    {
        map.size = new Vector3Int(10,10,0);
        for (int x = 0; x < level.Rank; x++)
        {
            for (int y = 0; y < level.GetLength(x); y++)
            {
                if (level[x, y] == 0)
                {
                    map.SetTile(new Vector3Int(x, y, 0), roadTile);
                }
                else
                {
                    map.SetTile(new Vector3Int(x, y, 0), blockTile);
                }
                
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

                if (percent > blockSetPercent)
                {
                    level[x, y] = 1;
                }
                else
                {
                    level[x, y] = 0;
                }
            }
        }
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
