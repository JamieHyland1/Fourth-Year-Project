using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class is a container for various stats related to agents
//The library used for serializing classes to JSON wont serialize 
//a class that inherits from Unity's MonoBehavior class
public class AggressiveStatistics
{
    public int numAgents = 0;
    public int diedOfOldAge = 0;
    public List<int> genders = new List<int>();
    public Dictionary<string,List<float>> genes = new Dictionary<string, List<float>>();
    public List<float> population = new List<float>();
    public List<float> health = new List<float>();
    public List<float> hunger = new List<float>();
    public List<float> thirst = new List<float>();
    public List<float> searchRadius = new List<float>();
    public List<float> turnSpeed = new List<float>();
    public List<float> moveSpeed = new List<float>();
    public List<float> cost = new List<float>();
    public List<float> gestation = new List<float>();
    public List<float> timeTillNextMove = new List<float>();
    public List<float> urge = new List<float>();
    public List<float> desire = new List<float>(); 
    public List<float> numChildren = new List<float>();
    public AggressiveStatistics(){
        //default constructor
    }
    public void generateOutput(){
        List<string> keys = new List<string>(genes.Keys);
        for(int i = 0; i < keys.Count; i++){
            var key = keys[i];
            var gene = genes[key];
            health.Add(gene[0]);
            thirst.Add(gene[1]);
            hunger.Add(gene[2]);
            searchRadius.Add(gene[3]);
            moveSpeed.Add(gene[4]);
            turnSpeed.Add(gene[5]);
            cost.Add(gene[6]);
            timeTillNextMove.Add(gene[7]);
            gestation.Add(gene[8]);
            urge.Add(gene[9]);
            desire.Add(gene[10]);
            numChildren.Add(gene[11]);

        }
    }
}
