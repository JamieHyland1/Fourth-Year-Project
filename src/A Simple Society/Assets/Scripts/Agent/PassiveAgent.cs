using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAgent : Agent
{
    AgentController pc;
    void Awake(){
        action = null;
        this.male = (Random.Range(0.0f,1.0f)>0.5)?true:false;
        moveAgent = Component.FindObjectOfType<MoveAgent>();
        agentReproduce = Component.FindObjectOfType<AgentReproduce>();
        pc = GameObject.FindGameObjectWithTag("SceneController").GetComponent<AgentController>();
        pc.ps.numAgents++;
        pc.agentBorn(this.tag);
        isMoving = false;
        gene = new PassiveGene(true); 
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
                    if(moveAgent.checkArea("Food",9) && moveAgent.searchTarget == null){
                        if(moveAgent.searchTarget && moveAgent.searchTarget.tag != "Food")moveAgent.searchTarget = null;
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
                } else if(((mateUrge > eatFood) && (mateUrge > drinkWaterUrge)) && male && canMate){
                    if(moveAgent.checkArea("Sheep",8) && !moveAgent.besideWater){
                        moveAgent.searchForTarget();
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
                spendEnergy();
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
        pc.ps.diedOfOldAge++;
        DestroyAgentInstance();
    }

    // wait for 5 seconds 
    // drink water then set action to null
    public override IEnumerator drinkWater(){
        yield return new WaitForSeconds(5f);
        thirst = maxThirst;
        isDrinking = false;
        action = null;
        moveAgent.besideWater = false;
        moveAgent.searchTarget = null;
    }

    //wait until gestation period is over then give birth,
    //perform the crossover event between the two parents genes
    //set the new gene to the new child
    public override IEnumerator giveBirth(){
        yield return new WaitForSeconds(gestation);
        for(int i = 0; i < numChildren; i++){
            var baby = (GameObject)Resources.Load("Prefabs/Sheep");
            Gene partnerGene = agentReproduce.partnersGene;
            Gene babyGene =  this.gene.crossover(partnerGene);
            var child = Instantiate(baby,transform.position, Quaternion.identity);
            child.transform.parent = GameObject.FindGameObjectWithTag("sheepContainer").transform;
            child.GetComponent<Agent>().setGene(babyGene);
            child.GetComponent<MoveAgent>().pos = moveAgent.pos;
            child.transform.position = transform.position;
            yield return new WaitForSeconds(this.gestation/numChildren);
        }
        agentReproduce.matingPartner = null;
        action = null;
        canMate = false;
        StartCoroutine("timeToReproduce");
    }

    //wait for the time specified to set the mating flag for the agent to true
    public override IEnumerator timeToReproduce(){
        yield return new WaitForSeconds(10);
        canMate = true;
    }

    //after the time specified wipe the rejected list from the agents memory
    public override IEnumerator wipeRejectedList(){
        while(true){
            yield return new WaitForSeconds(20);
            moveAgent.rejectedMates = new List<GameObject>();
        }
    }

    //destroy the gameobject
    public override void DestroyAgentInstance(){
        Destroy(gameObject);
    }

    //this gets called when an agent collides with another gameObject with a collider set to isTrigger
    //using it to eat food if agent finds it
    void OnTriggerEnter(Collider target){
        if(target.gameObject.tag.Equals("Food") == true ){
            if(target.gameObject.tag == "Food" && eatFood > drinkWaterUrge && eatFood > mateUrge){
                if(hunger < maxHunger)this.hunger = maxHunger;
                if(hunger > maxHunger) hunger -= (hunger-maxHunger);
                if(health > maxHunger) health -= (health-maxHealth);
                moveAgent.searchTarget = null;
            }
        }
    }

    public override Gene GetGene(){
        return this.gene;
    }

    //set the genes for the specific agent
    public override void setGene(Gene g){
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

    void OnDestroy(){
        pc.ps.genders.Add((this.male)?1:0);
        pc.ps.genes.Add(timeBornAt.ToString(),this.genes);
        pc.agentDied(this.tag);
    }
}
