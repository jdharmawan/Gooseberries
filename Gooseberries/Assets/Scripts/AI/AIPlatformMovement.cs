using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPlatformMovement : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField] private LayerMask obstacleMask;
    private float lastJumpTime;
    private float jumpCooldown = 1f;
    private float maxVelocity = 3f;
    private float maxForceX;
    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        lastJumpTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        var isGrounded = Physics2D.Raycast(transform.position,  -Vector3.up,1f, obstacleMask.value).collider==null?false:true;
        Debug.DrawLine(transform.position, transform.position - Vector3.up);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 xDirection = new Vector2(direction.x,0);
        
        Vector2 force = xDirection * speed * Time.deltaTime;

        // Jump
        if (jumpEnabled && isGrounded && (Time.time-lastJumpTime)>jumpCooldown)
        {        
            if (direction.y > jumpNodeHeightRequirement)
            {
                Debug.Log("JUMP " + Physics2D.Raycast(transform.position, -Vector3.up, 1f, obstacleMask.value).collider.name);
                rb.AddForce(Vector2.up * speed * jumpModifier);
                lastJumpTime = Time.time;
            }

        }

        // Movement
        if (rb.velocity.x < maxVelocity)
        {
            rb.AddForce(force);
            Debug.Log("AddForce");
        }
        
        //rb.velocity = new Vector2(force.x/5f,rb.velocity.y);
        //Debug.Log(rb.velocity.x);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
