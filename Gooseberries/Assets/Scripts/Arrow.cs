using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    void Start()
    {
        //assign values, since parent only decalre
        speed = 5.0f;
        damage = 1;
        rb2d = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        //move bullet according to heading
        rb2d.velocity = Vector2.right * speed;
    }

    //try smth funny
    void OnTriggerEnter2D(Collider2D collision)
    {
        //set parent to that thing that we collide with
        rb2d.velocity = Vector2.zero;
        transform.parent = collision.transform;

        //lots of bugs
    }
}
