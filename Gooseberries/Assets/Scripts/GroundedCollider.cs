using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCollider : MonoBehaviour
{
    BoxCollider2D col;

    //fk it just get reference
    public PlayerController player;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("test collision");

        if (collision.tag == "Ground")
            player.SetIsGrounded(true);
    }
}
