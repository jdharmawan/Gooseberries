using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : Enemy
{
    [SerializeField] private AISearchCollider detectionTrigger;
    [SerializeField] private AISearchCollider rangeTrigger;
    [SerializeField] private AISearchCollider meleeTrigger;
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private SuicideTrigger suicideTrigger;
    [SerializeField] private Suicide suicide;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private Animator animator;
    private Transform playerTrf;
    private EnemyAIState curState = EnemyAIState.Idle;
    private float lastPlayerDetectedTime;
    [SerializeField] private float loseInterestDuration;
    private bool exited = false;
    private Transform seekTarget;
    private AIDestinationSetter destinationSetter;
    private int arrowShot = 0;
    //private bool inDetection = false;
    //private bool inRange = false;
    //private bool inMelee= false;
    private bool IsExploding = false;
    private bool isDead = false;
    [SerializeField] private GameObject[] objectsToDisableOnDeath;


    [Header("Ground enemies")]
    private AIPlatformMovement aiPlatformMovement;

    private void Start()
    {
        detectionTrigger.Instantiate(InDetectionEnter, InDetectionStay, InDetectionExit);
        rangeTrigger.Instantiate(InRangeEnter, InRangeStay, InRangeExit);
        meleeTrigger.Instantiate(InMeleeEnter, InMeleeStay, InMeleeExit);
        playerTrf = FindObjectOfType<PlayerController>().transform;
        var seekGameobject = new GameObject("FlyingEnemySeek");
        seekTarget = seekGameobject.transform;
        destinationSetter = GetComponent<AIDestinationSetter>();
        if(destinationSetter != null)
            destinationSetter.target = seekTarget;
        suicideTrigger.Initialize(SuicideOnEnter);
        aiPlatformMovement = GetComponent<AIPlatformMovement>();
        if (aiPlatformMovement != null)
            aiPlatformMovement.target = seekTarget;

    }
    public void TryToShoot()
    {
        if (Time.time - lastShootTime > stats.shootCooldown)
        {
            lastShootTime = Time.time;
            Shoot();
        }
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        Debug.DrawLine(transform.position, transform.position + (transform.forward) * 10);
        var arrow = Instantiate(arrowPrefab, transform.position, aim.transform.rotation);
        arrowShot += 1;
        //.transform.right = searchCollider.transform.right;
    }

    private void Update()
    {
        if (isDead)
        {
            aiPlatformMovement.enabled = false;
            var aipath = GetComponent<AIPath>();
            if (aipath != null)
                aipath.enabled = false;
            return;
        }
        StateChangeCheck();

        if (curState == EnemyAIState.Idle)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Chasing", false);
            animator.SetBool("Range", false);
            animator.SetBool("Melee", false);
            seekTarget.position = transform.position;
        }
        else if(curState == EnemyAIState.Chase)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Chasing", true);
            animator.SetBool("Range", false);
            animator.SetBool("Melee", false);
            seekTarget.position = playerTrf.position;
        }
        else if (curState == EnemyAIState.RangeAtk)
        {
            seekTarget.position = transform.position;
            if(stats.ammo - arrowShot <= 0)
            {
                curState = EnemyAIState.MeleeAtk;
            }
            else
            {
                TryToShoot();
                animator.SetBool("Idle", false);
                animator.SetBool("Chasing", false);
                animator.SetBool("Range", true);
                animator.SetBool("Melee", false);
            } 
        }
        else if (curState == EnemyAIState.MeleeAtk)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Chasing", false);
            animator.SetBool("Range", false);
            animator.SetBool("Melee", true);
            seekTarget.position = playerTrf.position;
        }
        aim.transform.right = playerTrf.position - transform.position;
        
    }

    public void StateChangeCheck()
    {       
        if(exited && Time.time - lastPlayerDetectedTime > loseInterestDuration)
        {
            Debug.Log("State: Change to idle " + Time.time);
            curState = EnemyAIState.Idle;
            exited = false;
        }
    }

    public void InDetectionEnter(Transform player)
    {
        Debug.Log("State: InDetectionEnter");
        if (curState == EnemyAIState.Idle)
        {
            curState = EnemyAIState.Chase;
        }
    }

    public void InDetectionStay(Transform player)
    {

    }

    public void InDetectionExit(Transform player)
    {
        Debug.Log("State: InDetectionExit");
        lastPlayerDetectedTime = Time.time;
        curState = EnemyAIState.Chase;
        exited = true;
    }

    public void InRangeEnter(Transform player)
    {
        Debug.Log("State: InRangeEnter");

        if (curState == EnemyAIState.Chase || curState == EnemyAIState.Idle)
        {
            if(stats.ammo - arrowShot > 0)
            {
                curState = EnemyAIState.RangeAtk;
            }
            else
            {
                curState = EnemyAIState.MeleeAtk;
            }

        }
    }

    public void InRangeStay(Transform player)
    {

    }

    public void InRangeExit(Transform player)
    {
        if (curState == EnemyAIState.Chase || curState == EnemyAIState.Idle)
        {
            curState = EnemyAIState.Chase;
        }
    }

    public void InMeleeEnter(Transform player)
    {
        Debug.Log("State: InMeleeEnter");

        if (curState == EnemyAIState.Chase || curState == EnemyAIState.Idle || curState == EnemyAIState.RangeAtk)
        {
            curState = EnemyAIState.MeleeAtk;
        }
    }

    public void InMeleeStay(Transform player)
    {

    }

    public void InMeleeExit(Transform player)
    {

        if (curState == EnemyAIState.MeleeAtk)
        {
            if (stats.ammo - arrowShot > 0)
            {
                curState = EnemyAIState.RangeAtk;
            }
            else
            {
                curState = EnemyAIState.MeleeAtk;
            }
        }
    }

    public void SuicideOnEnter()
    {
        Debug.Log("SUICIDE on enter");
        var suicideObj = Instantiate(suicide.gameObject, transform.position, transform.rotation);
        suicideObj.GetComponent<Suicide>()?.StartSuicide(Exploded, stats.suicideDmg, stats.shieldDmg);
        Destroy(gameObject);
        
    }

    public void Exploded()
    {

    }

    public void TakeDamage(int dmg, Vector2 flyDirection)
    {
        health -= dmg;
        StartCoroutine(FlashRedEnumerator(1));
        if (health <= 0)
            Die(flyDirection);
    }

    IEnumerator FlashRedEnumerator(float duration)
    {
        var startTime = Time.time;
        Debug.Log(Time.time - startTime < duration);
        while(Time.time - startTime < duration)
        {
            float redvalue = Mathf.PingPong((Time.time - startTime) * 2, duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, redvalue, redvalue, spriteRenderer.color.a);
            Debug.Log(spriteRenderer.color.r);
            yield return null;
        }
        spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a);
    }

    public void Die(Vector2 flyDirection)
    {
        ZoneCounter.EnemyDied();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .7f);
        for (int i = 0; i < objectsToDisableOnDeath.Length; i++)
        {
            objectsToDisableOnDeath[i].SetActive(false);
        }
        aiPath.enabled = false;
        //detectionTrigger.enabled = false;
        //meleeTrigger.enabled = false;
        //rangeTrigger.enabled = false;
        //suicideTrigger.enabled = false;
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(new Vector2(flyDirection.normalized.x*5, 10), ForceMode2D.Impulse);
            if (Random.Range(-1, 1) > 0)
            {
                rb.AddTorque(40);
            }
            else
            {
                rb.AddTorque(-40);
            }
            
        }
        var capsule = GetComponent<CapsuleCollider2D>();
        if (capsule != null)
            capsule.enabled = false;
        StartCoroutine(DieEnumerator());
    }

    IEnumerator DieEnumerator()
    {
        yield return new WaitForSeconds(7f);
        Destroy(gameObject);
        
    }
}
