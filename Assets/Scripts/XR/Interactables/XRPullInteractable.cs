using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Inkworld.XR
{
    public class XRPullInteractable : XRGrabInteractable
    {
        [Space]
        [Header("Pull Settings")]
        [SerializeField] private float requiredExtraDistance;

        [Space]
        [SerializeField] private float horizontalMultiplier = 1.2f;
        [SerializeField] private float verticalMultiplier = 4f;

        [SerializeField] private float minVerticalVelocity = 2f;

        [Space]

        private float startDistance;
        [SerializeField] private Rigidbody body;

        void StartGrab(SelectEnterEventArgs args)
        {
            if (args.interactorObject.GetType() == typeof(XRRayInteractor))
            {
                startDistance = Vector3.Distance(args.interactorObject.transform.position, transform.position);
                SetMovementEnabled(false);
            }
        }

        void SetMovementEnabled(bool enabled)
        {
            trackPosition = enabled;
            trackRotation = enabled;
            throwOnDetach = enabled;
        }

        void EndGrab(SelectExitEventArgs args)
        {
            if (args.interactorObject.GetType() == typeof(XRRayInteractor))
            {
                SetMovementEnabled(true);
                StartCoroutine(Pull(args));
            }
        }

        private IEnumerator Pull(SelectExitEventArgs args)
        {
            yield return 0;
            TryPull(args);
        }

        private void TryPull(SelectExitEventArgs args)
        {
            float endDistance = Vector3.Distance(args.interactorObject.transform.position, transform.position);

            if (startDistance < endDistance - requiredExtraDistance)
            {
                Vector3 pull = (args.interactorObject.transform.position - transform.position).normalized * endDistance;
                pull.x *= horizontalMultiplier;
                pull.z *= horizontalMultiplier;

                pull.y = Mathf.Max((pull.y + minVerticalVelocity) * verticalMultiplier, minVerticalVelocity);

                body.velocity = pull;
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

}