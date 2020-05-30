using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentReproduce : MonoBehaviour
{
    public GameObject matingPartner;
    public Gene partnersGene;
    public bool canReproduce = false;
    public bool requestMate(GameObject partner){
        canReproduce = GetComponent<Agent>().canMate;
        float desire = partner.GetComponent<Agent>().desireable;
        if(Random.Range(0.0f,100.0f) < desire && canReproduce && matingPartner == null){
            this.matingPartner = partner;
            partnersGene = matingPartner.GetComponent<Agent>().GetGene();
            return true;
        }
        return false;
    }
}
