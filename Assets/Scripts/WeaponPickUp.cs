using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rb.velocity = Vector3.zero;    // stops player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Pick_Up_Item", true);
            playerInventory.weaponsInventory.Add(weapon);
            Destroy(gameObject);
        }
    }
}
