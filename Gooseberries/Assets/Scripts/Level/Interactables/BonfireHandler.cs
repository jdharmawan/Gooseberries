using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider2D))]
    public class BonfireHandler : MonoBehaviour
    {
        Collider2D bonfireCollider;

        [HideInInspector] public GameManager_Level levelManager;

        bool isActive = false;

        //Counters
        float lerpTime = 0f;

        private void Start()
        {
            bonfireCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && isActive == false)
            {
                levelManager.TriggerUpgrade();
                UpdateLatestCheckpoint();
                Time.timeScale = 0f;
                GameManager_Level.isGamePaused = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                
            }
        }

        #region Bonfire Functions
        void DisplayEnemyInformation()
        {

        }

        void UpdateLatestCheckpoint()
        {
            isActive = true;
        }

        #endregion

        IEnumerator Lerp (float lerpTo, float duration)
        {
            lerpTime = 0f;
            while (true)
            {
                float t = lerpTime / duration;
                if (lerpTime < duration)
                {

                }
                else
                {

                }
            }
        }
    }
}

