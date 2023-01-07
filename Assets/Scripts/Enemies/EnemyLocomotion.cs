using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RY
{
    public class EnemyLocomotion : MonoBehaviour
    {
        EnemyManager enemyManager;

        [Header("AI Detection Settings")]
        [SerializeField]
        float _detectionFOVAngle = 100;
        public LayerMask detectionLayers;

        [Header("AI Movement Attributes")]
        public float moveSpeedAnimVerticalFloat = 0.85f;
        public float rotationSpeed = 15;
        public float maxAttackRange = 1.5f;

        [Header("Grounding Detection")]
        public float groundDetectionRayStartPoint = 0.5f;
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
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        public void HandleGrounding()
        {
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += groundDetectionRayStartPoint;

            Vector3 targetPosition = transform.position;

            Debug.DrawRay(origin, -Vector3.up * minDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                Vector3 hitPoint = hit.point;
                targetPosition.y = hitPoint.y;

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
        }

        public void HandleRotate()
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

        public bool IsWithinViewableAngle(Vector3 targetPosition, float minAngle, float maxAngle)
        {
            Vector3 targetDir = targetPosition - transform.position;
            float viewableAngle = Vector3.Angle(targetDir, transform.forward);

            // Probably need to fix this... Vector3.Angle always return smallest positive angle, so a negative minAngle is pointless
            if (viewableAngle >= minAngle && viewableAngle <= maxAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

