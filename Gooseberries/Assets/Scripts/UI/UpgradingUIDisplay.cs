using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradingUIDisplay : MonoBehaviour
{
    Coroutine cor;
    PlayerController player;

    float lerpTime = 0f;
    bool isEnemyRolled = false;
    bool isSkillRolled = false;

    [HideInInspector] public GameManager_Level levelManager;

    [SerializeField] TextMeshProUGUI playerSkillPoints;
    [SerializeField] GameObject raycastBlocker;

    [Header("Dices")]
    [SerializeField] GameObject enemyDice;
    [SerializeField] GameObject skillPointDice;
    [SerializeField] TextMeshProUGUI enemyDiceValueTmp;
    [SerializeField] TextMeshProUGUI skillDiceValueTmp;

    [Header("Upgrades")]
    [SerializeField] TextMeshProUGUI playerVitalityAmt;
    [SerializeField] TextMeshProUGUI playerQuiverAmt;
    [SerializeField] TextMeshProUGUI playerMovementSpeed;

    [Header("Projected")]
    [SerializeField] TextMeshProUGUI projVitalityAmt;
    [SerializeField] TextMeshProUGUI projQuiverAmt;
    [SerializeField] TextMeshProUGUI projMovementSpeed;

    public void BeginUpgradeSession(PlayerController _player, List<int> diceFacePool)
    {
        player = _player;
        cor = StartCoroutine(StartRolling(diceFacePool, player));
        DisplayUpgrades();
    }

    public void RespawnUpgrateSession()
    {
        DisplayUpgrades();
        StartCoroutine(StartRespawnReroll());
    }

    void DisplayUpgrades()
    {
        playerSkillPoints.text = player.skillPoints.ToString();
        playerVitalityAmt.text = $"x{player.maxHP}";
        playerQuiverAmt.text = $"x{player.maxArrows}";
        playerMovementSpeed.text = $"x{player.moveSpeed}";
        projVitalityAmt.text = $"x{player.maxHP + 1}";
        projQuiverAmt.text = $"x{player.maxArrows + 1}";
        projMovementSpeed.text = $"x{player.moveSpeed + 0.7f}";
    }

    IEnumerator StartRolling(List<int> diceFacePool, PlayerController player)
    {
        lerpTime = 0f;
        isEnemyRolled = false;
        isSkillRolled = false;
        levelManager.enemyRolled = 0;
        levelManager.skillRolled = 0;
        enemyDiceValueTmp.alpha = 0f;
        skillDiceValueTmp.alpha = 0f;
        raycastBlocker.SetActive(true);

        while (true)
        {
            lerpTime += Time.deltaTime; 
            float t = lerpTime;

            if (!isEnemyRolled)
            {
                if (lerpTime < 1f)
                {
                    enemyDice.transform.Rotate(0f, 0f, 1000 * Time.deltaTime);
                }
                else
                {
                    
                    enemyDice.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    levelManager.enemyRolled = Utility.DiceHandler.Roll(diceFacePool);
                    diceFacePool.Remove(levelManager.enemyRolled);
                    enemyDiceValueTmp.alpha = 1f;
                    enemyDiceValueTmp.text = levelManager.enemyRolled.ToString();
                    isEnemyRolled = true;
                    lerpTime = 0f;
                }
            }
            else if (!isSkillRolled)
            {
                if (lerpTime < 1f)
                {
                    skillPointDice.transform.Rotate(0f, 0f, 1000 * Time.deltaTime);
                }
                else
                {
                    skillPointDice.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    levelManager.skillRolled = Utility.DiceHandler.Roll(diceFacePool);
                    diceFacePool.Remove(levelManager.skillRolled);
                    player.skillPoints += levelManager.skillRolled;
                    skillDiceValueTmp.alpha = 1f;
                    skillDiceValueTmp.text = levelManager.skillRolled.ToString();
                    isSkillRolled = true;
                }
            }
            else if (isEnemyRolled && isSkillRolled)
            {
                raycastBlocker.SetActive(false);
                DisplayUpgrades();
                levelManager.UpdateDiceCollection();
                StopCoroutine(cor);
            }
            levelManager.checkPoint.enemyRolled = levelManager.enemyRolled;
            levelManager.checkPoint.skillRolled = levelManager.skillRolled;
            yield return null;
        }
    }

    IEnumerator StartRespawnReroll()
    {
        lerpTime = 0f;
        isEnemyRolled = false;
        isSkillRolled = false;
        levelManager.enemyRolled = 0;
        levelManager.skillRolled = 0;
        enemyDiceValueTmp.alpha = 0f;
        skillDiceValueTmp.alpha = 0f;
        raycastBlocker.SetActive(true);

        while (true)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime;

            if (!isEnemyRolled)
            {
                if (lerpTime < 1f)
                {
                    enemyDice.transform.Rotate(0f, 0f, 1000 * Time.deltaTime);
                }
                else
                {

                    enemyDice.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    levelManager.enemyRolled = levelManager.checkPoint.enemyRolled;
                    enemyDiceValueTmp.alpha = 1f;
                    enemyDiceValueTmp.text = levelManager.enemyRolled.ToString();
                    isEnemyRolled = true;
                    lerpTime = 0f;
                }
            }
            else if (!isSkillRolled)
            {
                if (lerpTime < 1f)
                {
                    skillPointDice.transform.Rotate(0f, 0f, 1000 * Time.deltaTime);
                }
                else
                {
                    skillPointDice.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    levelManager.skillRolled = levelManager.checkPoint.skillRolled;
                    player.skillPoints += levelManager.skillRolled;
                    skillDiceValueTmp.alpha = 1f;
                    skillDiceValueTmp.text = levelManager.skillRolled.ToString();
                    isSkillRolled = true;
                }
            }
            else if (isEnemyRolled && isSkillRolled)
            {
                raycastBlocker.SetActive(false);
                DisplayUpgrades();
                levelManager.UpdateDiceCollection();
                StopCoroutine(cor);
            }
            yield return null;
        }
    }

    #region Upgrade Buttons
    public void B_UpgradeVitality()
    {
        if (player.skillPoints >= player.vitalityLevel)
        {
            player.maxHP++;
            player.skillPoints -= player.vitalityLevel;
            player.vitalityLevel++;
            DisplayUpgrades();
        }
    }

    public void B_UpgradeQuiver()
    {
        if (player.skillPoints >= player.quiverLevel)
        {
            player.maxArrows++;
            player.skillPoints -= player.quiverLevel;
            player.quiverLevel++;
            DisplayUpgrades();
        }
    }

    public void B_UpgradeMovementSpeed()
    {
        if (player.skillPoints >= player.speedLevel)
        {
            player.moveSpeed += 0.7f;
            player.skillPoints -= player.speedLevel;
            player.speedLevel++;
            DisplayUpgrades();
        }
    }
    #endregion
}
