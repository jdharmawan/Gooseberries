using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] private AISearchCollider detectionTrigger;
    [SerializeField] private AISearchCollider rangeTrigger;
    [SerializeField] private AISearchCollider meleeTrigger;
    [SerializeField] private Transform aim;
    [SerializeField] private Transform seekTarget;
    [SerializeField] private GameObject arrowPrefab;
    private Transform playerTrf;
    private EnemyAIState curState = EnemyAIState.Idle;
    private float lastPlayerDetectedTime;
    [SerializeField] private float loseInterestDuration;
    private bool exited = false;

    //private bool inDetection = false;
    //private bool inRange = false;
    //private bool inMelee= false;

    private void Start()
    {
        detectionTrigger.Instantiate(InDetectionEnter, InDetectionStay, InDetectionExit);
        rangeTrigger.Instantiate(InRangeEnter, InRangeStay, InRangeExit);
        meleeTrigger.Instantiate(InMeleeEnter, InMeleeStay, InMeleeExit);
        playerTrf = FindObjectOfType<PlayerController>().transform;
    }
    public void TryToShoot()
    {
        if (Time.time - lastShootTime > shootCooldown)
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
            seekTarget.position = playerTrf.position;
            aim.transform.right = playerTrf.position - transform.position;
            TryToShoot();
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

        //if(inMelee == true && curState!=EnemyAIState.MeleeAtk)
        //{
        //    curState = EnemyAIState.MeleeAtk;
        //}
        //else if(inRange == true && curState != EnemyAIState.RangeAtk)
        //{
        //    curState = EnemyAIState.RangeAtk;
        //}
        //else if (inDetection == true && curState != EnemyAIState.Chase)
        //{

        //}
        //else
        //{

        //}
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
        exited = true;
    }

    public void InRangeEnter(Transform player)
    {
        Debug.Log("State: InRangeEnter");

        if (curState == EnemyAIState.Chase || curState == EnemyAIState.Idle)
        {
            curState = EnemyAIState.RangeAtk;
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
            curState = EnemyAIState.RangeAtk;
        }
    }
}
