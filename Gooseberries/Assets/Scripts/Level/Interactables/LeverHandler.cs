using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LeverHandler : MonoBehaviour
    {

        [HideInInspector] public GameManager_Level levelManager;

        [Header("UI Elements")]
        [SerializeField] GameObject inputPromptTag;
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite inactiveSprite;

        [HideInInspector] public bool isActive = false;
        bool isPlayerWithinCollider = false;

        private void Start()
        {
            
        }

        private void Update()
        {
            GetPlayerInput();
            if (isPlayerWithinCollider) inputPromptTag.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                inputPromptTag.SetActive(true);
                isPlayerWithinCollider = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                inputPromptTag.SetActive(false);
                isPlayerWithinCollider = false;
            }
        }

        void GetPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isPlayerWithinCollider && !isActive)
                {
                    isActive = true;
                    inputPromptTag.SetActive(false);
                    GetComponent<SpriteRenderer>().sprite = activeSprite;
                }
                else if (isPlayerWithinCollider && isActive)
                {
                    isActive = false;
                    inputPromptTag.SetActive(false);
                    GetComponent<SpriteRenderer>().sprite = inactiveSprite;
                }
            }
        }
    }
}


