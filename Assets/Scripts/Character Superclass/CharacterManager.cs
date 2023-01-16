using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        [SerializeField]
        protected Collider mainCollider;
        [SerializeField]
        protected Collider collisionBlockerCollider;
        [SerializeField]
        protected Collider backstabCollider;
        [SerializeField]
        protected Collider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isParrying;
    }
}
