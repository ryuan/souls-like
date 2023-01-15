using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponPickUp : Interactable
    {
        PlayerInteractions playerInteractions;
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        PlayerAnimatorManager animatorManager;
        UIManager uIManager;

        public WeaponItem weapon;



        private void Awake()
        {
            playerInteractions = FindObjectOfType<PlayerInteractions>();
            playerInventory = playerInteractions.GetComponent<PlayerInventory>();
            playerLocomotion = playerInteractions.GetComponent<PlayerLocomotion>();
            animatorManager = playerInteractions.GetComponentInChildren<PlayerAnimatorManager>();
            uIManager = FindObjectOfType<UIManager>();
        }

        public override void Interact()
        {
            base.Interact();
            StartCoroutine(PickUpItem());
        }

        IEnumerator PickUpItem()
        {
            yield return StartCoroutine(playerLocomotion.SlerpFunction(playerInteractions.transform.position, transform.position));

            animatorManager.PlayTargetAnimation("Pick_Up_Item", true);

            yield return new WaitForSeconds(1);

            playerInventory.weaponsInventory.Add(weapon);

            // Update and show the Item Pop Up UI
            playerInteractions.itemPickUpAlertUIGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerInteractions.itemPickUpAlertUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerInteractions.itemPickUpAlertUIGameObject.SetActive(true);

            // Update the Weapon Inventory Window here in case it's open while picking up weapon
            uIManager.UpdateWeaponInventoryUI();

            Destroy(gameObject);
        }
    }
}
