using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Level : MonoBehaviour
{
    
    public static bool isGamePaused = false;

    [SerializeField] Cinemachine.CinemachineVirtualCamera virtualCamera;
    public Transform player;

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

    private void Start()
    {
        Initialise_Interactables();
        Initialise_UIComponents();
    }

    private void Update()
    {
        
    }

    void Initialise_Interactables()
    {
        for (int i = 0; i < bonfires.Count; i++)
        {
            bonfires[i].GetComponent<Interactables.BonfireHandler>().levelManager = this;
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

    public void TriggerUpgrade()
    {
        upgradingUi.SetActive(true);
    }

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

    public void B_ProceedLevel()
    {
        upgradingUi.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}
