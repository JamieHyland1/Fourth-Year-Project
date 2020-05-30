using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawn : MonoBehaviour
{
    Vector2 pos;
    [SerializeField] private gridScript gs;
    public int secondsTillNextSpawn = 0;
    private float counter;

    void Awake(){
       secondsTillNextSpawn = Random.Range(15,20);
       gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
    }
    void Start()
    {
        pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0))); 
        StartCoroutine("spawnTrees");
        while(!getStartPos());
    }

    // Update is called once per frame
    IEnumerator spawnTrees()
    {
        while(true){
            yield return new WaitForSeconds(secondsTillNextSpawn);
            Vector2 spawnPoint = pos;
            var food = (GameObject)Resources.Load("Prefabs/Food");
            //left
            spawnPoint.x-=1;
            if(!gs.isOccupied(spawnPoint)){
                var left = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                left.transform.parent =  GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //top left
            spawnPoint.y+=1;
            if(!gs.isOccupied(spawnPoint)){
                var left = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                left.transform.parent =  GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //bottom left
            spawnPoint.y -=2;
            if(!gs.isOccupied(spawnPoint)){
                var left = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                left.transform.parent =  GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //right
            spawnPoint = pos;
            spawnPoint.x+=1;
            if(!gs.isOccupied(spawnPoint)){
                var right = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                right.transform.parent = GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //top right
            spawnPoint.y+=1;
            if(!gs.isOccupied(spawnPoint)){
                var right = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                right.transform.parent = GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //bottom right
            spawnPoint.y -=2;
            if(!gs.isOccupied(spawnPoint)){
                var right = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                right.transform.parent = GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //top
            spawnPoint = pos;
            spawnPoint.y-= 1;
            if(!gs.isOccupied(spawnPoint)){
                var up = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                up.transform.parent = GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
            //bottom
            spawnPoint.y+=2;
            if(!gs.isOccupied(spawnPoint)){
                var down = Instantiate(food,gs.getGridPosition(spawnPoint),Quaternion.identity);
                down.transform.parent = GameObject.FindGameObjectWithTag("foodContainer").transform;
            }
        }
    }

    bool getStartPos(){
        pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0)));
        if(!gs.isOccupied(pos)){ 
            transform.position = gs.getGridPosition(pos);
            gs.occupySpace(pos);
            return true;
        }
        return false;
    }
}
