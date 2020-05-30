using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveGene : Gene
{

    public bool male=false;
    private float health=0;
    private float thirst=0;
    private float hunger=0;
    private float turnSpeed=0;
    private float moveSpeed=0;
    private float searchRadius=0;
    private float cost=0;
    private float timeTillNextMove=0;
    private float gestation = 0;
    private float urge = 0;
    private float desireable = 0;
    private float numChildren = 0;
    uitl u = GameObject.FindGameObjectWithTag("SceneController").GetComponent<uitl>();
    List<float> genes = new List<float>();

    public PassiveGene(){
        //default constructor
    }

    public PassiveGene(bool male){
        this.male = male;
        this.health = u.map(u.randomGause(),0.0f,1.0f,20.0f,100.0f);
        this.thirst = u.map(u.randomGause(),0.0f,1.0f,80.0f,100.0f);
        this.hunger = u.map(u.randomGause(),0.0f,1.0f,80.0f,100.0f);
        this.searchRadius = u.map(u.randomGause(),0.0f,1.0f,2.5f,20.0f);
        this.moveSpeed = u.map(u.randomGause(),0.0f,1.0f,0.1f,10.0f);
        this.turnSpeed = (float)Random.Range(0.1f,5.5f);
        this.timeTillNextMove =  Random.Range(1.0f,3.0f);
        this.gestation = u.map(u.randomGause(),0.0f,1.0f,5.0f,30.0f);
        this.urge = u.map(u.randomGause(),0.0f,1.0f,0.0f,100.0f);
        this.cost = ((moveSpeed*turnSpeed)+searchRadius)/timeTillNextMove;
        this.desireable = u.map((moveSpeed+searchRadius),2.6f,30.0f,0.0f,100.0f);
        this.gestation = Random.Range(0.0f,10.0f);
        this.numChildren = u.map(gestation,1.0f,gestation,1,4);
        cost = u.map(cost,2,110,0.1f,5.0f);

        

        genes.Add(this.health);
        genes.Add(this.thirst);
        genes.Add(this.hunger);
        genes.Add(this.searchRadius);
        genes.Add(this.moveSpeed);
        genes.Add(this.turnSpeed);
        genes.Add(this.cost);
        genes.Add(this.timeTillNextMove);
        genes.Add(this.gestation);
        genes.Add(this.urge);
        genes.Add(desireable);
        genes.Add(numChildren);


    }

    public PassiveGene(List<float> genes){
        this.male = (Random.Range(0,1)>0.5)? true : false;
        this.genes = genes;
    }

     public override List<float> getGenes(){
        return this.genes;
    }

    public override float mutation(float geneCharacteristic){
        //add some sort of mutation to a gene
        //we have a low population size, so the mutation is high to speed up diversity among the population
        //this basic implementation just increases the specific characteristic by 25% or decreases it by 25%
       if(u.randomGause() > 0.5f){
            geneCharacteristic*=1.25f;
            return  geneCharacteristic;
        }else{
            
            if(geneCharacteristic > 0.25f)geneCharacteristic *=0.75f;
        }
        return geneCharacteristic;
    }

    public override Gene crossover(Gene partner){
        //take half of each gene and mix them into a child gene
        List<float> parent1,child = new List<float>();
        parent1 = partner.getGenes();
        for(int i = 0; i < this.genes.Count;i++){
           float g = (u.randomGause()>0.5f)?parent1[i]:genes[i];
           child.Add((u.randomGause()>0.8f)?mutation(g):g);
        }
        return new PassiveGene(child);
    }

   


}
