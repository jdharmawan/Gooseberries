using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public Sprite sprite = null;
    public float damage = 1f;
    public float force = 10f;
    public float mass = 1f;
    public float gravityScale = 10f;
}
