using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System;
public class PathManager : MonoBehaviour
{
      Queue<PathRequest> requests = new Queue<PathRequest>();
      PathRequest currentRequests;
      static PathManager instance;

      public Pathfinding pathfinder;
      bool isProcessingPath;

      void Awake() {
          pathfinder = GetComponent<Pathfinding>();
          instance = this;
      }
    public static void RequestPath(Vector3 start, Vector3 end, Action<List<Cell>, bool> callback){
        PathRequest request = new PathRequest(start,end,callback);
        instance.requests.Enqueue(request);
        instance.tryProcessNext();

    }
    public void finishedProcessingPath(List<Cell>path, bool success){
        currentRequests.callback(path,success); 
        isProcessingPath = false;
        tryProcessNext();
    }

    public void tryProcessNext(){
        if(!isProcessingPath && requests.Count > 0){
            currentRequests = requests.Dequeue();
            isProcessingPath = true;
            pathfinder.StartFindPath(currentRequests.start,currentRequests.end);
        }
    }

    struct PathRequest{
      
        public Vector3 start;
        public Vector3 end;
        public Action<List<Cell>,bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<List<Cell>, bool> _callback){
            this.start = _start;
            this.end = _end;
            this.callback = _callback;
        }

    }
}
