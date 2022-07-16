using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAIState
{
    Idle,
    Chase,
    RangeAtk,
    MeleeAtk
}

public class Enemy : MonoBehaviour
{
    public float health;
    protected float lastShootTime;
    [SerializeField] protected float shootCooldown;
    //[SerializeField] protected int ammo;
    //[SerializeField] protected float arrowDmg;
    //[SerializeField] protected float suicideDmg;
    //[SerializeField] protected float disableDuration;
}
