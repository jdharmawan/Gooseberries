using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCollider : MonoBehaviour
{
    BoxCollider2D col;

    //fk it just get reference
    public PlayerController player;
    public LayerMask layerMask;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //IsGrounded();
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.1f;

        RaycastHit2D raycasthit = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + extraHeight, layerMask);

        Color raycolor;

        if(raycasthit.collider != null)
        {
            raycolor = Color.green;
        }
        else
        {
            raycolor = Color.red;
        }

        Debug.DrawRay(col.bounds.center, Vector2.down * (col.bounds.extents.y + extraHeight), raycolor);
        Debug.Log(raycasthit.collider);

        return raycasthit.collider != null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("test collision");

        //if (collision.tag == "Ground")
        //    player.SetIsGrounded(true);
    }
}
