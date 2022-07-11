using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationaryAIState { 
    Search,
    Attack
}


public class StationaryAI : MonoBehaviour
{
    [SerializeField] private AISearchCollider searchCollider;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private AISearchPatternData aiSearchPatternData;
    [SerializeField] private float shootCooldown;
    private float lastShootTime;

    private StationaryAIState curState = StationaryAIState.Search;
    private Transform playerTrf;

    public void Start()
    {
        lastShootTime = Time.time;
        searchCollider.Instantiate(PlayerInSightEnter, PlayerInSightStay, PlayerInSightExit);
    }

    public void Update()
    {
        if (curState == StationaryAIState.Search)
        {
            LerpToAngle();
        }
        else if (curState == StationaryAIState.Attack)
        {
            searchCollider.transform.right = playerTrf.position - transform.position;
            TryToShoot();
        }
    }

    public void LerpToAngle()
    {
        var curZ = searchCollider.transform.eulerAngles.z;
        var offsetRatio = Mathf.PingPong(Time.time* aiSearchPatternData.searchSpeed, 2) - 1f;
        var targetZ = offsetRatio * aiSearchPatternData.searchAngleOffset;
        searchCollider.transform.eulerAngles = new Vector3(searchCollider.transform.eulerAngles.x, searchCollider.transform.eulerAngles.y, targetZ);
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
        Debug.DrawLine(transform.position, transform.position + (transform.forward)*10);
    }

    public void PlayerInSightEnter(Transform _playerTrf)
    {
        Debug.Log("PLAYER ENTER SIGHT");
        curState = StationaryAIState.Attack;
        playerTrf = _playerTrf;
    }
    public void PlayerInSightStay(Transform _playerTrf)
    {

    }
    public void PlayerInSightExit(Transform _playerTrf)
    {
        Debug.Log("PLAYER Exit SIGHT");

        curState = StationaryAIState.Search;
    }

    //public bool FoundPlayer(out Transform hitPlayer)
    //{

    //}
}
