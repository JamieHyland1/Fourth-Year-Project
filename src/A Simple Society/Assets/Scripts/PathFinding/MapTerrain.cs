using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTerrain : MonoBehaviour
{

    public LayerMask Unwalkable;
    public Vector2 gridWorldSize;
    public float cellRadius;
    Cell[,]grid;

    float cellDiameter;
    public int gridSizeX, gridSizeY;
    float gridSizeXf = 0.0f;
    float gridSizeYf = 0.0f;

    public List<Vector3> p;

    public GameObject start;
    public GameObject end;

    void Awake(){
        p = new List<Vector3>();
        cellDiameter = cellRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/cellDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/cellDiameter);
        grid = new Cell[gridSizeX,gridSizeY];
        createGrid();
        
    }

  public Vector3 GetRandomPosition(){
      float x = Random.Range(-gridWorldSize.x/2f,gridWorldSize.x/2f);
      float y = Random.Range(-gridWorldSize.y/2f,gridWorldSize.y/2f);
      return new Vector3(x,1.0f,y);
  }
    void createGrid(){
        for(int i = (-gridSizeX/2); i < gridSizeX/2; i++){
            for(int j = (gridSizeY/2); j > (-gridSizeY/2);j--){
                int x = i+gridSizeX/2;
                int y = Mathf.Abs(j - gridSizeY/2);
                bool walkable = !Physics.CheckSphere(new Vector3(i,1,j),1,Unwalkable);
                grid[x,y] = new Cell(new Vector3(i,1,j),walkable,x,y);      
                   
            }
        }
    }

    public int MapSize(){
            return gridSizeX*gridSizeY;
    }

    public Cell GetCellFromWorldPos(Vector3 pos){
        int i = Mathf.Abs(Mathf.RoundToInt(Mathf.Clamp(pos.x,-gridSizeX/2,(gridSizeX/2)-1))+(gridSizeX/2)-1);
        int j = Mathf.Abs(Mathf.RoundToInt(Mathf.Clamp(pos.z,-gridSizeY/2,(gridSizeY/2)-1))-(gridSizeY/2)+1);
       // Debug.Log(i + " " + j);
        
        return grid[i,j];
    }
    public Cell GetCellFromGrid(int i, int j){
        if(i < grid.GetLength(0) && i >= 0 && j < grid.GetLength(1) && j >= 0){
            return grid[i,j];
        }
        return null;
    }

    public List<Cell> GetNeighbours(int i, int j){
        List<Cell> neighbours = new List<Cell>();
        
        if(i-1 >=0 && j-1 >= 0)neighbours.Add(grid[i-1,j-1]);
        if(i < grid.GetLength(0) && j-1 >= 0)neighbours.Add(grid[i,j-1]);
        if(i+1 < grid.GetLength(0) && j-1 >= 0)neighbours.Add(grid[i+1,j-1]);

        if(i-1 >=0)neighbours.Add(grid[i-1,j]);
        if(i+1 < grid.GetLength(0))neighbours.Add(grid[i+1,j]);

        if(i-1 >=0 && j+1 < grid.GetLength(1))neighbours.Add(grid[i-1,j+1]);
        if(i < grid.GetLength(0) &&j+1 < grid.GetLength(1))neighbours.Add(grid[i,j+1]);
        if(i+1 < grid.GetLength(0) && j+1 < grid.GetLength(1))neighbours.Add(grid[i+1,j+1]);


        return neighbours;
    }
}