using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Level : MonoBehaviour
{
    public static bool isGamePaused = false;

    [Header("Interactables")]
    [SerializeField] List<GameObject> bonfires;

    [Header("UI Overlay")]
    [SerializeField] GameObject upgradingUi;

    private void Start()
    {
        Initialise_Interactables();
    }

    void Initialise_Interactables()
    {
        for (int i = 0; i < bonfires.Count; i++)
        {
            bonfires[i].GetComponent<Interactables.BonfireHandler>().levelManager = this;
        }
    }

    public void TriggerUpgrade()
    {
        upgradingUi.SetActive(true);
    }

    public void B_ProceedLevel()
    {
        upgradingUi.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}
