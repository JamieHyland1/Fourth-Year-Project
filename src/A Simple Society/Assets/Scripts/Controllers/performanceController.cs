using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class performanceController : MonoBehaviour
{
    string json;
    public int frameCount = 0;
    public float dt = 0.0F;
    public float fps = 0.0F;
    public List<float> framesOverTime;    public float gameTime;
    public float timeSinceLastFrame;
    void Start(){
        framesOverTime = new List<float>();
        timeSinceLastFrame = Time.realtimeSinceStartup;
    }

     void Update(){
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 ){
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0F;
        }
        framesOverTime.Add(fps);
        gameTime = Time.realtimeSinceStartup - timeSinceLastFrame;
    }

    void OnDestroy(){
        json = JsonUtility.ToJson(this,true);
        System.IO.File.WriteAllText("C:/Users/Jamie/Desktop/College/Fourth Year Project/Fourth-Year-Project/src/Logs" + "/data.json", json);

    }
}
