using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class CharacterLocomotion : MonoBehaviour
    {
        public bool IsWithinViewableAngle(Vector3 targetPosition, float minAngle, float maxAngle)
        {
            Vector3 targetDir = targetPosition - transform.position;
            float viewableAngle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);

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
