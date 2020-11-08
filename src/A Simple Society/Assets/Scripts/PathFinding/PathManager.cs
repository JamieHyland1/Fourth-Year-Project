using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
public class PathManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();
      static PathManager instance;

      public Pathfinding pathfinder;
    

      void Awake() {
          pathfinder = GetComponent<Pathfinding>();
          instance = this;
      }

      void Update(){
          if(results.Count > 0){
              int itemsInQueue = results.Count;
              lock(results){
                  for(int i = 0; i < itemsInQueue; i++){
                      PathResult result = results.Dequeue();
                      result.callback(result.path,result.success);
                  }
              }
          }
      }
    public static void RequestPath(PathRequest request){
       ThreadStart threadStart = delegate {
           instance.pathfinder.getPath(request,instance.finishedProcessingPath);
       };
       Thread thread = new Thread(threadStart);
       thread.Start();
    }
    public void finishedProcessingPath(PathResult result){   
        lock(results){
            results.Enqueue(result);
        }
    }

  


   
}
 public struct PathRequest{
      
        public Vector3 start;
        public Vector3 end;
        public Action<List<Cell>,bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<List<Cell>, bool> _callback){
            this.start = _start;
            this.end = _end;
            this.callback = _callback;
        }

    }

    public struct PathResult{
        public List<Cell> path;
        public bool success;
        public Action<List<Cell>,bool> callback;
        public PathResult(List<Cell> path, bool success,Action<List<Cell>,bool> callback){
            this.path = path;
            this.success = success;
            this.callback = callback;
        }
    }