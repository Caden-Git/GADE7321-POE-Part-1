using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static float speed = 3f;
    private Rigidbody rb;
    private float moveHorizontal;
    private float moveVertical;
    public GameObject blueFlag;
    private bool isFlagPickedUp;
    public GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isFlagPickedUp = false;
    }
    
    void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
    
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
    
        rb.velocity = movement * speed;

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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueFinish"))
        {
            //Score++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isFlagPickedUp = false;
    }
}
