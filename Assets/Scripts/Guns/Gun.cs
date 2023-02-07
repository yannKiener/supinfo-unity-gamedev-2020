using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Gun
{
    void StartShooting(Vector3 targetPosition, bool isFriendly);
    void StopShooting();
}
