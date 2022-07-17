using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.activeSelf)
            GameManager_Level.isPlayerLocked = true;
        else
            GameManager_Level.isPlayerLocked = false;

    }

    private void OnEnable()
    {
        GameManager_Level.isPlayerLocked = true;
    }

    private void OnDisable()
    {
        GameManager_Level.isPlayerLocked = false;
    }
}
