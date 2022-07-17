using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Suicide : MonoBehaviour
{
    public Transform[] explosionRadius;
    public GameObject ExplosionVfx;
    public void StartSuicide(UnityAction exploded, int dmg, float stunDuration,  float delay = 2, float radius = 3)
    {
        StartCoroutine(SuicideEnemy(exploded, dmg,  stunDuration,  delay, radius));
        var animator = GetComponent<Animator>();
        animator.speed = 1 / delay;
        for (int i = 0; i < explosionRadius.Length; i++)
        {
            explosionRadius[i].localScale = new Vector3(radius, radius, radius);
        }
    }
    
    IEnumerator SuicideEnemy(UnityAction exploded, int dmg, float stunDuration,  float delay, float radius)
    {
        yield return new WaitForSeconds(delay);
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("Knight"))
            {
                collider.GetComponent<IEnemySuicide>()?.ExplodedOnPlayer(dmg, stunDuration);
            }
        }
        exploded.Invoke();
        ZoneEnemyCounter.EnemyDied();
        PlayExplosionVfx();
        //EnemyManager.Destroy(this.gameObject);
    }

    public void PlayExplosionVfx()
    {
        ExplosionVfx.SetActive(true);
        Destroy(this.gameObject,.5f);
    }
}
