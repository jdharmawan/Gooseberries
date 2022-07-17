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
        [SerializeField] List<EnemySpawnPoint> spawnPoints;

        public List<GameObject> spawnedEnemies = new List<GameObject>();

        private PlayerController player;

        private void Start()
        {
            bonfireCollider = GetComponent<Collider2D>();
            SetBlockerActive(false);
            player = FindObjectOfType<PlayerController>();
            isVisited = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && isActive == false && bonfireIndex >= 0)
            {
                if(isVisited == false)
                {
                    isVisited = true;
                    Debug.Log("activeate bon fire");
                    //levelManager.checkPoint.savedPlayer = player.GetPlayerSavedData();
                }
                levelManager.TriggerUpgrade();
                levelManager.ActivateBonfireZone(this);
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
            spawnedEnemies = new List<GameObject>();
            //Debug.Log(transform.name);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                //Debug.Log(enemies[i].name, enemies[i].gameObject);
                //enemies[i].SetActive(true);
                spawnedEnemies.Add(Instantiate(spawnPoints[i].enemyPrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation));
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

