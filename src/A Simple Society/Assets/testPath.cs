using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPath : MonoBehaviour
{
   [SerializeField]
   float speed = 15;
   public Transform target;
   List<Cell> path;
   int targetIndex;
   public MapTerrain map;
   void Start(){
      // transform.position = map.GetRandomPosition();
       PathManager.RequestPath(new PathRequest(transform.position,target.position, onPathFound));
      StartCoroutine("wait");
   }
    public void onPathFound(List<Cell> path, bool success){
        if(success){
            this.path = path;
            StopCoroutine("followPath");
            StartCoroutine("followPath");
        }else{
            print("no path found");
        }
    }

    IEnumerator followPath(){
        while(path.Count > 0){
            Vector3 localTarget = path[0].position;
            while(Vector3.Distance(transform.position,localTarget) > 0.1f){
                Vector3 newPos = Vector3.MoveTowards(transform.position,localTarget,speed*Time.deltaTime);
                transform.position = newPos;
                yield return null;
            }
            path.Remove(path[0]);
            yield return new WaitForSeconds(0.2f);
        }
    }

  public IEnumerator wait(){
      yield return new WaitForSeconds(Random.RandomRange(1,5));
      

  }
}
