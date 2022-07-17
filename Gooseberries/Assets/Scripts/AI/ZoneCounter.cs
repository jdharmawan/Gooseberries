using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ZoneCounter
{
    public static int numOfZoneEnemykilled = 0;
    public static int totalDiceCollected = 0;
    public static int curZoneIndex;
    private static UnityAction zoneClear;
    public static int zoneEnemyNumber { get; private set; }
    public static void EnemyDied()
    {
        numOfZoneEnemykilled += 1;
        CheckIfConditionClear();
    }

    public static void SetZoneClearEvent(UnityAction _zoneClear)
    {
        zoneClear = _zoneClear;
    }
    
    public static void SetZoneEnemyNumber(int _zoneEnemyNumber)
    {
        numOfZoneEnemykilled = 0;
        zoneEnemyNumber = _zoneEnemyNumber;
    }
    public static void SetCurZoneIndex(int _curZoneIndex)
    {
        curZoneIndex = _curZoneIndex;
    }
    public static void DiceCollected()
    {
        totalDiceCollected += 1;
        CheckIfConditionClear();
    }

    public static void CheckIfConditionClear()
    {
        Debug.Log("COUNTER, Enemies:  " + numOfZoneEnemykilled + " Targe enemies count : " + zoneEnemyNumber);
        if (numOfZoneEnemykilled >= zoneEnemyNumber && zoneClear != null
            && totalDiceCollected>= (curZoneIndex+1)*2)
        {
            Debug.Log("COUNTER, Total dice collected:  " + totalDiceCollected + "Curzoneindex: " + (curZoneIndex + 1) + ", total: " + (curZoneIndex + 1) * 2);
            zoneClear.Invoke();
        }
    }
}
