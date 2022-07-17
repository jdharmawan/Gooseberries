using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PressurePlateHandler : MonoBehaviour
    {
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public ElevatorHandler elevatorHandler;

        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite inactiveSprite;

        bool isPlayerWithinCollider = false;
        bool isKnightWithinCollider = false;
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player") isPlayerWithinCollider = true;
            if (collision.tag == "Knight") isKnightWithinCollider = true;
            isActive = true;
            elevatorHandler.ResetLerpTime();
            GetComponent<SpriteRenderer>().sprite = activeSprite;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player") isPlayerWithinCollider = false;
            if (collision.tag == "Knight") isKnightWithinCollider = false;
            if (!isPlayerWithinCollider && !isKnightWithinCollider)
            {
                isActive = false;
                elevatorHandler.ResetLerpTime();
                GetComponent<SpriteRenderer>().sprite = inactiveSprite;

            }
        }
    }
}

