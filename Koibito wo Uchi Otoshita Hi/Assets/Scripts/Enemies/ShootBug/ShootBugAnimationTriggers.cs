using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBugAnimationTriggers : MonoBehaviour
{
    private ShootBug enemy => GetComponentInParent<ShootBug>();
    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }
}
