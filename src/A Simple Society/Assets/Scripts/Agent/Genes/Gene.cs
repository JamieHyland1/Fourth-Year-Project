using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will hold each agents attribute or genetics, each agent in the scene will contain this object.
//It will also contain methods for mutation and crossover, the cornerstone of our breeding process and genetic algorithim



public abstract class Gene 
{
    //class methods
    public abstract List<float> getGenes();
    public abstract float mutation(float geneCharacteristic);
    public abstract Gene crossover(Gene one);
}
