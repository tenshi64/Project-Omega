using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ClimbingSystem : MonoBehaviour
{
    [SerializeField]
    Transform helpingPlayer;
    [SerializeField]
    Transform playerClimbing;
    [Header("Configuration")]
    [SerializeField]
    float distanceBetweenPlayers;

    [SerializeField]
    float jumpForce;

    float startPositionY = -1;

    void Update()
    {
        if(helpingPlayer != null)
        {
            helpingPlayer.GetComponent<PlayerMovement>().enabled = false;
            helpingPlayer.GetComponent<Rigidbody>().isKinematic = true;
            helpingPlayer.position = new Vector3(transform.position.x, helpingPlayer.position.y, transform.position.z);
            helpingPlayer.eulerAngles = new Vector3(helpingPlayer.eulerAngles.x, -transform.Find("Text (TMP)").eulerAngles.y, helpingPlayer.eulerAngles.z);

            helpingPlayer.GetComponent<PlayerAnimationManager>().PlayAnimationWithTransition("climb help", 0.25f);

            if(playerClimbing != null)
            {
                if(startPositionY == -1)
                {
                    startPositionY = transform.position.y;
                }
                Vector3 local = transform.TransformPoint(Vector3.left * distanceBetweenPlayers);
                playerClimbing.GetComponent<PlayerMovement>().enabled = false;
                playerClimbing.GetComponent<Rigidbody>().isKinematic = true;
                playerClimbing.position = new Vector3(local.x, playerClimbing.position.y, local.z);
                playerClimbing.eulerAngles = new Vector3(playerClimbing.eulerAngles.x, transform.Find("Text (TMP)").eulerAngles.y, playerClimbing.eulerAngles.z);

                playerClimbing.GetComponent<PlayerAnimationManager>().PlayAnimationWithTransition("jumping", 0.25f);
                if(transform.position.y < startPositionY + 2)
                {
                    playerClimbing.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name.Contains("Playermodel"))
        {
            if(helpingPlayer == null)
            {
                if (!other.GetComponent<PlayerAnimationManager>().IsInState("falling") && !other.GetComponent<PlayerAnimationManager>().IsInState("landing") && !other.GetComponent<PlayerAnimationManager>().IsInState("jumping"))
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        helpingPlayer = other.transform;
                    }
                }
            }
            else
            {
                if (other.transform != helpingPlayer && !other.GetComponent<PlayerAnimationManager>().IsInState("falling") && !other.GetComponent<PlayerAnimationManager>().IsInState("landing") && !other.GetComponent<PlayerAnimationManager>().IsInState("jumping"))
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        playerClimbing = other.transform;
                    }
                }
            }
        }
    }
}
