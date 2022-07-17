using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReceiveExplosion
{
    public void ExplodedOnPlayer(int dmg, float shieldDmg);
}
