using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DetectClick : MonoBehaviour
{
    [SerializeField]
    private GameObject tooltip; // The tooltip GameObject

    [SerializeField]
    private string directInteractorTag = "DirectInteractor"; // Tag for the hand or controller

    private bool isTriggerPressed = false; // Current state of the trigger
    private bool isInteractorInside = false; // Whether the interactor is inside this object

    void Start()
    {
        if (tooltip != null)
        {
            tooltip.SetActive(false); // Ensure the tooltip starts hidden
        }
    }

    void Update()
    {
        DetectTriggerPress();

        // Toggle tooltip when the interactor is inside and trigger is pressed
        if (isInteractorInside && isTriggerPressed)
        {
            ToggleTooltip();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the interacting object is the Direct Interactor
        if (other.CompareTag(directInteractorTag))
        {
            isInteractorInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the interactor state when it leaves the object
        if (other.CompareTag(directInteractorTag))
        {
            isInteractorInside = false;
        }
    }

    private void DetectTriggerPress()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, inputDevices);

        if (inputDevices.Count > 0)
        {
            InputDevice rightHandController = inputDevices[0];
            bool triggerState;
            rightHandController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerState);

            // Detect if the trigger is newly pressed
            if (triggerState && !isTriggerPressed)
            {
                isTriggerPressed = true;
            }
            else if (!triggerState)
            {
                isTriggerPressed = false;
            }
        }
    }

    private void ToggleTooltip()
    {
        if (tooltip != null)
        {
            tooltip.SetActive(!tooltip.activeSelf); // Toggle tooltip visibility
        }
    }
}
