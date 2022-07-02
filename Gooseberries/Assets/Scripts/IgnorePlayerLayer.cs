using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerLayer : MonoBehaviour
{
    Collider col;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        rb2d = GetComponent<Rigidbody2D>();
    }
}
