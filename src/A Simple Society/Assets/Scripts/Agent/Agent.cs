using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public bool male;
    public float health;
    public float thirst;
    public float hunger;
    public float cost;
    public float timeTillNextMove;
    public bool isMoving;
    public bool isDrinking;
    public bool isEating;
    public int timeToLive = 35; 
    public float gestation;
    public float desireable;
    public float urge;
    public Gene gene;
    public List<float> genes;
    public MoveAgent moveAgent;
    public AgentReproduce agentReproduce; 
    public IEnumerator action;
    public bool canMate = false;
    public float maxHunger;
    public float maxThirst;
    public float maxHealth;
    public int numChildren;
    public float timeBornAt;
    public float eatFood = 0,drinkWaterUrge = 0,mateUrge = 0, diff = 0;
   public abstract void setGene(Gene g);
   public abstract Gene GetGene();
   public abstract void DestroyAgentInstance();
   public abstract IEnumerator wipeRejectedList();
   public  abstract IEnumerator timeToReproduce();
   public abstract IEnumerator giveBirth();
   public abstract IEnumerator drinkWater();
   public abstract IEnumerator TimeToLive();
   public abstract void spendEnergy();
   public abstract IEnumerator Move();
   public abstract IEnumerator makeDecision();
}


    
