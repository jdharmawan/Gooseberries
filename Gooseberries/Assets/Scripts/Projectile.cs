using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float speed;
    protected int damage;
    protected Rigidbody2D rb2d;
    protected BoxCollider2D boxCol;

    //switch to trigger detection
    //shifted this here from bullet and missile, so that more optimized, dont need to keep writing in child classes
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit enemy, minus hp
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("TakeDamage", damage);

            Destroy(gameObject);
        }
        //if hit player do nothing
        else if (collision.gameObject.tag == "Player" ||
                 collision.gameObject.tag == "Projectile" ||
                 collision.gameObject.tag == "Weapon")
        { }
        //if hit anything else, just destroy projectile
        else
        {
            //eventually do stuff like effects or what
            Destroy(gameObject);
        }
    }
}