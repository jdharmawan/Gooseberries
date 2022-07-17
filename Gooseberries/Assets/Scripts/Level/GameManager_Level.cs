using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Checkpoint
{
    public SavedPlayer savedPlayer;
    public int enemyRolled;
    public int skillRolled;
}

public class GameManager_Level : MonoBehaviour
{
    
    public static bool isGamePaused = false;
    public static bool isPlayerLocked = false;

    public Interactables.BonfireHandler curBonFire = null;

    public Checkpoint checkPoint = new Checkpoint();

    [SerializeField] Cinemachine.CinemachineVirtualCamera virtualCamera;
    public PlayerController player;

    [Header("Interactables")]
    [SerializeField] List<GameObject> bonfires;
    [SerializeField] List<GameObject> levers;
    [SerializeField] List<GameObject> elevators;
    [SerializeField] List<GameObject> diceFaces;

    [Header("UI Overlay")]
    [SerializeField] GameObject upgradingUi;

    [Header("Main UI Components")]
    [SerializeField] Transform diceFacesDisplayGroup;

    [HideInInspector] public List<int> diceFacesValue = new List<int>();
    [HideInInspector] public int enemyRolled, skillRolled;

    private void Start()
    {
        Initialise_Interactables();
        Initialise_UIComponents();
    }

    private void Update()
    {
        
    }

    #region Initialise
    void Initialise_Interactables()
    {
        for (int i = 0; i < bonfires.Count; i++)
        {
            Interactables.BonfireHandler bonfireHandler = bonfires[i].GetComponent<Interactables.BonfireHandler>();
            bonfireHandler.bonfireIndex = i;
            bonfireHandler.levelManager = this;
        }
        for (int i = 0; i < levers.Count; i++)
        {
            levers[i].GetComponent<Interactables.LeverHandler>().levelManager = this;
        }
        for (int i = 0; i < elevators.Count; i++)
        {
            Interactables.ElevatorHandler elevatorHandler = elevators[i].GetComponent<Interactables.ElevatorHandler>();
            elevatorHandler.virtualCamera = virtualCamera;
            elevatorHandler.levelManager = this;
        }
        for (int i = 0; i < diceFaces.Count; i++)
        {
            diceFaces[i].GetComponent<Interactables.DiceFaceHandler>().levelManager = this;
        }
    }

    void Initialise_UIComponents()
    {
        UpdateDiceCollection();
    }
    #endregion

    #region Bonfire Functions
    public void TriggerUpgrade()
    {
        UpgradingUIDisplay display = upgradingUi.GetComponent<UpgradingUIDisplay>();
        upgradingUi.SetActive(true);
        display.levelManager = this;
        display.BeginUpgradeSession(player, diceFacesValue);
    }

    [ContextMenu("Respawn")]
    public void TriggerRespawn()
    {
        player.SetPlayerSavedData(checkPoint.savedPlayer);
        UpgradingUIDisplay display = upgradingUi.GetComponent<UpgradingUIDisplay>();
        upgradingUi.SetActive(true);
        display.levelManager = this;
        display.RespawnUpgrateSession();
        player.transform.position = curBonFire.transform.position;
        player.knight.transform.position = curBonFire.transform.position;
        for (int i = 0; i < curBonFire.spawnedEnemies.Count; i++)
        {
            if (curBonFire.spawnedEnemies[i] != null)
            {
                Destroy(curBonFire.spawnedEnemies[i]);
            }
        }
        curBonFire.spawnedEnemies.Clear();
    }

    public void ActivateBonfireZone(Interactables.BonfireHandler _curBonFire)
    {
        curBonFire = _curBonFire;
        bonfires[_curBonFire.bonfireIndex].GetComponent<Interactables.BonfireHandler>().SetBlockerActive(true);
        Debug.Log("INDEX: " + _curBonFire.bonfireIndex + 1);
        Debug.Log("INDEX: " + (_curBonFire.bonfireIndex + 1 <= bonfires.Count));
        if (_curBonFire.bonfireIndex + 1 < bonfires.Count)
        {
            bonfires[_curBonFire.bonfireIndex + 1].GetComponent<Interactables.BonfireHandler>().SetBlockerActive(true);
        }
    }
    void SpawnEnemy()
    {
        curBonFire.SpawnEnemies(enemyRolled);
        //bonfires[lastBonfireIndex].GetComponent<Interactables.BonfireHandler>().SpawnEnemies(enemyRolled);
    }

    public void CurrentBonfireCleared()
    {
        if (curBonFire.bonfireIndex + 1 <= bonfires.Count)
        {
            bonfires[curBonFire.bonfireIndex + 1].GetComponent<Interactables.BonfireHandler>().SetBlockerActive(false);
        }
    }


    #endregion

    #region Dice-Related Functions
    int GetNumberOfDiceOfFace(int faceNumber)
    {
        int number = 0;
        for (int i = 0; i < diceFacesValue.Count; i++)
        {
            if (faceNumber == diceFacesValue[i])
                number++;
        }
        return number;
    }

    public void UpdateDiceCollection()
    {
        for (int i = 0; i < diceFacesDisplayGroup.childCount; i++)
        {
            DiceFaceDisplay diceDisplay = diceFacesDisplayGroup.GetChild(i).GetComponent<DiceFaceDisplay>();
            diceDisplay.UpdateDiceFaceQuantity(GetNumberOfDiceOfFace(i + 1));
        }
    }
    #endregion

    public void B_ProceedLevel()
    {
        upgradingUi.SetActive(false);
        isPlayerLocked = false;
        SpawnEnemy();
    }
}
