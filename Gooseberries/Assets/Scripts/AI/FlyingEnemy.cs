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
    

    private void Start()
    {
        detectionTrigger.Instantiate(InDetectionEnter, InDetectionStay, InDetectionExit);
        rangeTrigger.Instantiate(InRangeEnter, InRangeStay, InRangeExit);
        meleeTrigger.Instantiate(InMeleeEnter, InMeleeStay, InMeleeExit);
        playerTrf = FindObjectOfType<PlayerController>().transform;
        var seekGameobject = new GameObject("FlyingEnemySeek");
        seekTarget = seekGameobject.transform;
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = seekTarget;
        suicideTrigger.Initialize(SuicideOnEnter);

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
        StateChangeCheck();

        if (curState == EnemyAIState.Idle)
        {
            seekTarget.position = transform.position;
        }
        else if(curState == EnemyAIState.Chase)
        {
            seekTarget.position = playerTrf.position;
        }
        else if (curState == EnemyAIState.RangeAtk)
        {
            seekTarget.position = transform.position;
            aim.transform.right = playerTrf.position - transform.position;
            if(stats.ammo - arrowShot <= 0)
            {
                curState = EnemyAIState.MeleeAtk;
            }
            else
            {
                TryToShoot();
            }
            
        }
        else if (curState == EnemyAIState.MeleeAtk)
        {
            seekTarget.position = playerTrf.position;
        }
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
        suicideObj.GetComponent<Suicide>()?.StartSuicide(Exploded, stats.suicideDmg, stats.disableDuration);
        Destroy(gameObject);
        
    }

    public void Exploded()
    {

    }
}
