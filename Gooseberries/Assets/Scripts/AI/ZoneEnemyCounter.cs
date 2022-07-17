using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ZoneEnemyCounter
{
    public static int numOfZoneEnemykilled = 0;
    private static UnityAction allZoneEnemiesKilled;
    public static int zoneEnemyNumber { get; private set; }
    public static void EnemyDied()
    {
        numOfZoneEnemykilled += 1;
        if(numOfZoneEnemykilled>= zoneEnemyNumber && allZoneEnemiesKilled!=null)
        {
            allZoneEnemiesKilled.Invoke();
        }
    }

    public static void SetZoneEnemyNumber(UnityAction _allZoneEnemiesKilled, int _zoneEnemyNumber)
    {
        numOfZoneEnemykilled = 0;
        zoneEnemyNumber = _zoneEnemyNumber;
        allZoneEnemiesKilled = _allZoneEnemiesKilled;
    }
}
