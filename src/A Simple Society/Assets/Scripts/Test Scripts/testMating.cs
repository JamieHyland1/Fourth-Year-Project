using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMating : MonoBehaviour
{
    gridScript gs;
    MoveAgent ma;
    Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector2(1,1);
        ma =Component.FindObjectOfType<MoveAgent>();
        gs = GameObject.FindGameObjectWithTag("SceneController").gameObject.GetComponent<gridScript>();
    }

    // Update is called once per frame
    void Update()
    {

        gs.leaveSpace(pos);
      
        transform.position = gs.getGridPosition(pos);

        Debug.Log(safeToGiveBirth(pos) + " " + pos);


        if(Input.GetKeyDown(KeyCode.W) && pos.y < gs.maxY) pos.y += 1;
        if(Input.GetKeyDown(KeyCode.S) && pos.y > 0) pos.y -= 1;
        if(Input.GetKeyDown(KeyCode.D) && pos.x < gs.maxX) pos.x += 1;
        if(Input.GetKeyDown(KeyCode.A) && pos.x > 0) pos.x -= 1;

        gs.occupySpace(pos);

    }

    public bool safeToGiveBirth(Vector2 pos){
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
//        Debug.Log(!gs.isOccupied(up) && !gs.isOccupied(down) && !gs.isOccupied(right) && !gs.isOccupied(left));
        return (!gs.isOccupied(up) && !gs.isOccupied(down) && !gs.isOccupied(right) && !gs.isOccupied(left));
    }
}
