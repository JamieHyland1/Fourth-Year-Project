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
       PathManager.RequestPath(transform.position,target.position, onPathFound);
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

    void OnDrawGizmos() {
        if(path != null){
            Gizmos.color = Color.black;
            for(int i = 0; i < path.Count;i++){
                Gizmos.DrawWireSphere(path[i].position,0.2f);
            }
              for(int i = 1; i < path.Count;i++){
                Gizmos.DrawLine(path[i-1].position,path[i].position);
            }

        }
    }

}
