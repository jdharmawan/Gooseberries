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
        [HideInInspector] public bool isMoving = false;

        bool isEngaged = false;

        [SerializeField] Vector2 targetPosition;
        [SerializeField] List<LeverHandler> leversToActivate;
        [SerializeField] List<PressurePlateHandler> pressurePlateToActivate;

        float lerpTime = 0f;
        Vector2 curPosition;

        public bool isCameraZoom = true;
        public bool isOneWay = false;

        Vector2 initial;

        enum ElevatorState
        {
            Ascending,
            Descending
        }

        private void Start()
        {
            initial = transform.localPosition;
            Initialise_Activators();
        }

        private void Update()
        {
            if (!isMoving)
            {
                if (IsActivated() && !isEngaged)
                    cor = StartCoroutine(ElevatorActivating());
                if (!isOneWay)
                {
                    if (!IsActivated() && isEngaged)
                        cor = StartCoroutine(ElevatorDeactivating());
                }
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
            isMoving = true;
            if (isCameraZoom) virtualCamera.Follow = transform;
            while (true)
            {
                lerpTime += Time.deltaTime;
                float t = lerpTime / 1.5f;
                if (lerpTime < 1.5f)
                {
                    transform.localPosition = Vector2.Lerp(initial, targetPosition, t);
                    if (isMoving)
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
                    isMoving = false;
                    if (IsActivated()) isEngaged = true;
                    virtualCamera.Follow = levelManager.player.transform;
                    StopCoroutine(cor);
                }
                yield return null;
            }
        }

        IEnumerator ElevatorDeactivating()
        {
            lerpTime = 0f;
            isMoving = true;
            if (isCameraZoom) virtualCamera.Follow = transform;
            while (true)
            {
                lerpTime += Time.deltaTime;
                float t = lerpTime / 1.5f;
                if (lerpTime < 1.5f)
                {
                    transform.localPosition = Vector2.Lerp(targetPosition, initial, t);
                    if (isMoving)
                    {
                        if (IsActivated())
                        {
                            transform.localPosition = Vector2.Lerp(curPosition, initial, t);
                        }
                    }
                }
                else
                {
                    isMoving = false;
                    if (!IsActivated()) isEngaged = false;
                    virtualCamera.Follow = levelManager.player.transform;
                    StopCoroutine(cor);
                }
                yield return null;
            }
        }
    }
}

