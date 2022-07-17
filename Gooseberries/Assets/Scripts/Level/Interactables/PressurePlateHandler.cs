using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class PressurePlateHandler : MonoBehaviour
    {
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public ElevatorHandler elevatorHandler;

        bool isPlayerWithinCollider = false;
        bool isKnightWithinCollider = false;
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player") isPlayerWithinCollider = true;
            if (collision.tag == "Knight") isKnightWithinCollider = true;
            isActive = true;
            elevatorHandler.ResetLerpTime();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player") isPlayerWithinCollider = false;
            if (collision.tag == "Knight") isKnightWithinCollider = false;
            if (!isPlayerWithinCollider && !isKnightWithinCollider)
            {
                isActive = false;
                elevatorHandler.ResetLerpTime();
            }
        }
    }
}

