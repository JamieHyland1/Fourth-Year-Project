using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodTest : MonoBehaviour
{

    public gridScript gs;
    public AnimationCurve animation;
    public Vector2 pos;
    void Awake(){
        gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
        pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,(gs.grid.GetLength(0))));
        this.transform.position = gs.getGridPosition(pos);
    }

    IEnumerator deactivateForTime(){
       pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0)));
       this.transform.position = gs.getGridPosition(pos);
        gameObject.tag ="Player";
        yield return new WaitForSeconds(35);
        gameObject.tag = "Food";
    }
    void OnTriggerEnter(Collider target){
        StartCoroutine(deactivateForTime());
    }

    float getRandom(){
        return animation.Evaluate(Random.value);
    }
}
