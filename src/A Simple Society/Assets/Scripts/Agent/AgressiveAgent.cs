using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveAgent : Agent
{
    AgentController agentController;
    void Awake(){
        action = null;
        this.male = (Random.Range(0.0f,1.0f)>0.5)?true:false;
        moveAgent = Component.FindObjectOfType<MoveAgent>();
        agentReproduce = Component.FindObjectOfType<AgentReproduce>();
        agentController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<AgentController>();
        agentController.aS.numAgents++;
        agentController.agentBorn(this.tag);
        isMoving = false;
        gene = new AggressiveGene(male);
        genes = gene.getGenes();
        this.health = genes[0];
        maxHealth = genes[0];
        this.thirst = genes[1];
        maxThirst = genes[1];
        this.hunger = genes[2];
        maxHunger = genes[2];
        moveAgent.searchRadius = genes[3];
        moveAgent.moveSpeed = genes[4];
        moveAgent.rotateSpeed = genes[5];
        this.cost = genes[6];
        this.timeTillNextMove = genes[7];
        this.gestation = genes[8];
        this.urge = genes[9];
        this.desireable = genes[10];
        this.numChildren = (int)genes[11];
        agentReproduce.canReproduce = canMate;
       
        timeBornAt = Time.realtimeSinceStartup;
        moveAgent.getStartPos();
    }
    void Start(){
        //StartCoroutine("TimeToLive");
        StartCoroutine("timeToReproduce");
        StartCoroutine("makeDecision");
        StartCoroutine("wipeRejectedList");
    }

    public override IEnumerator makeDecision(){
        while(true){
            if(action == null){
                if(!male && agentReproduce.matingPartner != null){
                    action = giveBirth();
                    StartCoroutine(action);
                }
                if(eatFood > drinkWaterUrge && eatFood > mateUrge){
                    if(moveAgent.checkArea("Sheep",8) && moveAgent.searchTarget == null){
                        if(moveAgent.searchTarget && moveAgent.searchTarget.tag != "Sheep")moveAgent.searchTarget = null;
                        moveAgent.searchForTarget();
                        action = Move();
                        StartCoroutine(action);
                    }        
                } else if(drinkWaterUrge>eatFood && drinkWaterUrge>mateUrge){
                    if(moveAgent.checkArea("Water",4)&&!moveAgent.besideWater){
                        moveAgent.searchForTarget();
                        action = Move();
                        StartCoroutine(action);
                    }else if(moveAgent.besideWater){
                        action = drinkWater();
                        StartCoroutine(action);
                    }
                } else if(((mateUrge > eatFood) && (mateUrge > drinkWaterUrge)) ){
                    if(moveAgent.checkArea("Hound",9) && !moveAgent.besideWater && male && canMate){
                        moveAgent.searchForTarget();
                        action = Move();
                        StartCoroutine(action);
                    }else if(!male){
                        action = Move();
                        StartCoroutine(action);
                    }
                }
                if(action==null && moveAgent.searchTarget == null){
                    moveAgent.getNewPos();
                    action = Move();
                    StartCoroutine(action);
                }else if(action == null){
                    moveAgent.moveToTarget(moveAgent.searchTarget);
                    action = Move();
                    StartCoroutine(action);
                }
               
            }
            yield return new WaitForSeconds(timeTillNextMove);
        }
    }

    public override IEnumerator Move(){
        if(isMoving && !isDrinking && !isEating){
            yield break;
        }
        isMoving = true;
        yield return moveAgent.StartCoroutine("moveToNextPos");
        isMoving = false;
        action = null;
        spendEnergy();
    }

    public override void spendEnergy(){
        if(hunger > 0) hunger -= cost;
        if(thirst > 0) thirst -= cost;
        if(hunger  <= 0 || thirst <=0)health -= cost;
        
        eatFood = 1-((hunger)/(maxHunger+thirst));
        drinkWaterUrge = 1-((thirst)/(maxThirst+hunger));
        mateUrge = 1-(urge-diff)/((urge-diff)+hunger+thirst);
        diff = Mathf.Abs(eatFood-drinkWaterUrge);
        if(health <=0)DestroyAgentInstance();
    }

    //destroys agent after the time specified
    public override IEnumerator TimeToLive(){
        yield return new WaitForSeconds(timeToLive*60);
        Component.FindObjectOfType<gridScript>().leaveSpace(moveAgent.pos);
        agentController.aS.diedOfOldAge++;
        DestroyAgentInstance();
    }

    //drink water
    public override IEnumerator drinkWater(){
        while(thirst < maxThirst){
            isDrinking = true;
            yield return new WaitForSeconds(2.5f);
            thirst = maxThirst;
        }
      
        isDrinking = false;
        action = null;
        moveAgent.besideWater = false;
        moveAgent.searchTarget = null;
    }

    //wait for the time specified to set the mating flag for the agent to true
    public override IEnumerator timeToReproduce(){
        yield return new WaitForSeconds(1);
        canMate = true;
    }

   
    //this gets called when an agent collides with another gameObject with a collider set to isTrigger
    //using it to eat food if agent finds it
    void OnTriggerEnter(Collider target){
        if(target.gameObject.tag.Equals("Sheep") == true ){
            if(target.gameObject.tag == "Sheep" && eatFood > drinkWaterUrge && eatFood > mateUrge){
                Destroy(target.gameObject);
                if(hunger < maxHunger)this.hunger = maxHunger;
                if(hunger > maxHunger) hunger -= (hunger-maxHunger);
                if(health > maxHunger) health -= (health-maxHealth);
                moveAgent.searchTarget = null;
            }
        }
    }

    
    //wait until gestation period is over then give birth,
    //perform the crossover event between the two parents genes
    //set the new gene to the new child
    public override IEnumerator giveBirth(){
        int numChildren = GetComponent<Agent>().numChildren;
        float gestation = GetComponent<Agent>().gestation;
        yield return new WaitForSeconds(gestation);
        for(int i = 0; i < numChildren; i++){
            var baby = (GameObject)Resources.Load("Prefabs/Hound");
            Gene partnerGene = agentReproduce.partnersGene;
            Gene babyGene =  GetComponent<Agent>().gene.crossover(partnerGene);
            var child = Instantiate(baby,transform.position, Quaternion.identity);
            child.transform.parent = GameObject.FindGameObjectWithTag("houndContainer").transform;
            child.GetComponent<Agent>().setGene(babyGene);
            child.GetComponent<MoveAgent>().pos = GetComponent<MoveAgent>().pos;
            child.transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }
        agentReproduce.matingPartner = null;
        action = null;
        canMate = false;
        StartCoroutine("timeToReproduce");
    }


    public override void setGene(Gene g)
    {
         this.gene = g;
        genes = gene.getGenes();
        this.health = genes[0];
        maxHealth = genes[0];
        this.thirst = genes[1];
        maxThirst = genes[1];
        this.hunger = genes[2];
        maxHunger = genes[2];
        moveAgent.searchRadius = genes[3];
        moveAgent.moveSpeed = genes[4];
        moveAgent.rotateSpeed = genes[5];
        this.cost = genes[6];
        this.timeTillNextMove = genes[7];
        this.gestation = genes[8];
        this.urge = genes[9];
        this.desireable = genes[10];
        this.numChildren = (int)genes[11];
        agentReproduce.canReproduce = canMate;

        maxThirst = genes[1];
        maxHunger = genes[2];
    }

    public override Gene GetGene()
    {
        return this.gene;
    }

    public override void DestroyAgentInstance()
    {
        agentController.aS.genders.Add((this.male)?1:0);
        agentController.aS.genes.Add(timeBornAt.ToString(),this.genes);
        agentController.agentDied(this.tag);
        Destroy(gameObject);
    }

    public override IEnumerator wipeRejectedList()
    {
         while(true){
            yield return new WaitForSeconds(20);
            moveAgent.rejectedMates = new List<GameObject>();
        }
    }

}
