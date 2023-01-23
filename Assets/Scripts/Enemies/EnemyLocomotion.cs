using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RY
{
    public class EnemyLocomotion : CharacterLocomotion
    {
        EnemyManager enemyManager;

        Vector3 targetPosition;

        [Header("AI Detection Settings")]
        [SerializeField]
        float _detectionFOVAngle = 100;
        public LayerMask detectionLayers;

        [Header("AI Movement Attributes")]
        public float moveSpeedAnimVerticalFloat = 0.85f;
        public float rotationSpeed = 15;
        public float maxAttackRange = 1.5f;

        [Header("Grounding Detection")]
        public float groundingSphereCastHeight = 0.5f;
        public float groundingSphereCastForwardDistance = 0.3f;
        public float groundingSphereCastRadius = 0.1f;
        public float minDistanceNeededToBeginFall = 1f;
        public LayerMask ignoreForGroundCheck;

        public float MinDetectionAngle { get { return -(_detectionFOVAngle / 2); } }
        public float MaxDetectionAngle { get { return _detectionFOVAngle / 2; } }
        public float DistanceFromTarget { get { return Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position); } }



        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            detectionLayers = (1 << 9);
            ignoreForGroundCheck = ~(1 << 7 | 1 << 8 | 1 << 10 | 1 << 11 | 1 << 12 | 1 << 13);
        }

        public void HandleGrounding()
        {
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += groundingSphereCastHeight;

            Vector3 dir = transform.forward;
            dir.Normalize();
            origin = origin + dir * groundingSphereCastForwardDistance;

            targetPosition = transform.position;

            //Debug.DrawRay(origin, Vector3.down * minDistanceNeededToBeginFall, Color.red, 0.1f, false);
            //if (Physics.Raycast(origin, Vector3.down, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
            if (Physics.SphereCast(origin, groundingSphereCastRadius, Vector3.down, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                Vector3 hitPoint = hit.point;
                targetPosition.y = hitPoint.y;

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
        }

        public void HandleRotate()
        {
            if (enemyManager.canRotate)
            {
                Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;
                targetDir.y = 0;
                targetDir.Normalize();

                if (targetDir == Vector3.zero)
                {
                    targetDir = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(targetDir);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation, targetRotation, rotationSpeed * Time.deltaTime
                    );
            }
        }
    }
}

