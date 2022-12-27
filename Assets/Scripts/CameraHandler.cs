using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class CameraHandler : MonoBehaviour
    {
        public static CameraHandler singleton;

        InputHandler inputHandler;
        PlayerManager playerManager;
        Transform playerTransform;
        Transform mainCameraTransform;
        Transform cameraPivotTransform;

        [Header("Camera Movement Attributes")]
        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
        public float minPivot = -35;
        public float maxPivot = 35;

        float lookAngle;
        float pivotAngle;
        Vector3 followVelocity = Vector3.zero;

        [Header("Collision Controls")]
        public float detectionSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minCollisionOffset = 0.2f;
        public LayerMask ignoreLayers;

        float defaultMainCamLocalPosZ;
        float targetMainCamLocalPosZ;
        Vector3 lerpingMainCamPos;
        
        [Header("Target Locked Camera")]
        public float maxLockOnDistance = 30f;
        public float lockedPivotPosYLift = 0.6f;
        public CharacterManager nearestLockOnTarget;
        public CharacterManager currentLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        Vector3 lockedPivotPos;
        Vector3 unlockedPivotPos;
        Vector3 pivotVelocity = Vector3.zero;



        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            playerTransform = FindObjectOfType<PlayerManager>().transform;
            mainCameraTransform = Camera.main.transform;
            cameraPivotTransform = this.transform.GetChild(0);

            defaultMainCamLocalPosZ = mainCameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            unlockedPivotPos = cameraPivotTransform.position;
            lockedPivotPos = unlockedPivotPos + new Vector3(0, lockedPivotPosYLift);
            singleton = this;
        }

        public void FollowTarget(float delta)
        {
            Vector3 followPosition = Vector3.SmoothDamp(
                transform.position, playerTransform.position, ref followVelocity, delta / followSpeed
                );

            transform.position = followPosition;
            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                transform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;
                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                Vector3 dir = currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollisions(float delta)
        {
            targetMainCamLocalPosZ = defaultMainCamLocalPosZ;
            RaycastHit hit;
            Vector3 direction = mainCameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(
                cameraPivotTransform.position, detectionSphereRadius, direction, out hit, Mathf.Abs(targetMainCamLocalPosZ), ignoreLayers
                ))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetMainCamLocalPosZ = -(dis - cameraCollisionOffset);

                if (Mathf.Abs(targetMainCamLocalPosZ) < minCollisionOffset)
                {
                    targetMainCamLocalPosZ = -minCollisionOffset;
                }
            }

            lerpingMainCamPos.z = Mathf.Lerp(mainCameraTransform.localPosition.z, targetMainCamLocalPosZ, delta / 0.1f);
            mainCameraTransform.localPosition = lerpingMainCamPos;
        }

        public void HandleLockOn()
        {
            Collider[] colliders = Physics.OverlapSphere(playerTransform.position, 26);
            float shortestDistance = Mathf.Infinity;
            float shortestDistOfLeftTarget = -Mathf.Infinity;
            float shortestDistOfRightTarget = Mathf.Infinity;

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - playerTransform.position;
                    float distanceFromTarget = Vector3.Distance(playerTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, mainCameraTransform.forward);

                    if (character.transform.root != playerTransform.transform.root
                        && viewableAngle > -50
                        && viewableAngle < 50
                        && distanceFromTarget <= maxLockOnDistance)
                    {
                        RaycastHit hit;

                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position, Color.red, 2f);
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.Log(hit.transform.gameObject.name);

                            if (hit.transform.gameObject.GetComponent<CharacterManager>() != null)
                            {
                                availableTargets.Add(character);
                            }
                            else
                            {
                                // Pass
                                Debug.Log("Linecast hit some non-enemy collider");
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(playerTransform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (inputHandler.lockOnFlag)
                {
                    //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0 && distanceFromLeftTarget > shortestDistOfLeftTarget && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k];
                    }
                    else if (relativeEnemyPosition.x >= 0 && distanceFromRightTarget < shortestDistOfRightTarget && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k];
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, lockedPivotPos, ref pivotVelocity, Time.deltaTime
                    );
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, unlockedPivotPos, ref pivotVelocity, Time.deltaTime
                    );
            }
        }
    }
}
