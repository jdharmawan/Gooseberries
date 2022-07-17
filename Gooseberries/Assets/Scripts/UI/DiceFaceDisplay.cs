using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceFaceDisplay : MonoBehaviour
{
    public int diceFace;
    int diceAmt = 0;
    [SerializeField] TextMeshProUGUI diceFaceQuantity;

    public void UpdateDiceFaceQuantity(int qty)
    {
        diceAmt = qty;
        diceFaceQuantity.text = qty.ToString();
    }
}
