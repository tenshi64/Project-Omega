using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LadderSystem : MonoBehaviour
{
    [SerializeField]
    Transform bottom;
    [SerializeField]
    Transform upper;
    [SerializeField]
    float climbSpeed = 1f;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject player2;
    private bool player1isClimbing = false;
    private bool player1isInRange = false;
    private bool player2isInRange = false;
    private bool player2isClimbing = false;
    //private bool someoneIsClimbing = false;
    void Update()
    {
        if(player1isInRange && !player1isClimbing && !player2isClimbing && Input.GetKeyUp(KeyCode.E))
        {
            player1isClimbing = true;
            Climb();
        }
        if(player2isInRange && !player2isClimbing && !player1isClimbing && Input.GetKeyUp(KeyCode.E))
        {
            player2isClimbing = true;
            Climb();
        }
        if(player1isClimbing)
        {
            float vertical = Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(0, vertical * climbSpeed * Time.deltaTime, 0);
            player.transform.position += moveDirection;

            CheckExit();
        }
        if(player2isClimbing)
        {
            float vertical = Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(0, vertical * climbSpeed * Time.deltaTime, 0);
            player2.transform.position += moveDirection;

            CheckExit();
        }
    }
    void Climb()
    {
        if(player1isClimbing)
        {
            player.transform.position = new Vector3(bottom.position.x, bottom.position.y, bottom.position.z/*player.transform.position.z*/);
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if(player2isClimbing)
        {
            player2.transform.position = new Vector3(bottom.position.x, bottom.position.y, bottom.position.z/*player.transform.position.z*/);
            player2.GetComponent<PlayerMovement>().enabled = false;
            player2.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    void StopClimb()
    {
        if(player1isClimbing)
        {
            player1isClimbing = false;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
        else if(player2isClimbing)
        {
            player2isClimbing = false;
            player2.GetComponent<PlayerMovement>().enabled = true;
            player2.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    void CheckExit()
    {
        if(player1isClimbing)
        {
            if (player.transform.position.y >= upper.position.y)
            {
                StopClimb();
            }
            else if (player.transform.position.y <= bottom.position.y - 0.1f)
            {
                StopClimb();
            }
        }
        if(player2isClimbing)
        {
            if (player2.transform.position.y >= upper.position.y)
            {
                StopClimb();
            }
            else if (player2.transform.position.y <= bottom.position.y - 0.1f)
            {
                StopClimb();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.gameObject;
            player1isInRange = true;     
        }
        if(other.CompareTag("Player2"))
        {
            player2 = other.gameObject;
            player2isInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player1isInRange = false;
        }
        if(other.CompareTag("Player2"))
        {
            player2isInRange = false;
        }
    }
}
