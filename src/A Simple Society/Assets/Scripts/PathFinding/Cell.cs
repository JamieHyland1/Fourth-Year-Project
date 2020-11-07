using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : IHeapItem<Cell>
{
    public int heapindex;
    public int i;
    public int j;
    public float gScore;
    public float hScore;

    public Cell parent;

    public bool current = false;

    public Vector3 position;

    public bool walkable = true;

    public Cell(){
        //Default Constructor
    }
    public Cell(Vector3 position, bool walkable){
        this.position = position;
        this.walkable = walkable;
    }
    public Cell(Vector3 position, bool walkable, int i, int j){
        this.position = position;
        this.walkable = walkable;
        this.i = i;
        this.j = j;
    }

    public float fScore{
        get{
            return gScore+hScore;
        }
    }

    public int CompareTo(Cell cell)
    {
        int compare = fScore.CompareTo(cell.fScore);
        if(compare == 0){
            compare = hScore.CompareTo(cell.hScore);
        }
        return -compare;
    }

    public int heapIndex{
        get{
            return heapindex;
        }
        set{
            heapindex = value;
        }
    }

}
