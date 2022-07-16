using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuicideTrigger : MonoBehaviour
{
    private UnityAction onEnter;
    public void Initialize(UnityAction _onEnter)
    {
        onEnter = _onEnter;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            onEnter.Invoke();
        }
    }
}
