using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectTest : MonoBehaviour
{
    public void SelectEntered(SelectEnterEventArgs e)
    {
        Debug.Log("Selected");
    }

    public void SelectExited(SelectExitEventArgs e)
    {
        XRRayInteractor rayInteractor = (XRRayInteractor)e.interactorObject;
    }
}
