using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Actions")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float minAttackAngle = -35;
        public float maxAttackAngle = 35;

        public float minDistanceNeededToAttack = 0;
        public float maxDistanceNeededToAttack = 3;
    }
}