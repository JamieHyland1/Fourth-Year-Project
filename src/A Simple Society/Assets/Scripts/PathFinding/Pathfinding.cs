using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    MapTerrain map;
    [Range(0,1),SerializeField]
    List<Cell> path;
   
   public void getPath(PathRequest request, Action <PathResult> callback){
        List<Cell> path = new List<Cell>();
        Heap<Cell> openSet = new Heap<Cell>(map.MapSize());
        List<Cell> closedSet = new List<Cell>();
        Cell strt = map.GetCellFromWorldPos(request.start);
        Cell final = map.GetCellFromWorldPos(request.end);

        openSet.Add(strt);
        if(strt.walkable && final.walkable){
            while(openSet.Count > 0){
                Cell current = openSet.RemoveFirst();
                closedSet.Add(current);
                //openSet.Remove(current);

                if(current == final){
                    while(current.parent != null){
                        path.Add(current);
                        current = current.parent;
                    }
                    List<Cell> simplePath = SimplifyPath(path);
                    simplePath.Reverse();
                    callback(new PathResult(simplePath,true,request.callback));
                    break;
                }

                foreach(Cell c in map.GetNeighbours(current.i,current.j)){
                    if(c.walkable && !closedSet.Contains(c)){    
                        float tentativeScore = current.gScore + getDistance(current,c);
                        if(tentativeScore < c.gScore)c.gScore = tentativeScore;
                        c.hScore = getDistance(c,final);
                        c.parent = current;
                        if(!openSet.Contains(c))openSet.Add(c);else openSet.UpdateItem(c);
                    }
                }
            }
        }
       callback(new PathResult(path,false,request.callback));
   }

   public float getDistance(Cell start, Cell end){
       float dx = Mathf.Abs(start.position.x - end.position.x);
       float dy = Mathf.Abs(start.position.z - end.position.z);

       return (dx + dy) - 0.5857f * Mathf.Min(dx, dy);
   }

   public List<Cell> SimplifyPath(List<Cell> path){
       List<Cell> points = new List<Cell>();
       Vector2 oldDirection = Vector2.zero;
       for(int i = 1; i < path.Count;i++){
           Vector2 newDirection = new Vector2(path[i-1].i-path[i].i,path[i-1].j-path[i].j);
           if(newDirection != oldDirection){
               points.Add(path[i]);
           }
           oldDirection = newDirection;

       }
       return points;
   }
}
