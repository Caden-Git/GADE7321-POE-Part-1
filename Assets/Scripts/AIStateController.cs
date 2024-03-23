using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateController : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject flag;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = flag.transform.position;
    }

    // private void MoveTo()
    // {
    //     //Called when movement is involved
    //     
    // }
    //
    // private void Touch()
    // {
    //     //Called when player has the flag
    // }
    //
    // private void Pickup()
    // {
    //     //Called when flag is picked up
    // }
}
