using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomWalk : MonoBehaviour
{
    gridScript gs;
    public Vector2 pos;
    public Vector2 prevPos;
    public Vector3 lastPos;
    public Vector3 nextPos;

   public List<Vector3> path = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
        gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
        pos = new Vector2(0,0);
        prevPos = pos;
        transform.position = gs.getGridPosition(pos);
        StartCoroutine("walk");
    }

    // Update is called once per frame
    IEnumerator walk(){
        while(true){
            while(!getNextPos());
            transform.position = gs.getGridPosition(pos);
            yield return new WaitForSeconds(1);
            if(path.Count > 15)path = new List<Vector3>();
        }
    }

    bool getNextPos(){
        int choice = Random.Range(0,4);
        lastPos = gs.getGridPosition(prevPos);
        prevPos = pos;
        if(choice == 0 && pos.x < gs.maxX){
            pos.x +=1;
        }else if(choice == 1 && pos.x > 0){
            pos.x -=1;
        }else if(choice == 2 && pos.y < gs.maxX){
            pos.y+=1;
        }else if(choice == 3 && pos.y > 0){
            pos.y-=1;
        }
        return true;
    }

    bool notInPath(Vector3 pos){
        for(var i = 0; i < path.Count;i++){
            if(V3Equal(pos,path[i]))return false;
        }
        return true;
    }
    
    //check if two
    bool V3Equal(Vector3 a, Vector3 b){
        return Vector3.SqrMagnitude(a - b) < 9.99999944E-11f;
    }


}
