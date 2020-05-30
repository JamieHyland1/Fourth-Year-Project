using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a general class that will spawn x amount of a certain gameObject you apply to it
public class Spawner : MonoBehaviour
{
    public GameObject Object_to_Spawn;
    public int num_instances;

    
    // Start is called before the first frame update
    void Start()
    {
      for(int i = 0; i <= num_instances-1; i ++){
        var obj = Instantiate(Object_to_Spawn,new Vector3(0,0,0),Quaternion.identity);
        if(string.Equals(obj.gameObject.name,"Sheep(Clone)")){
          obj.transform.parent = GameObject.FindGameObjectWithTag("sheepContainer").transform;
        }
        if(string.Equals(obj.gameObject.name,"Tree(Clone)")){
          obj.transform.parent = GameObject.FindGameObjectWithTag("treeContainer").transform;
        }
        if(string.Equals(obj.gameObject.name,"Hound(Clone)")){
          obj.transform.parent = GameObject.FindGameObjectWithTag("houndContainer").transform;
        }
        if(string.Equals(obj.gameObject.name,"Food(Clone)")){
          obj.transform.parent = GameObject.FindGameObjectWithTag("foodContainer").transform;
        }
         if(string.Equals(obj.gameObject.name,"Water(Clone)")){
          obj.transform.parent = GameObject.FindGameObjectWithTag("Tiles").transform;
        }
      }   
    }
}
