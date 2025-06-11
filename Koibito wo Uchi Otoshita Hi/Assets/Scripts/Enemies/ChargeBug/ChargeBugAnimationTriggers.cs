using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBugAnimationTriggers : MonoBehaviour
{
    private ChargeBug enemy => GetComponentInParent<ChargeBug>();
    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }
}
