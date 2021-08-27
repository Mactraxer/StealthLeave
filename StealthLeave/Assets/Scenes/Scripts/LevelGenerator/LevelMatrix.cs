using UnityEngine;

public class LevelMatrix : MonoBehaviour
{

    [SerializeField]
    int levelWidth;

    [SerializeField]
    int levelHeight;

    private int blockSetPercent = 70;
    private int[,] level = new int[10, 10];

    public delegate void Action(int[,] level);
    public static event Action OnAction;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
        PrintLevel(level);
        OnAction(level);
    }

    private void GenerateDoor(ref int[,] level, LevelSign door)
    {
        int lockCoordinate = Random.Range(0, 1);

        if (lockCoordinate == 0)
        {
            int sideXCoodinate = Random.Range(0, 1);

            if (sideXCoodinate == 0)
            {
                int x = 0;
                int y = Random.Range(0, level.GetUpperBound(1) + 1);
                level[x, y] = (int)door;
            }
            else
            {
                int x = level.GetUpperBound(0) + 1;
                int y = Random.Range(0, level.GetUpperBound(1) + 1);
                level[x, y] = (int)door;
            }
        }
        else
        {
            int sideYCoodinate = Random.Range(0, 1);

            if (sideYCoodinate == 0)
            {
                int x = Random.Range(0, level.GetUpperBound(0) + 1);
                int y = 0;
                level[x, y] = (int)door;
            }
            else
            {
                int x = Random.Range(0, level.GetUpperBound(0) + 1);
                int y = level.GetUpperBound(1) + 1;
                level[x, y] = (int)door;
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
                    level[x, y] = (int)LevelSign.wall;
                    continue;
                }

                if (percent > blockSetPercent)
                {
                    level[x, y] = (int)LevelSign.block;
                }
                else
                {
                    level[x, y] = (int)LevelSign.road;
                }
            }
        }

        GenerateDoor(ref level, LevelSign.enterDoor);
        GenerateDoor(ref level, LevelSign.exitDoor);
    }

    private void PrintLevel(int[,] level)
    {
        string row = "";
        for (int x = 0; x < level.GetUpperBound(0) + 1; x++)
        {
            
            for (int y = 0; y < level.GetUpperBound(1) + 1; y++)
            {
                row += $"{level[x, y]} ";
            }
            row += "\n";
        }

        Debug.Log(row);
    }

}
