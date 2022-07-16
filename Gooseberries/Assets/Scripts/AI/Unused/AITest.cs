using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AITest : MonoBehaviour
{
    [SerializeField] private Seeker seeker;
    [SerializeField] private Transform target;

    private void Update()
    {
        seeker.StartPath(transform.position, target.position, SeekComplete);
        
    }

    private void SeekComplete(Path path)
    {

    }

}
