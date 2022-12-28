using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponPickUp : Interactable
    {
        UIManager uIManager;

        public WeaponItem weapon;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

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
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);

            uIManager.UpdateUI();

            Destroy(gameObject);
        }
    }
}
