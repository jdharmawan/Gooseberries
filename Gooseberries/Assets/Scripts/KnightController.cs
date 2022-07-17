using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k what we need this guy to do
//always try to stick with princess position (x value)
//right click to enagage shield
//idea: when bring up shield, check if princess is in range, if so do a sort of teleport or relocate so that will center position to her and shield in front (relative to mouse)
//current undesired interactions: shield collides with princess, and princess can move knight because of this
//might need to kinematic him while shield is out
//fk im dumb, can just apply counter force or just set his velocity to 0
public class KnightController : MonoBehaviour, IReceiveExplosion
{
    public GameObject princess;
    public GameObject shieldPivot;
    public GameObject shieldObject;
    public GameObject shieldPlatform;

    private CapsuleCollider2D col;
    private Rigidbody2D rb2d;
    
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private float shieldStaminaRegen = 0.001f;
    [SerializeField] private int arrowCount = 0;

    public float moveSpeed = 2f;
    public float maxShieldStamina = 5f;
    public float currShieldStamina = 0f;

    private Vector3 mousePos = new Vector3();
    private float distToPrincess = 0f;
    private bool facingRight = true;
    private enum KnightState {Follow, ShieldOut, Platform, Disabled};
    private KnightState knightState = KnightState.Follow;
    private List<GameObject> arrowStock = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        shieldPlatform.SetActive(false);
        shieldObject.SetActive(false);

        arrowStock.Clear();
        arrowCount = arrowStock.Count;
        currShieldStamina = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //actually maybe shield can be toggle
        //do some QOL where the knight teleports to the princess
        if (Input.GetMouseButtonDown(1))
        {
            if (knightState == KnightState.Follow)
            {
                //if right click do shield
                knightState = KnightState.ShieldOut;
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.isKinematic = false;
                shieldObject.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (knightState == KnightState.ShieldOut)
            {
                //release shield
                knightState = KnightState.Follow;
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.isKinematic = false;
                shieldObject.SetActive(false);
            }
        }

        //do F input to make knight become platform
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (knightState == KnightState.Follow)
            {
                knightState = KnightState.Platform;
                //activate shield platform
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.isKinematic = false;
                shieldPlatform.SetActive(true);
            }
            else if(knightState == KnightState.Platform)
            {
                knightState = KnightState.Follow;
                //deactivate shield platform
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.isKinematic = false;
                shieldPlatform.SetActive(false);
            }
        }

