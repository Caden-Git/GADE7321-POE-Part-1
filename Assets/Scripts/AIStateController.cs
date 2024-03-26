using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AIStateController : MonoBehaviour
{
    public static NavMeshAgent agent;
    public GameObject redFlag;
    public GameObject redBase;
    public GameObject blueBase;
    public Transform player;
    public static States currentState;
    public static bool inPlayerRange;
    public static bool isFlagPickedUp;

    public static int playerScore;
    public static int aiScore;
    public static string winner;
    public static TMP_Text playerScoreText;
    public TMP_Text aiScoreText;
    
    public GameObject restartUI;
    
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
        Time.timeScale = 1f;
        aiScore = 0;
        agent = GetComponent<NavMeshAgent>();
        currentState = States.moveToFlag;
        inPlayerRange = false;
        isFlagPickedUp = false;
        
        restartUI.SetActive(false);
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
            
            StartCoroutine(Accelerate());
        }
        else if (currentState == States.touch && isFlagPickedUp)
        {
            redFlag.transform.position = agent.transform.position;
            agent.speed = 3f;
            agent.destination = redBase.transform.position;
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

        if (other.CompareTag("RedFinish") && isFlagPickedUp && (Vector3.Distance(agent.transform.position, redBase.transform.position) <= 0.5f))
        {
            aiScore++;

            aiScoreText.text = aiScore.ToString();
            
            if (aiScore == 5)
            {
                Debug.Log("ai");
                winner = "ai";
            }

            if (aiScore > 5)
            {
                aiScore = 5;
            }
            Restart();
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

    void Restart()
    {
        Debug.Log("ai restart");
        currentState = States.moveToFlag;
        inPlayerRange = false;
        isFlagPickedUp = false;
        agent.transform.position = redBase.transform.position;

        Debug.Log("ai player restart");
        PlayerController.isFlagPickedUp = false;
        player.transform.position = blueBase.transform.position;
    }
}
