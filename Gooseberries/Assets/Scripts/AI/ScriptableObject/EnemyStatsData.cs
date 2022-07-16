using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStatsData : ScriptableObject
{
    [field: SerializeField] public float shootCooldown { get; private set; }
    [field: SerializeField] public int ammo {get; private set;}
    [field: SerializeField] public int arrowDmg {get; private set;}
    [field: SerializeField] public int suicideDmg {get; private set;}
    [field: SerializeField] public float disableDuration {get; private set;}
}
