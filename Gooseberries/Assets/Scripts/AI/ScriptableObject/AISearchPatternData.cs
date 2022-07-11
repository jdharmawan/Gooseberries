using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AISearchPatternData", menuName = "ScriptableObjects/AISearchPatternData")]
public class AISearchPatternData : ScriptableObject
{
    public float searchAngleOffset = 20f;
    /// <summary>
    /// ratio per second
    /// </summary>
    public float searchSpeed = 1f;
}
