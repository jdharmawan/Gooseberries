using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIDisplay : MonoBehaviour
{
    public PlayerController player;
    public KnightController knight;

    [SerializeField] Image knightIcon;

    [SerializeField] Transform health;
    [SerializeField] Transform arrows;

    [SerializeField] TextMeshProUGUI arrowPool;

    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite filledHeart;
    [SerializeField] Sprite emptyArrow;
    [SerializeField] Sprite filledArrow;
    [SerializeField] Sprite angryKnight;
    [SerializeField] Sprite idleKnight;

    private void Update()
    {
        UpdateHealth();
        UpdateArrows();
        UpdateKnight();
        arrowPool.text = knight.arrowStock.Count.ToString();
    }

    void UpdateHealth()
    {
        for (int i = 0; i < player.maxHP; i++)
        {
            health.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < health.childCount; i++)
        {
            health.GetChild(i).GetComponent<Image>().sprite = emptyHeart;
        }
        for (int i = 0; i < player.currHP; i++)
        {
            health.GetChild(i).GetComponent<Image>().sprite = filledHeart;
        }

    }

    void UpdateArrows()
    {
        for (int i = 0; i < player.maxArrows; i++)
        {
            arrows.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < arrows.childCount; i++)
        {
            arrows.GetChild(i).GetComponent<Image>().sprite = emptyArrow;
        }
        for (int i = 0; i < player.currArrows; i++)
        {
            arrows.GetChild(i).GetComponent<Image>().sprite = filledArrow;
        }
    }

    void UpdateKnight()
    {
        if (knight.knightState == KnightController.KnightState.Platform)
            knightIcon.sprite = angryKnight;
        else
            knightIcon.sprite = idleKnight;
    }
}