        switch(knightState)
        {
            //case KnightState.Follow:
            //    break;
            case KnightState.ShieldOut:
                animator.SetTrigger("shield up");
                shieldPivot.GetComponent<AimToMouse>().AimTowardMouse();
                FlipWithMouseAim();

                //half shield regen
                RegenShieldStamina(shieldStaminaRegen / 2);
                break;
            case KnightState.Platform:
                animator.SetTrigger("shield up");
                RegenShieldStamina(shieldStaminaRegen);
                break;
            case KnightState.Disabled:
                RegenShieldStamina(shieldStaminaRegen * 2);
                if (currShieldStamina >= maxShieldStamina)
                    knightState = KnightState.Follow;
                break;
            default:
                //revert to default rotation
                //again, actually not required, just doing this for now coz we dont have the sprites
                //if (facingRight)
                //{
                //    shieldPivot.transform.rotation = Quaternion.identity;
                //}
                //else
                //{
                //    shieldPivot.transform.rotation = Quaternion.Euler(0, 180f, 0);
                //}
                animator.SetTrigger("idle");
                FollowPrincess();
                RegenShieldStamina(shieldStaminaRegen);
                break;
        }
    }

    void FollowPrincess()
    {
        //try to stick to princess as much as possible, but only on the x axis
        //oh yeah i think i did this before, need to have this small range where the knight will stop, otherwise he will keep moving back and forth
        distToPrincess = princess.transform.position.x - transform.position.x;

        if (-0.1f <= distToPrincess && distToPrincess <= 0.1f)
        {
            //do nothing
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        else if (distToPrincess > 0.1f)
        {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            facingRight = true;
            if (sprite.flipX)
                sprite.flipX = false;
        }
        else if(distToPrincess < 0.1f)
        {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            facingRight = false;
            if (!sprite.flipX)
                sprite.flipX = true;
        }
    }

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

            //then do shieldPivot transforms
            if (shieldPivot.transform.localPosition.x < 0)
            {
                shieldPivot.transform.localPosition = new Vector3(0 - shieldPivot.transform.localPosition.x, shieldPivot.transform.localPosition.y);
            }

            if (shieldPivot.transform.localScale.y < 0)
            {
                shieldPivot.transform.localScale = new Vector3(shieldPivot.transform.localScale.x, shieldPivot.transform.localScale.y * -1, shieldPivot.transform.localScale.z);
            }
        }
        else
        {
            facingRight = false;

            if (!sprite.flipX)
                sprite.flipX = true;

            //then do shieldPivot transforms
            if (shieldPivot.transform.localPosition.x > 0)
            {
                shieldPivot.transform.localPosition = new Vector3(0 - shieldPivot.transform.localPosition.x, shieldPivot.transform.localPosition.y);
            }

            if (shieldPivot.transform.localScale.y > 0)
            {
                shieldPivot.transform.localScale = new Vector3(shieldPivot.transform.localScale.x, shieldPivot.transform.localScale.y * -1, shieldPivot.transform.localScale.z);
            }
        }
    }

    public bool InsertArrow(GameObject arrow)
    {
        Debug.Log("insert arrow");
        //call this func from enemy arrow
        //inserts arrow into array to keep track
        //when princess comes near, autorefill and destroy arrows from array
        if (arrowStock.Count < 5)
        {
            arrowStock.Add(arrow);
            //arrowCount = arrowStock.Count;
            return true;
        }
        //else do bounce arrow, dunno if wnat to do here
        else
            return false;
    }

    public int RefillArrows(int arrowsNeeded)
    {
        int arrowsReturned = 0;
        //refill arrows for princess
        //if got enough or less, clear all and return amount
        if (arrowStock.Count == 0)
            return 0;
        else if (arrowStock.Count <= arrowsNeeded)
        {
            arrowsReturned = arrowStock.Count;
            Debug.Log("arrows returned 1: " + arrowsReturned);

            foreach (GameObject arrow in arrowStock)
                Destroy(arrow);

            Debug.Log("arrow stock 1: " + arrowStock.Count);
            arrowStock.Clear();
            Debug.Log("arrow stock 2: " + arrowStock.Count);
        }
        //if got too much, return until enough
        else
        {
            //get num arrows to return
            arrowsReturned = arrowsNeeded;
            Debug.Log("arrows returned 2: " + arrowsReturned);

            for (int i = 0; i < arrowsNeeded; i++)
                Destroy(arrowStock[i]);

            Debug.Log("arrow stock 3: " + arrowStock.Count);
            arrowStock.RemoveRange(0, arrowsNeeded);
            Debug.Log("arrow stock 4: " + arrowStock.Count);
        }

        Debug.Log("RELOAD!");
        Debug.Log("arrowstock: " + arrowStock.Count);
        return arrowsReturned;
    }

    public void ExplodedOnPlayer(int dmg, float shieldDmg)
    {
        if(knightState == KnightState.ShieldOut)
        {
            currShieldStamina -= shieldDmg;
            Debug.Log("knight hit by explosion");

            if (currShieldStamina <= 0)
            {
                knightState = KnightState.Disabled;
            }
        }
        else
        {
            //if not shield, explosion will immediately stun
            currShieldStamina = 0f;
            knightState = KnightState.Disabled;
        }
    }

    void RegenShieldStamina(float f)
    {
        if(currShieldStamina < maxShieldStamina)
            currShieldStamina += f;

        Debug.Log("currShieldStamina: " + currShieldStamina);
    }
}
