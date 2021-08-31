using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField]
    int levelWidth;

    [SerializeField]
    int levelHeight;

    private int maxLeafSize = 3;

    private List<Leaf> leafs = new List<Leaf>();

    public delegate void Action(List<Leaf> level, int width, int height);
    public static event Action OnAction;

    // Start is called before the first frame update
    void Start()
    {
        Leaf root = new Leaf(new Rectangle(0, 0, levelWidth, levelHeight));
        leafs.Add(root);
        bool didSplit = true;

        while (didSplit)
        {
            didSplit = false;

            foreach (var item in leafs.ToArray())
            {
                if (item.leftChild == null && item.rightChild == null)
                {
                    if (item.size.width > maxLeafSize || item.size.height > maxLeafSize || Random.Range(0f, 1f) > 0.75f)
                    {
                        if (item.Split())
                        {
                            leafs.Add(item.leftChild);
                            leafs.Add(item.rightChild);
                            didSplit = true;
                        }
                    }
                }
            }
        }
        
        OnAction(leafs, levelWidth, levelHeight);
    }

}
