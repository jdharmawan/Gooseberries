using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoxCollider2D col;
    private Rigidbody2D rb2d;

    //might need states to keep track of like shooting arrow, deploying shield etc
    private enum playerState {Idle, Walking, Charging};
    private bool isGrounded;

    private playerState pState;
    private Vector2 force;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float bowCharge;//temp, not sure if gonna use in the end


    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        pState = playerState.Idle;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInput();
        MoveCharacter();

        //slowly reset speed to 0
        if(moveSpeed > 0)
        {
            moveSpeed -= 1f;
        }
        else if(moveSpeed < 0)
        {
            moveSpeed += 1f;
        }

        //this one not so simple
        //if want to decelerate, must wait till max jump height
        //if not we just use gravity
        if(jumpForce > 0)
        {
            jumpForce -= 1f;
        }
    }

    private void GetPlayerInput()
    {
        //technically dont need W S unless got ladder
        //if (Input.GetKey(KeyCode.W))
        //{
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //}

        if (Input.GetKey(KeyCode.A))
        {
            moveSpeed = -2f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveSpeed = 2f;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpForce = 2f;
        }
    }

    private void MoveCharacter()
    {
        force = new Vector2(moveSpeed, jumpForce);
        rb2d.AddForce(force);
    }

    //for when anims are done
    private void HandleAnimations()
    {
        if(isGrounded)
        {
            switch (pState)
            {
                case playerState.Idle:
                    Debug.Log("Idle");
                    break;
                case playerState.Walking:
                    Debug.Log("Walking");
                    break;
                case playerState.Charging:
                    Debug.Log("Charging");
                    break;
                default:
                    break;
            }
        }
        else
        {
            //do jump anim
        }
    }
}
