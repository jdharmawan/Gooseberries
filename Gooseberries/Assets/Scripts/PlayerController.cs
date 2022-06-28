using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//think about how weighty we want the game to feel
//fast paced or slower
//eg if we want to have a anim when recovering from fall, might need to have another state
//wall jump? i just thought of the code, can just set isgrounded to true when touching walls or smth, of course the physics is more complex

//TODO: Convert code from velocity to addforce, but need to limit max speed if not will keep accelerating
//keep track of last input so that the latest one will override the previous one, can A D then move to D etc
//figure out knight collider and shield rotation

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D groundedCol;
    public GameObject bowPivot;

    private BoxCollider2D col;
    private Rigidbody2D rb2d;

    //might need states to keep track of like shooting arrow, deploying shield etc
    private enum playerState {Idle, Walking, Aiming, Shooting, Reloading};
    //gonna need to set up a seperate smaller collider below the player collider to keep track of grounded
    [SerializeField] private bool isGrounded;

    private playerState pState;
    private Vector2 force = new Vector2();
    private Vector3 mousePos = new Vector3();

    private int hp = 3;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float bowCharge;//temp, not sure if gonna use in the end

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    private bool facingRight = true;
    private bool shield = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        pState = playerState.Idle;
        isGrounded = true;

        Debug.Log("bow pivot pos: " + bowPivot.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInput();

        ////test quick deceleration
        //if (moveSpeed != 0)
        //    moveSpeed = Mathf.Lerp(moveSpeed, 0, 1f);

        if (pState == playerState.Aiming)
            FlipWithMouseAim();
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
            //FlipWithMouseAim();
            //if left click
            if (isGrounded)
            {
                pState = playerState.Aiming;
                animator.SetTrigger("aim");
                Debug.Log("aiming");
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            //when release mouse, shoot arrow and maybe idle, or reload
            //check just in case i guess
            if (pState == playerState.Aiming)
            {
                pState = playerState.Shooting;
                Shoot();
            }
        }

        //actually maybe shield can be toggle
        if(Input.GetMouseButtonDown(1))
        {
            //if right click do shield
            shield = true;
        }

        if(Input.GetMouseButtonUp(1))
        {
            //release shield
            shield = false;
        }

        //consider saving last input so that can press A, then D, then move right
        if (pState != playerState.Aiming)//temp just put aiming, consider putting shooting and reloading here also
        {
            //need to consider if we want like jump A D movement
            if (Input.GetKey(KeyCode.A))
            {
                if (isGrounded)
                    pState = playerState.Walking;

                //move left
                moveSpeed = -2f;
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);

                //temp anim test
                if (facingRight)
                {
                    sprite.flipX = true;
                    facingRight = false;
                }
            }
            else if(Input.GetKeyUp(KeyCode.A))
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (isGrounded)
                    pState = playerState.Walking;

                //move right
                moveSpeed = 2f;
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);

                //temp anim test
                if (!facingRight)
                {
                    sprite.flipX = false;
                    facingRight = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    //jumpForce = 5f;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                    isGrounded = false;
                    //pState = playerState.Jumping;
                }
            }
        }

        //Debug.Log("player state: " + pState.ToString());
    }

    //below 2 functions might be enumerator, coz need to deal with eventual animations
    void Shoot()
    {
        //do shooting, handle anims then set to reloading
        Debug.Log("shooting");
        pState = playerState.Reloading;
        Reload();
    }

    void Reload()
    {
        //reload then go back to idle
        Debug.Log("reloading");
        animator.SetTrigger("idle");
        pState = playerState.Idle;
    }

    //check if mouse if left or right of sprite and flip accordingly
    //try local position
    void FlipWithMouseAim()
    {
        Debug.Log("flip here");
        mousePos = Input.mousePosition;
        //need to set z to 10f because https://answers.unity.com/questions/331558/screentoworldpoint-not-working.html
        mousePos.z = 10f;
        //Debug.Log("mouse pos: " + Camera.main.ScreenToWorldPoint(mousePos));
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //first check the mouse x pos relative to sprite center
        if (mousePos.x > transform.position.x)
        {
            facingRight = true;

            if (sprite.flipX)
                sprite.flipX = false;

            Debug.Log("mouse pos: " + mousePos);
            Debug.Log("bowpivot transform 1");
            Debug.Log("bow pivot pos1 : " + bowPivot.transform.position.x);
            //then do bowpivot transforms
            if (bowPivot.transform.position.x < 0)
            {
                bowPivot.transform.position = new Vector3(0 - bowPivot.transform.position.x, bowPivot.transform.position.y);
                Debug.Log("bow pivot pos2 : " + bowPivot.transform.position.x);
            }

            Debug.Log("bow pivot y1 : " + bowPivot.transform.localScale.y);
            if (bowPivot.transform.localScale.y < 0)
            {
                bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, bowPivot.transform.localScale.y * -1, bowPivot.transform.localScale.z);
                Debug.Log("bow pivot y2 : " + bowPivot.transform.localScale.y);
            }
        }
        else
        {
            facingRight = false;

            if (!sprite.flipX)
                sprite.flipX = true;

            Debug.Log("mouse pos: " + Input.mousePosition);
            Debug.Log("bowpivot transform 2");
            Debug.Log("bow pivot pos1 : " + bowPivot.transform.position.x);
            //then do bowpivot transforms
            if (bowPivot.transform.position.x > 0)
            {
                bowPivot.transform.position = new Vector3(0 - bowPivot.transform.position.x, bowPivot.transform.position.y);
                Debug.Log("bow pivot pos2 : " + bowPivot.transform.position.x);
            }

            Debug.Log("bow pivot y1 : " + bowPivot.transform.localScale.y);
            if (bowPivot.transform.localScale.y > 0)
            {
                bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, bowPivot.transform.localScale.y * -1, bowPivot.transform.localScale.z);
                Debug.Log("bow pivot y2 : " + bowPivot.transform.localScale.y);
            }
        }

        ////in case here is a check based on the bowpivot position
        //if(mousePos.x > bowPivot.transform.position.x)
        //{
        //    facingRight = true;
        //}
        //else
        //{
        //    facingRight = false;
        //}
    }

    //for when anims are done
    //maybe dont need, maybe can do in checks above, see how
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
