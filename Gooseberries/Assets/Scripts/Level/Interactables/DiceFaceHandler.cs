using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class DiceFaceHandler : MonoBehaviour
    {
        public int diceFaceValue;

        [HideInInspector] public GameManager_Level levelManager;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                if (levelManager == null)
                    levelManager = FindObjectOfType<GameManager_Level>();
                levelManager.diceFacesValue.Add(diceFaceValue);
                levelManager.UpdateDiceCollection();
                Destroy(this.gameObject);
                ZoneCounter.DiceCollected();
            }
        }
    }
}

