using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyEffectPlayer
{
    public void ExplodedOnPlayer(int dmg, float stunDuration);
}
