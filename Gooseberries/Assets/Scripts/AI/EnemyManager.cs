using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    public static int numberOfEnemykilled = 0;
    public static void EnemyDied()
    {
        numberOfEnemykilled += 1;
    }

}
