using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Actions")]
    public class EnemyAttackAction : EnemyAction
    {
        public EnemyAttackAction comboAttackAction;

        public int attackChanceWeight = 3;
        public float recoveryTime = 2;

        private float _attackAngle = 70;

        public float minAttackDistance = 0;
        public float maxAttackDistance = 3;

        public float MinAttackAngle { get { return -(_attackAngle / 2); } }
        public float MaxAttackAngle { get { return _attackAngle / 2; } }
    }
}