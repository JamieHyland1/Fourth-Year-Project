using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField]private Vector3 rotatePoint;
   Vector3 newPos;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 20;
    float zoomDistance;

    Vector2 limit;
    void Start()
    {
        var gs = GameObject.FindGameObjectWithTag("SceneController").GetComponent<gridScript>();
        limit = new Vector2(gs.maxX,gs.maxY);
        zoomDistance = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {        
        Vector3 pos = transform.position;
        if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height-panBorderThickness){
           pos.z += panSpeed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness){
            pos.x -= panSpeed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness){
           pos.z -= panSpeed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width-panBorderThickness){
            pos.x += panSpeed*Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll*scrollSpeed * 100f * Time.deltaTime;
        
        pos.y = Mathf.Clamp(pos.y,6,100);
        pos.x = Mathf.Clamp(pos.x,-limit.x,limit.x);
        pos.z = Mathf.Clamp(pos.z,-limit.y, limit.y);
        transform.position = pos;
    }
}
