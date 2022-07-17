using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class ElevatorHandler : MonoBehaviour
    {
        Coroutine cor;

        [HideInInspector] public Cinemachine.CinemachineVirtualCamera virtualCamera;
        [HideInInspector] public GameManager_Level levelManager;
        [HideInInspector] public bool isActivating = false;

        bool isEngaged = false;

        [SerializeField] Vector2 targetPosition;
        [SerializeField] List<LeverHandler> leversToActivate;
        [SerializeField] List<PressurePlateHandler> pressurePlateToActivate;

        float lerpTime = 0f;
        Vector2 curPosition;

        public bool isCameraZoom = true;

        private void Start()
        {
            Initialise_Activators();
        }

        private void Update()
        {
            if (!isActivating)
            {
                if (IsActivated() && !isEngaged)
                    cor = StartCoroutine(ElevatorActivating());
            }
            if (!IsActivated())
            {
                Disengage();
            }
                
        }

        void Initialise_Activators()
        {
            for (int i = 0; i < pressurePlateToActivate.Count; i++)
            {
                pressurePlateToActivate[i].elevatorHandler = this;
            }
        }

        bool IsActivated()
        {
            for (int i = 0; i < leversToActivate.Count; i++)
            {
                if (!leversToActivate[i].isActive)
                    return false;
            }
            for (int i = 0; i < pressurePlateToActivate.Count; i++)
            {
                if (!pressurePlateToActivate[i].isActive)
                    return false;
            }
            return true;
        }

        public void Disengage()
        {
            isEngaged = false;
        }
        public void ResetLerpTime()
        {
            lerpTime = 0f;
            curPosition = transform.localPosition;
        }

        IEnumerator ElevatorActivating()
        {
            lerpTime = 0f;
            Vector2 initial = transform.localPosition;
            isActivating = true;
            if (isCameraZoom) virtualCamera.Follow = transform;
            while (true)
            {
                lerpTime += Time.deltaTime;
                float t = lerpTime / 1.5f;
                if (lerpTime < 1.5f)
                {
                    transform.localPosition = Vector2.Lerp(initial, targetPosition, t);
                    if (isActivating)
                    {
                        if (!IsActivated())
                        {
                            transform.localPosition = Vector2.Lerp(curPosition, initial, t);
                        }
                    }
                }
                else
                {
                    //transform.localPosition = targetPosition;
                    isActivating = false;
                    if (IsActivated()) isEngaged = true;
                    virtualCamera.Follow = levelManager.player.transform;
                    StopCoroutine(cor);
                }
                yield return null;
            }
        }
    }
}

