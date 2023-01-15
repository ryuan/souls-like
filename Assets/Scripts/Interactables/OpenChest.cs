using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class OpenChest : Interactable
    {
        PlayerManager playerManager;

        [SerializeField]
        float openPosZOffset = 0.75f;



        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        public override void Interact()
        {
            // Make coroutine to slerp the movement
            Vector3 targetDir = transform.position - playerManager.transform.position;
            targetDir.y = 0;
            targetDir.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            playerManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 300 * Time.deltaTime);

            // Lock his transform to a certian point in front of chest
            // Open chest's lid and animate player
            // Spawn an item inside the chest that players can pickup
        }
    }
}

