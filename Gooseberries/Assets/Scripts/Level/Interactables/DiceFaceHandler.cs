using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Interactables
{
    public class DiceFaceHandler : MonoBehaviour
    {
        public int diceFaceValue;

        [HideInInspector] public GameManager_Level levelManager;
        [SerializeField] TextMeshProUGUI value;

        private void Start()
        {
            value.text = diceFaceValue.ToString();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                if (levelManager == null)
                    levelManager = FindObjectOfType<GameManager_Level>();
                levelManager.diceFacesValue.Add(diceFaceValue);
                levelManager.UpdateDiceCollection();
                Destroy(this.gameObject);
                FindObjectOfType<ZoneCounter>().DiceCollected();
            }
        }
    }
}

