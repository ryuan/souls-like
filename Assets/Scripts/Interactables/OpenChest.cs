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



            // Lock his transform to a certian point in front of chest
            // Open chest's lid and animate player
            // Spawn an item inside the chest that players can pickup
        }

        IEnumerator OpenChestInteraction()
        {
            Vector3 targetMovePos = transform.position + transform.forward * openPosZOffset;
            Vector3 targetLookPos = transform.position;

            yield return StartCoroutine(playerLocomotion.SlerpFunction(targetMovePos, targetLookPos));

            animatorManager.PlayTargetAnimation("Open_Chest", true);
            anim.Play("Lift_Chest_Lid");

            StartCoroutine(SpawnItemInChest());

            WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

            if (weaponPickUp != null)
            {
                weaponPickUp.weapon = weaponInChest;
            }
        }

        IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1);

            Instantiate(itemSpawner, transform);
            Destroy(this);
        }
    }
}

