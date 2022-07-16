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

    private CapsuleCollider2D col;
    private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    //might need states to keep track of like shooting arrow, deploying shield etc
    private enum playerState {Idle, Walking, Aiming, Shooting, Reloading, Jumping};

    private playerState pState;
    private Vector2 force = new Vector2();
    private Vector3 mousePos = new Vector3();
    private bool facingRight = true;

    public int skillPoints = 0;
    public int hp = 3;
    public int arrows = 3;
    public float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float bowCharge;//temp, not sure if gonna use in the end
    [SerializeField] private bool isGrounded;//gonna need to set up a seperate smaller collider below the player collider to keep track of grounded
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowSpawner;

    //polish
    //reset aim after moving, maybe add some delay after aiming so wont moonwalk

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        pState = playerState.Idle;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager_Level.isGamePaused)
        {
            GetPlayerInput();
            StateMachine();

            //dunno what to do, just do jump check here
            if(!isGrounded)
            {
                animator.SetTrigger("jump");
            }
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

        if(Input.GetMouseButtonDown(0))
        {
            //FlipWithMouseAim();
            //if left click
            if (isGrounded)
            {
                bowPivot.SetActive(true);
                pState = playerState.Aiming;
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
                bowPivot.SetActive(false);
            }
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

                ////ACTUALLY DONT REALLY NEED THIS COZ CAN JUST CHANGE TO IDLE OR WALK SPRITE////
                //reset bow sprite here
                //do bowpivot transforms
                //bowPivot.transform.rotation = Quaternion.Euler(0, 180f, 0);
                //if (bowPivot.transform.localPosition.x > 0)
                //{
                //    bowPivot.transform.localPosition = new Vector3(0 - bowPivot.transform.localPosition.x, bowPivot.transform.localPosition.y);
                //}

                //if (bowPivot.transform.localScale.y > 0)
                //{
                //    bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, bowPivot.transform.localScale.y * -1, bowPivot.transform.localScale.z);
                //}

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
                pState = playerState.Idle;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (isGrounded)
                    pState = playerState.Walking;

                //move right
                moveSpeed = 2f;
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);

                //reset bow sprite here also
                //then do bowpivot transforms
                //bowPivot.transform.rotation = Quaternion.identity;
                //if (bowPivot.transform.localPosition.x < 0)
                //{
                //    bowPivot.transform.localPosition = new Vector3(0 - bowPivot.transform.localPosition.x, bowPivot.transform.localPosition.y);
                //}

                //if (bowPivot.transform.localScale.y < 0)
                //{
                //    bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, bowPivot.transform.localScale.y * -1, bowPivot.transform.localScale.z);
                //}

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
                pState = playerState.Idle;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    //jumpForce = 5f;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                    isGrounded = false;
                }
            }
        }
    }

    //below 2 functions might be enumerator, coz need to deal with eventual animations
    void Shoot()
    {
        //do shooting, handle anims then set to reloading
        //Instantiate(arrow, arrowSpawner, true);
        Instantiate(arrow, arrowSpawner.transform.position, arrowSpawner.transform.rotation);
        pState = playerState.Reloading;
        Reload();
    }

    void Reload()
    {
        //reload then go back to idle
        pState = playerState.Idle;
    }

    //check if mouse if left or right of sprite and flip accordingly
    //try local position
    void FlipWithMouseAim()
    {
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

            //then do bowpivot transforms
            if (bowPivot.transform.localPosition.x < 0)
            {
                bowPivot.transform.localPosition = new Vector3(0 - bowPivot.transform.localPosition.x, bowPivot.transform.localPosition.y);
            }

            if (bowPivot.transform.localScale.y < 0)
            {
                bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, bowPivot.transform.localScale.y * -1, bowPivot.transform.localScale.z);
            }
        }
        else
        {
            facingRight = false;

            if (!sprite.flipX)
                sprite.flipX = true;

            //then do bowpivot transforms
            if (bowPivot.transform.localPosition.x > 0)
            {
                bowPivot.transform.localPosition = new Vector3(0 - bowPivot.transform.localPosition.x, bowPivot.transform.localPosition.y);
            }

            if (bowPivot.transform.localScale.y > 0)
            {
                bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, bowPivot.transform.localScale.y * -1, bowPivot.transform.localScale.z);
            }
        }
    }

    //for when anims are done
    //maybe dont need, maybe can do in checks above, see how
    private void StateMachine()
    {
        if(isGrounded)
        {
            switch (pState)
            {
                case playerState.Idle:
                    //Debug.Log("Idle");
                    animator.SetTrigger("idle");
                    break;
                case playerState.Walking:
                    //Debug.Log("Walking");
                    animator.SetTrigger("walk");
                    break;
                case playerState.Aiming:
                    //Debug.Log("Aiming");
                    animator.SetTrigger("aim");
                    FlipWithMouseAim();
                    bowPivot.GetComponent<AimToMouse>().AimTowardMouse();
                    break;
                case playerState.Shooting:
                    //Debug.Log("Shooting");
                    break;
                case playerState.Reloading:
                    //Debug.Log("Reloading");
                    break;
                case playerState.Jumping:
                    //Debug.Log("Jumping");
                    break;
                default:
                    animator.SetTrigger("idle");
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

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log("HP: " + hp);
    }
}
