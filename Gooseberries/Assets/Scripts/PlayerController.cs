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

public struct SavedPlayer
{
    public int skillPoints ;
    public int maxHP;
    public int currHP;
    public int maxArrows;
    public int currArrows;
    public float moveSpeed;
    public int vitalityLevel ;
    public int quiverLevel ;
    public int speedLevel;

    public SavedPlayer (int _skillPoints, int _maxHP, int _currHP, int _maxArrows, int _currArrows, float _moveSpeed, int _vitalityLevel, int _quiverLevel, int _speedLevel)
    {
        skillPoints = _skillPoints;
        maxHP = _maxHP;
        currHP = _currHP;
        maxArrows = _maxArrows;
        currArrows = _currArrows;
        moveSpeed = _moveSpeed;
        vitalityLevel = _vitalityLevel;
        quiverLevel = _quiverLevel;
        speedLevel = _speedLevel;
    }
}

public class PlayerController : MonoBehaviour, IReceiveExplosion
{
    //[SerializeField] private BoxCollider2D groundedCol;
    public GameObject bowPivot;
    public Transform bowstringTop, bowstringBottom, pullbackPos;
    public LineRenderer lineRenderer;
    public LayerMask layerMask;

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
    private GameObject tempArrow;
    public KnightController knight;
    #region Saved player
    public int maxHP = 3;
    public int currHP = 3;
    public int maxArrows = 3;
    public int currArrows = 3;
    public int skillPoints = 0;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int vitalityLevel = 1;
    [HideInInspector] public int quiverLevel = 1;
    [HideInInspector] public int speedLevel = 1;
    #endregion

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float bowCharge;//temp, not sure if gonna use in the end
    [SerializeField] private bool isGrounded;//gonna need to set up a seperate smaller collider below the player collider to keep track of grounded
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowSpawner;
    private GameManager_Level gameLevel;
    private bool deathAnimation = false;
    //polish
    //reset aim after moving, maybe add some delay after aiming so wont moonwalk

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        pState = playerState.Idle;
        isGrounded = true;
        moveSpeed = 2f;
        lineRenderer.gameObject.SetActive(false);
        knight = FindObjectOfType<KnightController>();
        gameLevel = FindObjectOfType<GameManager_Level>();
    }
    // Update is called once per frame
    void Update()
    {
        if (deathAnimation)
            return;
        if (!GameManager_Level.isGamePaused)
        {
            if (!GameManager_Level.isPlayerLocked) GetPlayerInput();
            StateMachine();

            isGrounded = IsGrounded();
            //dunno what to do, just do jump check here
            if(!isGrounded)
            {
                animator.SetTrigger("jump");
            }

            CloseToKnight(knight.transform);
        }

        SetRenderLines();
    }
    public void SetPlayerSavedData(SavedPlayer savePlayer)
    {
        skillPoints = savePlayer.skillPoints;
        maxHP = savePlayer.maxHP;
        currHP = savePlayer.currHP;
        maxArrows = savePlayer.maxArrows;
        currArrows = savePlayer.currArrows;
        moveSpeed = savePlayer.moveSpeed;
        vitalityLevel = savePlayer.vitalityLevel;
        quiverLevel = savePlayer.quiverLevel;
        speedLevel = savePlayer.speedLevel;
    }
    public SavedPlayer GetPlayerSavedData()
    {
        return new SavedPlayer(skillPoints, maxHP, currHP, maxArrows, currArrows, moveSpeed, vitalityLevel, quiverLevel, speedLevel);
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
                rb2d.velocity = Vector2.zero;
                bowPivot.SetActive(true);
                lineRenderer.gameObject.SetActive(true);
                pState = playerState.Aiming;
                SpawnArrow();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //when release mouse, shoot arrow and maybe idle, or reload
            //check just in case i guess
            if (pState == playerState.Aiming)
            {
                ////check arrow count
                //if(currArrows > 0)
                //{
                pState = playerState.Shooting;
                Shoot();
                //}
                bowPivot.SetActive(false);
                lineRenderer.gameObject.SetActive(false);
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
                //moveSpeed = -2f;
                rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);

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
                //moveSpeed = 2f;
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
    void SpawnArrow()
    {
        //tempArrow = Instantiate(arrow, arrowSpawner.transform.position, arrowSpawner.transform.rotation);
        if (currArrows > 0)
            tempArrow = Instantiate(arrow, arrowSpawner);
    }


    //below 2 functions might be enumerator, coz need to deal with eventual animations
    void Shoot()
    {
        //do shooting, handle anims then set to reloading
        //Instantiate(arrow, arrowSpawner, true);
        if (currArrows > 0)
        {
            tempArrow.GetComponent<PlayerArrow>().Fire();
            tempArrow.transform.parent = null;
            currArrows--;
        }
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
    private void SetRenderLines()
    {
        var points = new Vector3[3];

        //points[0] = bowstringTop.transform.position;
        //points[1] = pullbackPos.transform.position;
        //points[2] = bowstringBottom.transform.position;

        points[0] = new Vector3(bowstringTop.transform.position.x, bowstringTop.transform.position.y, bowstringTop.transform.position.z - 0.01f);
        points[1] = new Vector3(pullbackPos.transform.position.x, pullbackPos.transform.position.y, pullbackPos.transform.position.z - 0.01f);
        points[2] = new Vector3(bowstringBottom.transform.position.x, bowstringBottom.transform.position.y, bowstringBottom.transform.position.z - 0.01f);

        lineRenderer.SetPositions(points);
    }

    //maybe no need anymore
    //public void SetIsGrounded(bool b)
    //{
    //    isGrounded = b;
    //}

    private bool IsGrounded()
    {
        float extraHeight = 0.1f;

        //RaycastHit2D raycasthit = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + extraHeight, layerMask);

        //change to circlecast?
        RaycastHit2D raycasthit = Physics2D.CircleCast(col.bounds.center, (col.size.x/2), Vector2.down, extraHeight + (col.size.y/2), layerMask);

        Color raycolor;

        if (raycasthit.collider != null)
        {
            //Debug.Log(raycasthit.collider.name);
            raycolor = Color.green;
            //if (transform.position.y - raycasthit.point.y > col.size.y / 2)
            //{
            //    return false;
            //}
            //else
            //    return true;
        }
        else
        {
            raycolor = Color.red;
            //return false;
        }

        //Debug.DrawRay(col.bounds.center, Vector2.down * (extraHeight + (col.size.y / 2)), raycolor);
        //Debug.Log(raycasthit.collider);

        return raycasthit.collider != null;
    }


    public void TakeDamage(int dmg)
    {
        currHP -= dmg;
        if (currHP <= 0)
        {
            StartCoroutine(Death());
        }
        Debug.Log("HP: " + currHP);
        StartCoroutine(FlashRedEnumerator(.5f));
    }
    IEnumerator FlashRedEnumerator(float duration)
    {
        var startTime = Time.time;
        Debug.Log(Time.time - startTime < duration);
        while (Time.time - startTime < duration)
        {
            float redvalue = Mathf.PingPong((Time.time - startTime) * 2, duration);
            sprite.color = new Color(sprite.color.r, redvalue, redvalue, sprite.color.a);
            Debug.Log(sprite.color.r);
            yield return null;
        }
        sprite.color = new Color(1, 1, 1, sprite.color.a);
    }
    //check range to knight with distance
    void CloseToKnight(Transform t)
    {
        if (Vector3.Distance(transform.position, t.position) < 1f)
        {
            if (currArrows < maxArrows)
            {
                currArrows += knight.RefillArrows(maxArrows - currArrows);
            }
        }
    }

    public void ExplodedOnPlayer(int dmg, float shieldDmg)
    {
        TakeDamage(dmg);
        Debug.Log("player hit by explosion");
    }
    [ContextMenu("death")]
    private void TestDeath()
    {
        StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        deathAnimation = true;
        float duration = 2f;
        float startTime = Time.time;
        var princessRenderer = GetComponent<SpriteRenderer>();
        var knightRenderer = knight.GetComponent<SpriteRenderer>();
        while (Time.time - startTime < duration)
        {
            yield return null;
            princessRenderer.color = new Color(princessRenderer.color.r, princessRenderer.color.g, princessRenderer.color.b, 1-(Time.time - startTime)/duration);
            knightRenderer.color = new Color(knightRenderer.color.r, knightRenderer.color.g, knightRenderer.color.b, 1-(Time.time - startTime)/duration);
        }
        princessRenderer.color = new Color(princessRenderer.color.r, princessRenderer.color.g, princessRenderer.color.b, 1);
        knightRenderer.color = new Color(knightRenderer.color.r, knightRenderer.color.g, knightRenderer.color.b, 1);      
        //var princessRb = GetComponent<Rigidbody2D>();
        //var knightRb = GetComponent<Rigidbody2D>();
        //Debug.Log("RB: " + princessRb);
        //if (princessRb != null)
        //{
        //    princessRb.constraints = RigidbodyConstraints2D.None;
        //    princessRb.AddForce(new Vector2(Random.Range(-100, 100), 200), ForceMode2D.Impulse);
        //    knightRb.constraints = RigidbodyConstraints2D.None;
        //    knightRb.AddForce(new Vector2(Random.Range(-100, 100), 200), ForceMode2D.Impulse);
        //    if (Random.Range(-1, 1) > 0)
        //    {
        //        princessRb.AddTorque(40);
        //        knightRb.AddTorque(40);
        //    }
        //    else
        //    {
        //        princessRb.AddTorque(-40);
        //        knightRb.AddTorque(-40);
        //    }
        //}
        //var knightCol = knight.GetComponent<CapsuleCollider2D>();
        //var princessCol = GetComponent<CapsuleCollider2D>();
        //knightCol.enabled = false;
        //princessCol.enabled = false;
        //yield return new WaitForSeconds(1f);
        //princessCol.enabled = true;
        //princessRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //princessRb.velocity = new Vector2(0, 0);
        //princessRb.transform.rotation = new Quaternion();

        //knightCol.enabled = true;
        //knightRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //knightRb.velocity = new Vector2(0, 0);
        //knightRb.transform.rotation = new Quaternion();
        deathAnimation = false;
        gameLevel.ResetToLastCheckpoint();
    }
}
