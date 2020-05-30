using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testWater : MonoBehaviour
{
    public Vector2 pos;
    gridScript gs;
    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
        pos = new Vector2(Random.Range(0,gs.grid.GetLength(1)),Random.Range(0,gs.grid.GetLength(0)));
       // gs.occupySpace(pos);
        this.transform.position = gs.getGridPosition(pos);

    }
}
