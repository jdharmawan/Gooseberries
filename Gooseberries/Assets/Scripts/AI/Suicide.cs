using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Suicide : MonoBehaviour
{
    public Transform[] explosionRadius;
    public GameObject ExplosionVfx;
    public void StartSuicide(UnityAction exploded, int dmg, float shieldDmg,  float delay = 2, float radius = 3f)
    {
        StartCoroutine(SuicideEnemy(exploded, dmg,  shieldDmg,  delay, radius));
        var animator = GetComponent<Animator>();
        animator.speed = 1 / delay;
        for (int i = 0; i < explosionRadius.Length; i++)
        {
            explosionRadius[i].localScale = new Vector3(radius, radius, radius);
        }
    }
    
    IEnumerator SuicideEnemy(UnityAction exploded, int dmg, float shieldDmg,  float delay, float radius)
    {
        yield return new WaitForSeconds(delay);
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius/2);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("Knight"))
            {
                collider.GetComponent<IReceiveExplosion>()?.ExplodedOnPlayer(dmg, shieldDmg);
            }
        }
        exploded.Invoke();
        ZoneCounter.EnemyDied();
        PlayExplosionVfx();
        //EnemyManager.Destroy(this.gameObject);
    }

    public void PlayExplosionVfx()
    {
        ExplosionVfx.SetActive(true);
        Destroy(this.gameObject,.5f);
    }
}
