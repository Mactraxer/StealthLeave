using UnityEngine;

class Rectangle
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

class Point
{

    public int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
class Leaf
{
    private const int minLeafSize = 6;

    public Rectangle size;

    public Leaf leftChild;
    public Leaf rightChild;
    public Rectangle room;
    public Rectangle[] halls;

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
            splitH = true;
        } 
        else if (size.height > size.width && size.height / size.width >= 1.25)
        {
            splitH = false;
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

        return true;

    }

    public void CreateRoom()
    {
        if (leftChild != null || rightChild != null)
        {
            if (leftChild != null)
            {
                leftChild.CreateRoom();
            }
            else if (rightChild != null)
            {
                rightChild.CreateRoom();
            }

            if (leftChild != null && rightChild != null)
            {
                CreateHall(leftChild.GetRoom(), rightChild.GetRoom());
            }
        }
        else
        {
            Point roomSize;
            Point roomPos;

            //TODO Возможно стоит переделать. Нужли ли здесь Point?
            roomSize = new Point(Random.Range(3, size.width - 2), Random.Range(3, size.height - 2));
            roomPos = new Point(Random.Range(1, size.width - roomSize.x - 1), Random.Range(1, size.height - roomSize.y - 1));
            room = new Rectangle(size.x + roomPos.x, size.y + roomPos.y, roomSize.x, roomSize.y);
        }
    }

    public Rectangle GetRoom()
    {
        if (room != null)
        {
            return room;
        }
        else
        {
            Rectangle lRoom = null;
            Rectangle rRoom = null;

            if (leftChild != null)
            {
                lRoom = leftChild.GetRoom();
            }

            if (rightChild != null)
            {
                rRoom = rightChild.GetRoom();
            }

            if (lRoom == null && rRoom == null)
            {
                return null;
            }
            else if (rRoom == null)
            {
                return lRoom;
            }
            else if (lRoom == null)
            {
                return rRoom;
            }
            else if (Random.Range(0f,1f) > 0.5f)
            {
                return rRoom;
            }
            else
            {
                return lRoom;
            }
        }
    }

    public void CreateHall(Rectangle lRoom, Rectangle rRoom)
    {
        Rectangle[] halls = new Rectangle[2];

        Point point1 = new Point(Random.Range(lRoom.x + 1, lRoom.x + lRoom.width - 2), Random.Range(lRoom.y + 1, lRoom.y + lRoom.height - 2));
        Point point2 = new Point(Random.Range(rRoom.x + 1, rRoom.x + rRoom.width - 2), Random.Range(rRoom.y + 1, rRoom.y + rRoom.height - 2));

        int w = point2.x - point1.x;
        int h = point2.y - point1.y;

        if (w < 0)
        {
            if (h < 0)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    halls[0] = new Rectangle(point2.x, point1.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point2.x, point2.y, 1, Mathf.Abs(h));
                }
                else
                {
                    halls[0] = new Rectangle(point2.x, point2.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point1.x, point2.y, 1, Mathf.Abs(h));
                }
            }
            else if (h > 0)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    halls[0] = new Rectangle(point2.x, point1.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point2.x, point1.y, 1, Mathf.Abs(h));
                }
                else
                {
                    halls[0] = new Rectangle(point2.x, point2.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point1.x, point1.y, 1, Mathf.Abs(h));
                }
            }
            else
            {
                halls[0] = new Rectangle(point2.x, point2.y, Mathf.Abs(w), 1);
            }
        }
        else if (w > 0)
        {
            if (h < 0)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    halls[0] = new Rectangle(point1.x, point2.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point1.x, point2.y, 1, Mathf.Abs(h));
                }
                else
                {
                    halls[0] = new Rectangle(point1.x, point1.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point2.x, point2.y, 1, Mathf.Abs(h));
                }
            }
            else if (h > 0)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    halls[0] = new Rectangle(point1.x, point1.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point2.x, point1.y, 1, Mathf.Abs(h));
                }
                else
                {
                    halls[0] = new Rectangle(point1.x, point2.y, Mathf.Abs(w), 1);
                    halls[1] = new Rectangle(point1.x, point1.y, 1, Mathf.Abs(h));
                }
            }
            else
            {
                halls[0] = new Rectangle(point1.x, point1.y, Mathf.Abs(w), 1);
            }
        }
        else
        {
            if (h < 0)
            {
                halls[0] = new Rectangle(point2.x, point2.y, 1, Mathf.Abs(h));
            }
            else if (h > 0)
            {
                halls[0] = new Rectangle(point1.x, point1.y, 1, Mathf.Abs(h));
            }
        }
    }

}