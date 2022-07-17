using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirectionFilpUsingVelocity : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Update()
    {
        spriteRenderer.flipX = Vector2.Dot(transform.right, Vector2.right) < 0;
    }
}
