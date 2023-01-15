using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RY
{
    public class PlayerInteractions : MonoBehaviour
    {
        CameraHandler cameraHandler;
        InteractableUI interactableUI;
        InputHandler inputHandler;
        PlayerManager playerManager;

        [Header("UI Update Gameobjects")]
        public GameObject interactableAlertUIGameObject;
        public GameObject itemPickUpAlertUIGameObject;

        [Header("Detection Attributes")]
        [SerializeField]
        float detectionSphereRadius = 0.5f;
        [SerializeField]
        float detectionSphereHeight = 0.75f;
        [SerializeField]
        float detectionSphereDistance = 0.6f;
        
        Vector3 detectionSphereOffset;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            interactableUI = FindObjectOfType<InteractableUI>();
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        public void CheckForInteractables()
        {
            detectionSphereOffset = new Vector3(0, detectionSphereHeight) + (transform.forward * detectionSphereDistance);
            Vector3 checkPosition = transform.position + detectionSphereOffset;
            Collider[] hitColliders = Physics.OverlapSphere(
                checkPosition, detectionSphereRadius, cameraHandler.ignoreLayers, QueryTriggerInteraction.Collide
                );
            List<Interactable> interactables = new List<Interactable>();

            // Add all interactable script components from hitColliders to a list
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Interactable")
                {
                    Interactable interactable = hitCollider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        interactables.Add(interactable);
                    }
                }
            }

            // Show the UI popup for any interactable and let player interact with it
            // Otherwise if no interactable, auto-close interaction alert and let player close item pop up
            if (interactables.Count > 0)
            {
                foreach (Interactable interactable in interactables)
                {
                    string interactableText = interactable.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableAlertUIGameObject.SetActive(true);

                    if (inputHandler.a_Input && playerManager.isInteracting == false)
                    {
                        interactable.Interact();
                    }
                }
            }
            else
            {
                if (interactableAlertUIGameObject != null)
                {
                    interactableAlertUIGameObject.SetActive(false);
                }

                if (itemPickUpAlertUIGameObject != null && inputHandler.a_Input)
                {
                    itemPickUpAlertUIGameObject.SetActive(false);
                }
            }
        }

        private void OnDrawGizmos()
        {
            // For debugging CheckForInteractables function and its SphereCast collision against interactable colliders
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + detectionSphereOffset, detectionSphereRadius);
        }
    }
}
