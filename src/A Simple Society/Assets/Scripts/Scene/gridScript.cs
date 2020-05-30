using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gridScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject tile;
     Ray r;
    RaycastHit hit;
    [SerializeField]
    GameObject visitedTile;

   [SerializeField] GameObject water;

    [SerializeField]
    GameObject landmass;
    float width;
    float height;
    [SerializeField]
    float size = 2;
    Vector3 prevScale;
    int w,h;

    public int maxX;
    public int maxY;

    public Vector3[,] grid;
    int[,] occupied;
    private GameObject[] tiles;

   public Boolean showGrid = false;

    void Awake(){
        width = (int)landmass.transform.localScale.x;
        height = (int)landmass.transform.localScale.z;
        w = (int)(width/size);
        h = (int)(height/size);
        occupied = new int[h,w];
        prevScale = landmass.transform.localScale;
        maxX = occupied.GetLength(0)-1;
        maxY = occupied.GetLength(1)-1;
      
        setOccupied();
        createGrid();
    }
    private bool V3Equal(Vector3 a, Vector3 b)
    {
         return Vector3.SqrMagnitude(a - b) < 9.99999944E-11f;
    }

    public void setOccupied(){
        for(int j = 0; j < occupied.GetLength(0); j++){
            for(int i = 0; i < occupied.GetLength(1); i++){
                occupied[j,i] = 0;
            }
        }
    }


//Subdivide the landmass into a grid of 3D positions for agents to move to
//Check using a raycast if there is ground underneath, if not specify that area as a place for water
    void createGrid(){
       grid = new Vector3[h,w];
       for(int j = 0; j < grid.GetLength(0); j++){
           for(int i = 0; i < grid.GetLength(1); i++){
                float x = (i-landmass.transform.localScale.x/2) + tile.transform.localScale.x/2;
                float z = (j-landmass.transform.localScale.z/2) + tile.transform.localScale.y/2;
                int occupiedVal = occupied[j,i];
                grid[j,i] = new Vector3(x,2.75f,z);
                  r = new Ray(new Vector3(x,2.75f,z),Vector3.down);
                 RaycastHit hit;
                 if(Physics.Raycast(r.origin, Vector3.down, out hit,2f)){
                    var currentTile = Instantiate(water,new Vector3(x, 2.75f,z), tile.transform.rotation);
                    currentTile.transform.parent = GameObject.FindGameObjectWithTag("Tiles").transform;
                    occupied[j,i] = -1;
                }
                if(showGrid){
                    if(occupied[j,i] == 1){
                        var currentTile = Instantiate(visitedTile,new Vector3(x, 2.75f,z), tile.transform.rotation);
                        currentTile.transform.parent = GameObject.FindGameObjectWithTag("Tiles").transform;
                    }else if(occupied[j,i] != -1){
                             var currentTile = Instantiate(tile,new Vector3(x, 2.75f,z), tile.transform.rotation);
                             currentTile.transform.parent = GameObject.FindGameObjectWithTag("Tiles").transform;
                    }
    
                }
           }
       }
    }

    void clearTiles(){
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        for(int i = tiles.Length-1; i > 0; i--){
            Destroy(tiles[i]);
        }
    }

    public void occupySpace(Vector2 pos ){
        int i = (int)pos.x;
        int j = (int)pos.y;
       
        occupied[j,i] = 1;
        if(showGrid)clearTiles();
        if(showGrid)createGrid();
    }

    public void leaveSpace(Vector2 pos){
        int i = (int)pos.x;
        int j = (int)pos.y;
    
        occupied[j,i] = 0;
        if(showGrid)clearTiles();
        if(showGrid)createGrid();
    }

    public Boolean isOccupied(Vector2 pos){
        if((pos.x >= 0.0 && pos.x <= maxX) && (pos.y >= 0 && pos.y <= maxY)){
            int i = (int)pos.x;
            int j = (int)pos.y;

            return occupied[j,i] == 1 || occupied[j,i] == -1;
        }else if(pos.x > maxX || pos.x < 0 || pos.y < 0 || pos.y > maxY){ //outside the bounds of the island
            return true;
        }
        return false;
    }

    public Vector3 getGridPosition(Vector2 pos){
        int i = (int)pos.x;
        int j = (int)pos.y;
       
        return grid[j,i];
    }

///////DEBUG ONLY/////////////////////////////////////////////////
//
//     void checkOccupied(){
//     int counter = 0;
//     for(int j = 0; j < grid.GetLength(0); j++){
//            for(int i = 0; i < grid.GetLength(1); i++){
//                if(occupied[j,i] == 1) counter++;
//         }
//      }
//      Debug.Log(counter + "places occupied currently");
//     }
//
///////////////////////////////////////////////////////////////////

}

