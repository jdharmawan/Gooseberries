using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradingUIDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerSkillPoints;

    [Header("Upgrades")]
    [SerializeField] TextMeshProUGUI playerVitalityAmt;
    [SerializeField] TextMeshProUGUI playerQuiverAmt;
    [SerializeField] TextMeshProUGUI playerMovementSpeed;

    [Header("Projected")]
    [SerializeField] TextMeshProUGUI projVitalityAmt;
    [SerializeField] TextMeshProUGUI projQuiverAmt;
    [SerializeField] TextMeshProUGUI projMovementSpeed;

    public void DisplayUpgrades(PlayerController player)
    {
        playerSkillPoints.text = player.skillPoints.ToString();
        playerVitalityAmt.text = $"x{player.hp}";
        playerQuiverAmt.text = $"x{player.arrows}";
        playerMovementSpeed.text = $"x{player.moveSpeed}";
        projVitalityAmt.text = $"x{player.hp + 1}";
        projQuiverAmt.text = $"x{player.arrows + 1}";
        projMovementSpeed.text = $"x{player.moveSpeed + 0.7f}";
    }
}
