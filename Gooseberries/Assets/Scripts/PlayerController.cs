using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//think about how weighty we want the game to feel
//fast paced or slower
//eg if we want to have a anim when recovering from fall, might need to have another state
//wall jump? i just thought of the code, can just set isgrounded to true when touching walls or smth, of course the physics is more complex

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D groundedCol;

    private BoxCollider2D col;
    private Rigidbody2D rb2d;

    //might need states to keep track of like shooting arrow, deploying shield etc
    private enum playerState {Idle, Walking, Aiming, Shooting, Reloading};
    //gonna need to set up a seperate smaller collider below the player collider to keep track of grounded
    [SerializeField] private bool isGrounded;

    private playerState pState;
    private Vector2 force;

    private int hp = 3;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 2f;
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

        //test quick deceleration
        if (moveSpeed != 0)
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 1f);

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

        if(Input.GetMouseButtonDown(0))
        {
            //if left click
            pState = playerState.Aiming;
        }

        if(Input.GetMouseButtonUp(0))
        {
            //when release mouse, shoot arrow and maybe idle, or reload
            pState = playerState.Shooting;
            Shoot();
        }

        if(Input.GetMouseButtonDown(1))
        {
            //if right click do shield

        }

        if(Input.GetMouseButtonUp(1))
        {
            //release shield
        }

        if (pState != playerState.Aiming)
        {
            //need to consider if we want like jump A D movement
            if (Input.GetKey(KeyCode.A))
            {
                if (isGrounded)
                    pState = playerState.Walking;

                //move left
                moveSpeed = -2f;
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (isGrounded)
                    pState = playerState.Walking;

                //move right
                moveSpeed = 2f;
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    jumpForce = 5f;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                    isGrounded = false;
                    //pState = playerState.Jumping;
                }
            }
        }

        //Debug.Log("player state: " + pState.ToString());
    }

    private void MoveCharacter()
    {
        force = new Vector2(moveSpeed, 0);
        rb2d.AddForce(force);
    }

    //below 2 functions might be enumerator, coz need to deal with eventual animations
    void Shoot()
    {
        //do shooting, handle anims then set to reloading
        pState = playerState.Reloading;
        Reload();
    }

    void Reload()
    {
        //reload then go back to idle
        pState = playerState.Idle;
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
                case playerState.Aiming:
                    Debug.Log("Aiming");
                    break;
                case playerState.Shooting:
                    Debug.Log("Shooting");
                    break;
                case playerState.Reloading:
                    Debug.Log("Reloading");
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

    public void SetIsGrounded(bool b)
    {
        isGrounded = b;
    }
}
