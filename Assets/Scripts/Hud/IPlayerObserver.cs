using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerObserver
{
    void OnMeleeUnlocked();
    void OnRangedUnlocked();
}
