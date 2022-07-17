using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightTracker : MonoBehaviour
{
    private Transform knightTrf;
    private Transform playerTrf;
    [SerializeField] private float yThresholdToShow = 10f;
    private float xThresholdToShow;
    [SerializeField] private Transform pointer; 

    void Start()
    {
        knightTrf = FindObjectOfType<KnightController>().transform;
        playerTrf = FindObjectOfType<PlayerController>().transform;
        xThresholdToShow = yThresholdToShow * 1.7778f;
    }

    // Update is called once per frame
    void Update()
    {
        var diff = (Vector2)(knightTrf.position - playerTrf.position);
        if (Mathf.Abs(diff.x) > xThresholdToShow || Mathf.Abs(diff.y) > yThresholdToShow)
        {
            pointer.gameObject.SetActive(true);
            pointer.right = diff;
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }
       
    }
}
