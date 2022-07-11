using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AISearchCollider : MonoBehaviour
{
    private UnityAction<Transform> onEnterEvent;
    private UnityAction<Transform> onStayEvent;
    private UnityAction<Transform> onExitEvent;
    [SerializeField] private LayerMask obstacleLayer;
    private bool playerInSight = false;
    public void Instantiate(UnityAction<Transform> _onEnterEvent, UnityAction<Transform> _onStayEvent, UnityAction<Transform> _onExitEvent)
    {
        onEnterEvent = _onEnterEvent;
        onStayEvent = _onStayEvent;
        onExitEvent = _onExitEvent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger " + other.name);
        if (other.CompareTag("Player")){
            var obstacleHit = Physics2D.Linecast(transform.position, other.transform.position, obstacleLayer);
            if (obstacleHit.transform == null)
            {
                onEnterEvent.Invoke(other.transform);
                playerInSight = true;
            }              
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var obstacleHit = Physics2D.Linecast(transform.position, other.transform.position, obstacleLayer);
            if (playerInSight && obstacleHit.transform != null)
            {
                onExitEvent.Invoke(other.transform);
                playerInSight = false;
            }
            else
            {
                onStayEvent.Invoke(other.transform);
            }         
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInSight)
            {
                onExitEvent.Invoke(other.transform);
                playerInSight = false;
            }
            
        }
    }


}
