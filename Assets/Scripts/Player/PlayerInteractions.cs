using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerInteractions : MonoBehaviour
    {
        CameraHandler cameraHandler;
        InteractableUI interactableUI;
        InputHandler inputHandler;
        PlayerManager playerManager;
        PlayerLocomotion playerLocomotion;

        [Header("UI Update Gameobjects")]
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableUIGameObject;

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
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        public void CheckForInteractables()
        {
            detectionSphereOffset = new Vector3(0, detectionSphereHeight) + (transform.forward * detectionSphereDistance);
            Vector3 checkPosition = transform.position + detectionSphereOffset;
            Collider[] hitColliders = Physics.OverlapSphere(
                checkPosition, detectionSphereRadius, cameraHandler.ignoreLayers, QueryTriggerInteraction.Collide
                );

            if (hitColliders.Length > 0)
            {
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "Interactable")
                    {
                        Interactable interactable = hitCollider.GetComponent<Interactable>();

                        if (interactable != null)
                        {
                            string interactableText = interactable.interactableText;
                            interactableUI.interactableText.text = interactableText;
                            interactableUIGameObject.SetActive(true);

                            if (inputHandler.a_Input && playerManager.isInteracting == false)
                            {
                                StartCoroutine(HandleInteraction(interactable));
                            }
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                }
            }
        }

        IEnumerator HandleInteraction(Interactable interactable)
        {
            yield return StartCoroutine(playerLocomotion.SlerpFunction(transform.position, interactable.transform.position));

            interactable.Interact();
        }

        private void OnDrawGizmos()
        {
            // For debugging CheckForInteractables function and its SphereCast collision against interactable colliders
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + detectionSphereOffset, detectionSphereRadius);
        }
    }
}
