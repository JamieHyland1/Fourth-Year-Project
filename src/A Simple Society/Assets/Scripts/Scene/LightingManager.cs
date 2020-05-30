using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
   [SerializeField] private Light DirectionalLight;
   [SerializeField] private Light Moon;
   [SerializeField] private LightingPreset LightingPreset;


   [SerializeField, Range(0,24)] private float TimeOfDay;

    void Update(){
        if(LightingPreset == null) return;
        if(Application.isPlaying){
            TimeOfDay += Time.deltaTime;
            TimeOfDay%=24;
            updateLighting(TimeOfDay/24f);

        }else{
            updateLighting(TimeOfDay/24f);

        }
    }
    private void updateLighting(float timePercentage){
        RenderSettings.ambientLight = LightingPreset.ambientColor.Evaluate(timePercentage);
        RenderSettings.fogColor = LightingPreset.fogColor.Evaluate(timePercentage);
        if(DirectionalLight != null){
            DirectionalLight.color = LightingPreset.directionalColor.Evaluate(timePercentage);
            DirectionalLight.transform.rotation = Quaternion.Euler(new Vector3((timePercentage*360f)-90f,170f,0));
        }

    }
   private void onValidate(){
        if(DirectionalLight != null)
           return;
        if(RenderSettings.sun != null){
            DirectionalLight = RenderSettings.sun;
        }else{
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights){
                if(light.type == LightType.Directional) DirectionalLight = light;
            }
        }
       
   }
}
