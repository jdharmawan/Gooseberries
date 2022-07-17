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
        public bool isVisited { get; private set; }
        [SerializeField] public GameObject beforeCollider;

        bool isActive = false;

        //Counters
        float lerpTime = 0f;

        [SerializeField] List<GameObject> enemies;

        private PlayerController player;

        private void Start()
        {
            bonfireCollider = GetComponent<Collider2D>();
            SetBlockerActive(false);
            player = FindObjectOfType<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && isActive == false && bonfireIndex >= 0)
            {
                Debug.Log("activeate bon fire");
                levelManager.ActivateBonfireZone(this);
                levelManager.TriggerUpgrade();
                UpdateLatestCheckpoint();
                GameManager_Level.isPlayerLocked = true;
            }
            //if (bonfireIndex == 0) isActive = true;
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
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Debug.Log(enemies[i].name, enemies[i].gameObject);
                enemies[i].SetActive(true);
            }
            ZoneEnemyCounter.SetZoneEnemyNumber(levelManager.CurrentBonfireCleared,numberOfEnemies);
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

        public void SetBlockerActive(bool isActive)
        {
            beforeCollider.SetActive(isActive);
        }
    }
}

