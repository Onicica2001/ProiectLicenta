using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public Node parent;

    public int gCost, hCost, gridX, gridY;
    int heapIndex;

    public Node(bool walkable,Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
