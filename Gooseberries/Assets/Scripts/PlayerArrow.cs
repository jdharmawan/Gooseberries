using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : Arrow
{
    void Start()
    {
        //assign values, since parent only decalre
        speed = 5.0f;
        damage = 1;
        rb2d = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        //move bullet according to heading
        rb2d.velocity = transform.right * speed;
    }

    //try smth funny
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            //minus enemy hp
            Debug.Log("Hit enemy");
        }
    }
}
