using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class AgentController : MonoBehaviour
{
    public PassiveStatistics ps;
    public AggressiveStatistics aS;  

    public int currentNumPassiveAgents = 0;
    public int currentNumAggressiveAgents = 0;

    public List<float> deaths = new List<float>();
    void Awake(){
        ps = new PassiveStatistics();
        aS = new AggressiveStatistics();
    }
    public void agentBorn(string tag){
       if(tag =="Sheep"){
            currentNumPassiveAgents++;
            ps.population.Add(currentNumPassiveAgents);
        }else{
            currentNumAggressiveAgents++;
            aS.population.Add(currentNumAggressiveAgents);
        }
    }
    public void agentDied(string tag){
        if(tag == "Sheep"){
            this.currentNumPassiveAgents--;
            ps.population.Add(currentNumPassiveAgents);
            deaths.Add(Time.realtimeSinceStartup);
          }else{
            this.currentNumAggressiveAgents--;
            aS.population.Add(currentNumAggressiveAgents);
            deaths.Add(Time.realtimeSinceStartup);
        } 
         if((this.currentNumPassiveAgents == 0 || this.currentNumPassiveAgents >= 5000) || (this.currentNumAggressiveAgents == 0 || this.currentNumAggressiveAgents >= 5000) ){
                UnityEditor.EditorApplication.isPlaying = false;
        }  
    }
    
    void OnDestroy(){
        ps.generateOutput();
        var passiveJsonString =  JsonConvert.SerializeObject(ps, Formatting.Indented);
        var aggressiveJsonString = JsonConvert.SerializeObject(aS,Formatting.Indented);
        System.IO.File.WriteAllText("C:/Users/Jamie/Desktop/College/Fourth Year Project/Fourth-Year-Project/src/Logs" + "/passiveData.json", passiveJsonString);
        System.IO.File.WriteAllText("C:/Users/Jamie/Desktop/College/Fourth Year Project/Fourth-Year-Project/src/Logs" + "/aggressiveData.json", aggressiveJsonString);
    }
}
