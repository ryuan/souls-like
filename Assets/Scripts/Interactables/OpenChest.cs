using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class OpenChest : Interactable
    {
        PlayerLocomotion playerLocomotion;
        PlayerAnimatorManager animatorManager;
        Animator anim;

        public GameObject itemSpawner;
        public WeaponItem weaponInChest;

        [SerializeField]
        float openPosZOffset = 0.75f;



        private void Awake()
        {
            playerLocomotion = FindObjectOfType<PlayerLocomotion>();
            animatorManager = playerLocomotion.GetComponentInChildren<PlayerAnimatorManager>();
            anim = GetComponent<Animator>();
        }

        public override void Interact()
        {
            base.Interact();
            StartCoroutine(OpenChestInteraction());
        }

        IEnumerator OpenChestInteraction()
        {
            // Slerp to position and look towards chest before opening
            Vector3 targetMovePos = transform.position + transform.forward * openPosZOffset;
            Vector3 targetLookPos = transform.position;
            yield return StartCoroutine(playerLocomotion.SlerpFunction(targetMovePos, targetLookPos));

            animatorManager.PlayTargetAnimation("Open_Chest", true);
            anim.Play("Lift_Chest_Lid");

            // Wait for chest opening animation to play a bit, then spawn Item Pick Up object
            yield return new WaitForSeconds(1);

            Instantiate(itemSpawner, transform);

            // Get the WeaponPickUp script of the Item Pick Up and assign its embedded weapon
            WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

            if (weaponPickUp != null)
            {
                weaponPickUp.weapon = weaponInChest;
            }

            // Untag this Chest object and destroy its OpenChest script to remove UI pop up and interaction detection
            gameObject.tag = "Untagged";
            Destroy(this);
        }
    }
}

