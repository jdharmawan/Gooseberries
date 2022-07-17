using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : Arrow
{
    void Start()
    {
        //assign values, since parent only decalre
        speed = 10.0f;
        damage = 1;
        rb2d = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        //move bullet according to heading
        rb2d.velocity = transform.right * speed;
    }

    //try smth funny
    void OnTriggerEnter2D(Collider2D collision)
    {
        //set parent to that thing that we collide with
        //rotation bugs were caused by scale. should not exist if the shield sprite is exact size
        //got bugs when release shield, should be because of rotation or flip sprite
        //kind of solved by set active to false for shield
        //if problem re occurs then fix again, if not just use this
        //if(collision.tag == "TargetBoard")
        //{
        //    rb2d.velocity = Vector2.zero;
        //    transform.parent = collision.transform;
        //}

        switch (collision.tag)
        {
            //parent 3 above
            case "TargetBoard":
                Debug.Log("targetboard");
                if(collision.GetComponentInParent<KnightController>().InsertArrow(gameObject))
                {
                    rb2d.velocity = Vector2.zero;
                    transform.parent = collision.transform;
                    boxCol.enabled = false;
                    AudioManager.instance.Play("arrow hit shield");
                }
                else
                {
                    Debug.Log("break arrow");
                    //break arrow or smth, for now destroy
                    Destroy(gameObject);
                }
                break;
            case "Player":
                //minus player hp
                collision.GetComponent<PlayerController>().TakeDamage(damage);
                break;
            default:
                break;
        }

        //lots of bugs
    }
}
