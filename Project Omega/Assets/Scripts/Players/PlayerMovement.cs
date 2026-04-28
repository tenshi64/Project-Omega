using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerAnimationManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    Transform playerCamera;

    Rigidbody rb;
    Vector3 movementDirection;
    Vector2 input;

    Vector3 camF;
    Vector3 camR;

    [Header("Configuration")]
    [SerializeField]
    [Range(0.01f, 50f)]
    float walkingSpeed; //5 - the best speed

    [SerializeField]
    [Range(0.01f, 50f)]
    float runningSpeed; //10 - the best speed

    [SerializeField]
    float gravityForce;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float groundCheckDistance = 0.1f;

    [SerializeField]
    Vector2 minMaxPositionY;

    float speed;

    public bool isGrounded;
    bool previousFrameGroundCheck;
    bool alreadyJumped;
    bool jumpAnimationFlag;

    PlayerAnimationManager animationManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animationManager = GetComponent<PlayerAnimationManager>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (isGrounded)
        {
            if(!animationManager.IsInState("landing"))
            {
                if (input.x != 0 || input.y != 0)
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //TODO: add controller support
                    {
                        speed = runningSpeed;
                        animationManager.PlayAnimationWithTransition("running", 0.25f);
                    }
                    else
                    {
                        speed = walkingSpeed;
                        animationManager.PlayAnimationWithTransition("walking", 0.25f);
                    }
                }
                else
                {
                    animationManager.PlayAnimationWithTransition("idle", 0.25f);
                }
            }
        }
        else
        {
            if (!animationManager.IsInState("landing") && !animationManager.IsInState("jumping") && !animationManager.IsInState("jumping mirrored"))
            {
                animationManager.PlayAnimationWithTransition("falling", 0.25f);
            }
        }

        //checking if player is touching ground
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if(transform.position.y > minMaxPositionY.y || transform.position.y < minMaxPositionY.x)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0, 5, 0);
        }

        if(isGrounded && !previousFrameGroundCheck) //just landed
        {
            animationManager.PlayAnimationWithTransition("landing", 0.25f);
            alreadyJumped = false;
        }

        previousFrameGroundCheck = isGrounded;
    }

    private void FixedUpdate()
    {
        camF = playerCamera.forward;
        camR = playerCamera.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        movementDirection = new Vector3(input.x, 0, input.y);
        movementDirection.Normalize();

        Vector2 clampedInput = Vector2.ClampMagnitude(input, 1);

        Vector3 rotOffset = playerCamera.TransformDirection(movementDirection);
        rotOffset.y = 0;

        transform.forward = Vector3.RotateTowards(transform.forward, rotOffset, Time.fixedDeltaTime * 5, 0);

        Vector3 position = transform.position;
        position += (camF * clampedInput.y + camR * clampedInput.x) * Time.fixedDeltaTime * speed;

        if (!animationManager.IsInState("landing"))
        {
            rb.MovePosition(position);
        }

        rb.AddForce(0, gravityForce, 0);

        //jumping
        if (Input.GetKey(KeyCode.Space)) //TODO: add controller support
        {
            if(!alreadyJumped && isGrounded && !animationManager.IsInState("jumping") && !animationManager.IsInState("jumping mirrored") && !animationManager.IsInState("landing") && !animationManager.IsInState("falling"))
            {
                alreadyJumped = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                jumpAnimationFlag = !jumpAnimationFlag;
            }
        }

        if(alreadyJumped && !animationManager.IsInState("landing"))
        {
            if(jumpAnimationFlag)
            {
                animationManager.PlayAnimationWithTransition("jumping mirrored", 0.25f);
            }
            else
            {
                animationManager.PlayAnimationWithTransition("jumping", 0.25f);
            }
        }
    }
}
