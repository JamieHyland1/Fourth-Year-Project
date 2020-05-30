using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAgent : MonoBehaviour
{
    gridScript gs;

    [SerializeField, Range(5,5)] float searchRadius = 5;
     float moveSpeed = 0.5f;

    [SerializeField,Range(2,5)] float timeTillNextMove;

   public Vector3 nextPos;

    public IEnumerator action;

    public bool isMoving = false;

    Vector2 pos = new Vector2();

    Quaternion direction;

    float health = 100;
    float thirst = 100;

    void Awake(){
        gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
        pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0)));
        transform.position = gs.getGridPosition(pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("makeDecision");
    }

    IEnumerator makeDecision(){
        while(true){
            if(action == null){
                nextPos = checkForItem("Water",5);
                action = move();
                StartCoroutine(action);
            }
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator rotate(){
        // Vector3 offset = nextPos - transform.position;
        // Quaternion rotation = Quaternion.Euler(Vector3.up * (Mathf.Atan2 (offset.x, offset.y) * Mathf.Rad2Deg)*-1);
        // while(Quaternion.Angle(transform.rotation, direction) > 0.3){
        //     transform.rotation = Quaternion.Slerp(transform.rotation,direction,moveSpeed*Time.deltaTime);
            yield return null;
        // }
        transform.LookAt(transform.forward+nextPos);
        //transform.rotation = direction;
    }

    IEnumerator move(){
        if(isMoving)yield break;
        yield return StartCoroutine("rotate");
        Debug.Log(transform.forward);
        isMoving = true;
        while(Vector3.Distance(this.transform.position,nextPos) > 0.5){
            transform.position = Vector3.Lerp(transform.position,nextPos,moveSpeed*Time.deltaTime);
            yield return null;
        }
        isMoving = false;
        action = null;
    }

   Vector3 getNextPos(){
        float leftBoundary   = gs.getGridPosition(new Vector2(0,0)).x;
        float rightBoundary  = gs.getGridPosition(new Vector2(gs.maxX,0)).x;
        float topBoundary    = gs.getGridPosition(new Vector2(0,gs.maxY)).z;
        float bottomBoundary = gs.getGridPosition(new Vector2(0,0)).z;
    
        direction = Quaternion.AngleAxis(Random.Range(0,360), Vector3.up);
        Vector3 x = transform.forward*searchRadius;
        x.y = transform.position.y;
        

       //prevent agent from walking outside the landmass
      if(x.x < leftBoundary) x.x = leftBoundary;
      if(x.x > rightBoundary)x.x = rightBoundary;
      if(x.z < bottomBoundary)x.z = bottomBoundary;
      if(x.z > topBoundary)x.z = topBoundary;

       return x;
    }

    Vector3 checkForItem(string tag, int mask){
        int layer = 1<<mask;
        Collider[] objects = Physics.OverlapSphere(this.transform.position,searchRadius,layer);
        if(objects.Length > 0){
            Collider tempObject = objects[0];
            for(int i = 0; i < objects.Length; i++){
                var distToTemp = Vector3.Distance(tempObject.gameObject.transform.position,transform.position);
                var distToNextObject = Vector3.Distance(objects[i].gameObject.transform.position,transform.position);
                if(distToNextObject < distToTemp)tempObject = objects[i];
            }
            Debug.Log("Found " + tag);
            direction = Quaternion.AngleAxis(Quaternion.Angle(transform.rotation,tempObject.transform.rotation),Vector3.up);
            
            return tempObject.gameObject.transform.position;
        }
        return getNextPos();
    }


    void OnTriggerEnter(Collider target){
        Debug.Log(target.gameObject.name);
     
    }

    void onDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,searchRadius);
    }
}
