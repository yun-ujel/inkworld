using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPullInteractable : XRBaseInteractable
{
    [Space]
    [Header("Pull Settings")]
    [SerializeField] private float requiredExtraDistance;

    [Space]

    private float startDistance;
    [SerializeField] private Rigidbody body;

    void StartGrab(SelectEnterEventArgs args)
    {
        startDistance = Vector3.Distance(args.interactorObject.transform.position, transform.position);
    }

    void EndGrab(SelectExitEventArgs args)
    {
        TryPull(args);
    }

    private void TryPull(SelectExitEventArgs args)
    {
        float endDistance = Vector3.Distance(args.interactorObject.transform.position, transform.position);
        Debug.Log($"Ending Distance: {endDistance}");

        if (startDistance < endDistance - requiredExtraDistance)
        {
            Vector3 pull = (args.interactorObject.transform.position - transform.position).normalized * endDistance * 2f;


            body.velocity = pull;
            Debug.Log(pull.magnitude);
        }

        startDistance = 0f;
    }

    private void Start()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody>();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(StartGrab);
        selectExited.AddListener(EndGrab);
    }

    protected override void OnDisable()
    {
        selectEntered.RemoveListener(StartGrab);
        selectExited.RemoveListener(EndGrab);
        base.OnDisable();
    }
}
