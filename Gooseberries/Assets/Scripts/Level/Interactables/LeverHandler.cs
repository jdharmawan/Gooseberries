using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider2D))]
    public class LeverHandler : MonoBehaviour
    {
        Collider2D leverCollider;

        [HideInInspector] public GameManager_Level levelManager;

        [Header("UI Elements")]
        [SerializeField] GameObject inputPromptTag;

        [HideInInspector] public bool isActive = false;
        bool isPlayerWithinCollider = false;

        private void Start()
        {
            leverCollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            GetPlayerInput();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !isActive)
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
                if (isPlayerWithinCollider)
                {
                    isActive = true;
                    inputPromptTag.SetActive(false);
                }
            }
        }
    }
}


