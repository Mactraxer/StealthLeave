using UnityEngine;
using System.Collections.Generic;

public class Rectangle
{
    public int x, y, width, height;

    public Rectangle(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}

public class Point
{

    public int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
public class Leaf
{
    private const int minLeafSize = 3;

    public Rectangle size;

    public Leaf leftChild;
    public Leaf rightChild;
    public Rectangle hall;


    public Leaf(Rectangle size)
    {
        this.size = size;
    }

    public bool Split()
    {
        if (leftChild != null && rightChild != null)
        {
            return false;
        }

        bool splitH = Random.Range(0f, 1f) > 0.5f;

        if (size.width > size.height && size.width / size.height >= 1.25)
        {
            splitH = false;
        } 
        else if (size.height > size.width && size.height / size.width >= 1.25)
        {
            splitH = true;
        }

        int max = (splitH ? size.height : size.width) - minLeafSize;

        if (max <= minLeafSize)
        {
            return false;
        }

        int split = Random.Range(minLeafSize, max);

        if (splitH == true)
        {
            leftChild = new Leaf(new Rectangle(size.x, size.y, size.width, split));
            rightChild = new Leaf(new Rectangle(size.x, size.y + split, size.width, size.height - split));
        }
        else
        {
            leftChild = new Leaf(new Rectangle(size.x, size.y, split, size.height));
            rightChild = new Leaf(new Rectangle(size.x + split, size.y, size.width - split, size.height));
        }

        CreateHall(leftChild.size, rightChild.size);

        return true;

    }

    public void CreateHall(Rectangle lRoom, Rectangle rRoom)
    {
        if (lRoom == null || rRoom == null) return;

        if (lRoom.x == rRoom.x)
        {
            int hallWidth = Random.Range(1, lRoom.width - 1);
            hall = new Rectangle((lRoom.x + lRoom.width / 2) - hallWidth / 2, rRoom.y, hallWidth, 1);
        } 
        else if (lRoom.y == rRoom.y)
        {
            int hallHeight = Random.Range(1, rRoom.height - 1);
            hall = new Rectangle(rRoom.x, (rRoom.y + rRoom.height / 2) - hallHeight / 2, 1, hallHeight);
        }

    }

}