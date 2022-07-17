using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class PressurePlateHandler : MonoBehaviour
    {
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public ElevatorHandler elevatorHandler;
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            isActive = true;
            elevatorHandler.ResetLerpTime();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            isActive = false;
            elevatorHandler.ResetLerpTime();
            elevatorHandler.Disengage();
        }
    }
}

