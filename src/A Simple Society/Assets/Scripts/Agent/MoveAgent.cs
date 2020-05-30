using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class contains methods for moving an agent throughout the space, and searching for food when hungry.
//Every method in this class is made public as it will be used in the Agent.cs file.
public class MoveAgent : MonoBehaviour
{
    [SerializeField] public gridScript gs;
    public Vector2 pos;
    public float searchRadius;
    public GameObject matingPartner = null;
    public GameObject searchTarget = null; 
    public bool besideWater = false;
    public float rotateSpeed;
    public float moveSpeed;
    private Quaternion direction;
    public bool isRotating = false; 
    public float minDistance = 0;
    List<Collider> nearbyObjects = new List<Collider>();
    public Vector2 prevPos;
    public List<GameObject> rejectedMates = new List<GameObject>(); 
    uitl u;
    public Vector3 nextPos;

    //get the starting position on the map
    public void getStartPos(){
        gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
        u =  GameObject.FindGameObjectWithTag("SceneController").GetComponent<uitl>();
        while(!getRandomPos());
        prevPos = pos;
    }
   //pick a random x,y point in the search radius of the agent
   //if the x,y goes out of the bounds of the landmass clamp it
    public bool getNewPos(){
        prevPos = pos;
        int x = Mathf.FloorToInt(Random.Range(-1-searchRadius,searchRadius+1));
        int y = Mathf.FloorToInt(Random.Range(-1-searchRadius,searchRadius+1));
        
        pos.x+=x;
        pos.y+=y;
        
        if(pos.x > gs.maxX)pos.x = gs.maxX;
        if(pos.x < 0.0f)pos.x = 0;
        if(pos.y > gs.maxY)pos.y = gs.maxY;
        if(pos.y < 0.0f)pos.y = 0;
       
        return true;
    }
    IEnumerator rotate(){   
        isRotating = true; 
       Vector3 offset  = (transform.position-nextPos);
        direction = Quaternion.Euler(((Vector3.up)*-1) * Mathf.Atan2 (offset.x, offset.y) * Mathf.Rad2Deg);
        float angle = Quaternion.Angle(this.transform.rotation,direction);
        while(angle > 0.2){
            transform.rotation = Quaternion.Slerp(this.transform.rotation,direction,rotateSpeed*Time.deltaTime);
             yield return null;
             angle = Quaternion.Angle(this.transform.rotation,direction);
        } 
        this.transform.rotation = direction;
        isRotating = false;
    }
    //Move to the next position
    IEnumerator moveToNextPos(){
       nextPos = gs.getGridPosition(pos);
       yield return StartCoroutine("rotate");
            while(Vector3.Distance(transform.position,gs.getGridPosition(pos))>0.001){
                this.transform.position = Vector3.MoveTowards(this.transform.position,gs.getGridPosition(pos), moveSpeed*Time.deltaTime);
                yield return null;
            }
      
    }
    //search for the closest object(food, water, agent) in the area
    //if there is no object to search for just move to a random position
    public void searchForTarget(){
        Collider[] potentialTargets = nearbyObjects.ToArray();
        if(potentialTargets.Length > 0){
            Collider targetToFind = potentialTargets[0];
            for(int i = 0; i < potentialTargets.Length;i++){
                var distanceToPotentialTarget = Vector3.Distance(this.transform.position,potentialTargets[i].transform.position);
                var distanceToTempTarget      = Vector3.Distance(this.transform.position,targetToFind.transform.position);
                if(distanceToPotentialTarget < distanceToTempTarget && !GameObject.ReferenceEquals(potentialTargets[i].gameObject,this.gameObject)) targetToFind = potentialTargets[i];
            }
            searchTarget = targetToFind.gameObject;
        }
        if(searchTarget != null)moveToTarget(searchTarget);else getNewPos();
    }     
    //check if the surrounding area is safe to give birth around
    public  bool safeToGiveBirth(){
       float x = pos.x;
        float y = pos.y;
        Vector2 up = new Vector2(x,y+1);
        x = pos.x;
        y = pos.y;
        Vector2 down = new Vector2(x,y-1);
        x = pos.x;
        y = pos.y;
        Vector2 right = new Vector2(x+1,y);
        x = pos.x;
        y = pos.y;
        Vector2 left = new Vector2(x-1,pos.y);
        return (!gs.isOccupied(up) && !gs.isOccupied(down) && !gs.isOccupied(right) && !gs.isOccupied(left));
    }   
   //return true if already been rejected by a female
    bool alreadyRejected(GameObject g){
        if(rejectedMates.Count == 0) return false;
        for(int i = 0; i < rejectedMates.Count; i++){
            if(GameObject.ReferenceEquals(g,rejectedMates[i])){ 
                matingPartner = null;
                return true;
            }
        }
        return false;
    }
    //return a list of objects around the searchRadius of the agent               
    public bool checkArea(string tag, int mask){
        nearbyObjects = new List<Collider>();
        int layerMask = 1 << mask;      
        Collider[]  objs = Physics.OverlapSphere(this.transform.position,searchRadius,layerMask);
        if(objs.Length > 0){
            for(int i = 0; i < objs.Length; i++){
                if(!GameObject.ReferenceEquals(this.gameObject,objs[i].gameObject)) nearbyObjects.Add(objs[i]);
            }
        }
        
        return nearbyObjects.Count != 0;
    }
    bool V3Equal(Vector3 a, Vector3 b){
        return Vector3.SqrMagnitude(a - b) < 9.99999944E-11f;
    }
    bool getRandomPos(){
        if(this.tag == "Sheep"){
        pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0)));
        }else{
            pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0)));
        }
        if(!gs.isOccupied(pos)){ 
            transform.position = gs.getGridPosition(pos);
            return true;
        }
        return false;
    }
    public float moveToTarget(GameObject target){
        if(target.tag == "Player"){
            getNewPos();
            return 0;
        }
        if(target.tag == "Water" && Vector3.Distance(transform.position,target.transform.position)<=1){
            besideWater = true;
            searchTarget = null;
            return 1;
        }
        //request to mate with female agent, if rejected add that female to the list of rejected mates
        if(searchTarget.tag != "Food" && searchTarget.tag != "Water"){
            var thisAgent = this.GetComponent<Agent>();
            if((searchTarget.GetComponent<Agent>().GetType() == thisAgent.GetType() && thisAgent.male && !searchTarget.GetComponent<Agent>().male) && Vector3.Distance(this.transform.position, target.transform.position) <= 1 && !alreadyRejected(target)){
                if(!target.GetComponent<AgentReproduce>().requestMate(this.gameObject)){
                    rejectedMates.Add(target);
                    target = null;
                }
                getNewPos();
                searchTarget = null;
                return 0;
           }
        }
      
        if(target.tag == "Food" || target.tag =="Sheep" || target.tag == "Water"){
            Vector2 targetPos = new Vector2();
            if(target.tag == "Sheep")targetPos = target.GetComponent<MoveAgent>().pos;
            if(target.tag == "Water")targetPos = target.GetComponent<testWater>().pos;
            if(target.tag == "Food")targetPos = target.GetComponent<foodTest>().pos;
          
            pos = targetPos;
            minDistance = (target.tag == "Food" || (target.tag == "Sheep" && this.gameObject.tag == "Hound"))?0.2f:1.0f;}
        return Vector3.Distance(this.transform.position,target.transform.position);
    }

    //get the mid point between the for cardinal directions
    //Quaternions can behave oddly at angles of 180 degress when 
    //using Quaternion.Slerp()
    Quaternion getTempDirection(Quaternion agentRotation, Quaternion direction){
        Quaternion[] potentialDirections = {Quaternion.AngleAxis(0,Vector3.up),
                                            Quaternion.AngleAxis(90,Vector3.up),
                                            Quaternion.AngleAxis(180,Vector3.up),
                                            Quaternion.AngleAxis(-90,Vector3.up),
                                            Quaternion.AngleAxis(-180,Vector3.up)};
        for(int i = 0; i < potentialDirections.Length; i ++){
            float angleBetweenAgent = Mathf.Floor(Quaternion.Angle(agentRotation,potentialDirections[i]));
            float angleBetweenDirection = Mathf.Floor(Quaternion.Angle(direction,potentialDirections[i]));
            if(angleBetweenDirection == 90.00001) angleBetweenDirection = 90;
            if((angleBetweenDirection) == 90){ 
                return potentialDirections[i];
            }
        } 
        return direction;                                   
    }
}
