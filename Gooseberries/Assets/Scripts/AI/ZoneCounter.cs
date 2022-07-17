using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneCounter : MonoBehaviour
{
    public int numOfZoneEnemykilled = 0;
    public int totalDiceCollected = 0;
    public int curZoneIndex;
    private UnityAction zoneClear;
    public int zoneEnemyNumber { get; private set; }

    private void Awake()
    {
        numOfZoneEnemykilled = 0;
        totalDiceCollected = 0;
        curZoneIndex = 0;
    }

    public void EnemyDied()
    {
        numOfZoneEnemykilled += 1;
        CheckIfConditionClear();
    }

    public void SetZoneClearEvent(UnityAction _zoneClear)
    {
        zoneClear = _zoneClear;
    }
    
    public void SetZoneEnemyNumber(int _zoneEnemyNumber)
    {
        numOfZoneEnemykilled = 0;
        zoneEnemyNumber = _zoneEnemyNumber;
    }
    public void SetCurZoneIndex(int _curZoneIndex)
    {
        curZoneIndex = _curZoneIndex;
    }
    public void DiceCollected()
    {
        totalDiceCollected += 1;
        CheckIfConditionClear();
    }

    public void CheckIfConditionClear()
    {
        Debug.Log("COUNTER, Enemies:  " + numOfZoneEnemykilled + " Targe enemies count : " + zoneEnemyNumber);
        if (numOfZoneEnemykilled >= zoneEnemyNumber && zoneClear != null
            && totalDiceCollected>= (curZoneIndex+1)*2)
        {
            Debug.Log("COUNTER, Total dice collected:  " + totalDiceCollected + "Curzoneindex: " + (curZoneIndex + 1) + ", total: " + (curZoneIndex + 1) * 2);
            zoneClear.Invoke();
        }
    }

    public string GetConditionMessage()
    {
        string message = "";
        if(numOfZoneEnemykilled < zoneEnemyNumber && zoneClear != null)
        {
            message += "<color=#FF2E00>" + (zoneEnemyNumber - numOfZoneEnemykilled) + "</color> Enemies Left to be killed\n";
        }
        if(totalDiceCollected < (curZoneIndex + 1) * 2)
        {
            message += "Collect <color=#38E500>" + ((curZoneIndex + 1) * 2 - totalDiceCollected) + "</color> more dice face";
        }
        return message;
    }
}
