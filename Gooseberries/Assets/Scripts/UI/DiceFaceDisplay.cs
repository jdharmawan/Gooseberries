using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceFaceDisplay : MonoBehaviour
{
    public int diceFace;
    int diceAmt = 0;
    [SerializeField] Color inactiveColor;
    [SerializeField] TextMeshProUGUI diceFaceNumber;
    [SerializeField] TextMeshProUGUI diceFaceQuantity;

    public void UpdateDiceFaceQuantity(int qty)
    {
        diceAmt = qty;
        diceFaceNumber.text = diceFace.ToString();
        diceFaceQuantity.text = qty.ToString();
        if (diceAmt == 0)
            GetComponent<Image>().color = inactiveColor;
        else
            GetComponent<Image>().color = Color.white;
    }
}
