using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k what we need this guy to do
//always try to stick with princess position (x value)
//right click to enagage shield
//idea: when bring up shield, check if princess is in range, if so do a sort of teleport or relocate so that will center position to her and shield in front (relative to mouse)
//current undesired interactions: shield collides with princess, and princess can move knight because of this
//might need to kinematic him while shield is out
public class KnightController : MonoBehaviour
{
    public GameObject shieldPivot;

    private BoxCollider2D col;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
