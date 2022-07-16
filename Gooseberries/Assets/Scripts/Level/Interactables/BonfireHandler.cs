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
        [HideInInspector] public int bonfireIndex;

        bool isActive = false;

        //Counters
        float lerpTime = 0f;

        private void Start()
        {
            bonfireCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && isActive == false && bonfireIndex != 0)
            {
                levelManager.TriggerUpgrade();
                UpdateLatestCheckpoint();
                Time.timeScale = 0f;
                GameManager_Level.isGamePaused = true;
            }
            if (bonfireIndex == 0) isActive = true;
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

        public void SpawnEnemies(int numberOfEnemies)
        {

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

