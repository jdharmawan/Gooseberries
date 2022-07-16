using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySuicide
{
    public void ExplodedOnPlayer(int dmg, float stunDuration);
}
