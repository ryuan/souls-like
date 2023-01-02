using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponPickUp : Interactable
    {
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;
        UIManager uIManager;

        public WeaponItem weapon;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            uIManager = FindObjectOfType<UIManager>();
        }

        public override void Interact()
        {
            base.Interact();
            PickUpItem();
        }

        private void PickUpItem()
        {
            playerLocomotion.rb.velocity = Vector3.zero;    // stops player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Pick_Up_Item", true);
            playerInventory.weaponsInventory.Add(weapon);

            // Update and show the Item Pop Up UI
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);

            // Update the Weapon Inventory Window here in case it's open while picking up weapon
            uIManager.UpdateWeaponInventoryUI();

            Destroy(gameObject);
        }
    }
}
