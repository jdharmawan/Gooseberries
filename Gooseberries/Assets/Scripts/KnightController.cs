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
public class KnightController : MonoBehaviour
{
    public GameObject princess;
    public GameObject shieldPivot;
    public GameObject shieldObject;
    public GameObject shieldPlatform;

    private CapsuleCollider2D col;
    private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int arrowCount = 0;

    private Vector3 mousePos = new Vector3();
    private float distToPrincess = 0f;

    private bool facingRight = true;
    private bool shieldOut = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        shieldPlatform.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //actually maybe shield can be toggle
        //do some QOL where the knight teleports to the princess
        if (Input.GetMouseButtonDown(1))
        {
            //if right click do shield
            shieldOut = true;
            rb2d.velocity = Vector3.zero;
            rb2d.isKinematic = true;
            //activate shield platform
            shieldPlatform.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            //release shield
            shieldOut = false;
            rb2d.velocity = Vector3.zero;
            rb2d.isKinematic = false;
            //deactivate shield platform
            shieldPlatform.SetActive(false);
        }

        if(shieldOut)
        {
            shieldPivot.GetComponent<AimToMouse>().AimTowardMouse();
            FlipWithMouseAim();
        }
        else
        {
            //revert to default rotation
            //again, actually not required, just doing this for now coz we dont have the sprites
            if(facingRight)
            {
                shieldPivot.transform.rotation = Quaternion.identity;
            }
            else
            {
                shieldPivot.transform.rotation = Quaternion.Euler(0, 180f, 0);
            }

            FollowPrincess();
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
            rb2d.velocity = Vector2.zero;
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
}
