using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIStateController : MonoBehaviour
{
    public static NavMeshAgent agent;
    public GameObject redFlag;
    public GameObject redFinish;
    public Transform player;
    public States currentState;
    private bool inPlayerRange;
    public bool isFlagPickedUp;

    public enum States
    {
        moveToFlag,
        follow,
        touch
    };
    
    public AIStateController(NavMeshAgent _agent, GameObject _flag, Transform _player)
    {
        agent = _agent;
        redFlag = _flag;
        player = _player;
    }
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = States.moveToFlag;
        inPlayerRange = false;
        isFlagPickedUp = false;
    }

    private void Update()
    {
        if (currentState == States.moveToFlag)
        {
            agent.speed = 3f;
            agent.destination = redFlag.transform.position;
        }
        else if (currentState == States.follow && inPlayerRange)
        {
            agent.destination = player.transform.position;

            //agent.speed = 3f;
            StartCoroutine(Accelerate());
        }
        else if (currentState == States.touch && isFlagPickedUp)
        {
            redFlag.transform.position = agent.transform.position;
            agent.speed = 3f;
            agent.destination = redFinish.transform.position;
        }

        if (isFlagPickedUp == false)
        {
            redFlag.transform.position = new Vector3(-5.91f, 2, 0);
        }

        if ((Vector3.Distance(agent.transform.position, player.transform.position) < 0.7f))
        {
            isFlagPickedUp = false;
            currentState = States.moveToFlag;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isFlagPickedUp == false)
        {
            inPlayerRange = true;
            currentState = States.follow;
        }

        if (other.CompareTag("RedFlag") && (Vector3.Distance(agent.transform.position, redFlag.transform.position) <= 1f))
        {
            Debug.Log("picked up red");
            isFlagPickedUp = true;
            currentState = States.touch;
        }

        if (other.CompareTag("RedFinish"))
        {
            //AIScore++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPlayerRange = false;
            currentState = States.moveToFlag;
        }
    }

    IEnumerator Accelerate()
    {
        agent.speed += 0.2f * Time.deltaTime;
        if (agent.speed > 5)
        {
            agent.speed = 5;
        }

        yield return new WaitForSeconds(2);
    }
}
