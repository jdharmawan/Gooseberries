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

        float lerpTime = 0f;

        private void Update()
        {
            if (!isActivating)
            {
                if (IsActivated() && !isEngaged)
                    cor = StartCoroutine(ElevatorActivating());
            }
        }

        bool IsActivated()
        {
            for (int i = 0; i < leversToActivate.Count; i++)
            {
                if (!leversToActivate[i].isActive)
                    return false;
            }
            return true;
        }

        IEnumerator ElevatorActivating()
        {
            lerpTime = 0f;
            Vector2 initial = transform.localPosition;
            isActivating = true;
            virtualCamera.Follow = transform;
            while (true)
            {
                lerpTime += Time.deltaTime;
                float t = lerpTime / 1.5f;
                if (lerpTime < 1.5f)
                {
                    transform.localPosition = Vector2.Lerp(initial, targetPosition, t);
                }
                else
                {
                    transform.localPosition = targetPosition;
                    isActivating = false;
                    isEngaged = true;
                    virtualCamera.Follow = levelManager.player.transform;
                    StopCoroutine(cor);
                }
                yield return null;
            }
        }
    }
}

