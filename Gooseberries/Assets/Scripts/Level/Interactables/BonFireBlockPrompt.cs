using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonFireBlockPrompt : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.gameObject.SetActive(true);
            tmp.text = ZoneCounter.GetConditionMessage();
        }
        else
        {
            tmp.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.gameObject.SetActive(false);
        }
    }
}
