using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static float speed = 3f;
    private Rigidbody rb;
    private float moveHorizontal;
    private float moveVertical;
    public GameObject blueFlag;
    public static bool isFlagPickedUp;
    public GameObject player;
    public GameObject blueBase;
    public GameObject redBase;
    
    public static int playerScore;
    public static int aiScore;
    public static string winner;
    public TMP_Text playerScoreText;
    public TMP_Text aiScoreText;
    
    public TMP_Text winnerText;
    public Button restartButton;
    public GameObject restartUI;

    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        isFlagPickedUp = false;
        restartUI.SetActive(false);
    }
    
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
    
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
    
        rb.velocity = movement * speed;

        if (playerScore == 5)
        {
            winner = "player";
            
            if (AIStateController.winner == "ai")
            {
                ShowRestart();
                winnerText.text = "WINNER: AI";
            }
            
            if (winner == "player")
            {
                ShowRestart();
                winnerText.text = "WINNER: PLAYER";
            }
        }
        
        if (isFlagPickedUp)
        {
            blueFlag.transform.position = player.transform.position;
        }

        if (!isFlagPickedUp)
        {
            blueFlag.transform.position = new Vector3(5.69f, 2f, 0f);
        }
        
        if ((Vector3.Distance(player.transform.position, AIStateController.agent.transform.position) < 0.7f))
        {
            isFlagPickedUp = false;
        }

        if (Vector3.Distance(player.transform.position, blueFlag.transform.position) <= 1f)
        {
            isFlagPickedUp = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueFinish") && isFlagPickedUp)
        {
            playerScore++;

            playerScoreText.text = playerScore.ToString();
            
            if (playerScore == 5)
            {
                winner = "player";
                ShowRestart();
            }

            if (playerScore > 5)
            {
                playerScore = 5;
            }
            Restart();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isFlagPickedUp = false;
    }

    void Restart()
    {
        Debug.Log("player ai restart restart");
        AIStateController.currentState = AIStateController.States.moveToFlag;
        AIStateController.inPlayerRange = false;
        AIStateController.isFlagPickedUp = false;
        AIStateController.agent.transform.position = redBase.transform.position;

        Debug.Log("player restart");
        isFlagPickedUp = false;
        player.transform.position = blueBase.transform.position;
    }

    void ShowRestart()
    {
        restartUI.SetActive(true);
    }
}
