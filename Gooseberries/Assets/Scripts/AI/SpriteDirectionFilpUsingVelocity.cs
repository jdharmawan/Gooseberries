using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirectionFilpUsingVelocity : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Update()
    {
        spriteRenderer.flipX = rb.velocity.x < 0;
    }
}
