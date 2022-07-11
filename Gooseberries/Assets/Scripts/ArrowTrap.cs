using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject arrowLauncher;
    public GameObject arrow;

    IEnumerator shoot;
    float timer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        shoot = Shoot();
        StartCoroutine(shoot);
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            timer -= 0.1f;

            if (timer <= 0)
            {
                FireWeapon();
                timer = 2f;
                yield return new WaitForSeconds(timer);
            }
        }
    }

    public void FireWeapon()
    {
        //fire weapon
        //spawn a bullet and move it toward the mouse i guess
        Instantiate(arrow, arrowLauncher.transform.position, arrowLauncher.transform.rotation);
    }
}
