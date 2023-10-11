using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableVisual : MonoBehaviour
{
    private bool isGrabbed;

    [Header("References")]
    [SerializeField] private MeshRenderer meshRenderer;

    [Space]

    [Header("Materials")]
    [SerializeField] private Material baseOutlineMaterial;

    [Space]

    [SerializeField] private Material rayHoveredOutlineMaterial;
    [SerializeField] private Material directHoveredOutlineMaterial;

    [Space]

    [SerializeField] private Material raySelectedOutlineMaterial;
    [SerializeField] private Material directSelectedOutlineMaterial;

    public void StartHover(HoverEnterEventArgs args)
    {
        if (args.interactorObject.GetType() == typeof(XRRayInteractor))
        {
            SetOutline(rayHoveredOutlineMaterial);
        }
        else if (args.interactorObject.GetType() == typeof(XRDirectInteractor))
        {
            SetOutline(directHoveredOutlineMaterial);
        }
    }

    public void StartGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        if (args.interactorObject.GetType() == typeof(XRRayInteractor))
        {
            SetOutline(raySelectedOutlineMaterial);
        }
        else if (args.interactorObject.GetType() == typeof(XRDirectInteractor))
        {
            SetOutline(directSelectedOutlineMaterial);  
        }
    }

    public void EndHover(HoverExitEventArgs args)
    {
        if (!isGrabbed && !args.interactableObject.isHovered)
        {
            ResetMaterials();
        }
    }

    public void EndGrab(SelectExitEventArgs args)
    {
        ResetMaterials();
        isGrabbed = false;
    }

    void SetOutline(Material outline)
    {
        Material[] materials = meshRenderer.materials;
        materials[1] = outline;

        meshRenderer.materials = materials;
    }

    void ResetMaterials()
    {
        SetOutline(baseOutlineMaterial);
    }
}
