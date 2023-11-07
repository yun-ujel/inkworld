using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inkworld.Mechanics.PropCamera
{
    public class ViewCameraSwitcher : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] private Camera[] viewCameras;
        [SerializeField] private Camera propCameraComponent;

        [Header("References")]
        [SerializeField] private PropCameraFunctionality propCameraFunctionality;

        [Header("Settings")]
        [SerializeField] private float focusPreviewCooldown;

        private bool isFocusingPreview;
        private float focusPreviewCooldownCounter;

        private void Start()
        {
            propCameraFunctionality.OnTakePictureEvent += OnTakePicture;
        }

        private void OnTakePicture(object sender, PropCameraFunctionality.OnTakePictureEventArgs args)
        {
            FocusViewCamera(args.GraffitiID);
            isFocusingPreview = true;
            focusPreviewCooldownCounter = focusPreviewCooldown;
        }

        private void Update()
        {
            if (isFocusingPreview)
            {
                focusPreviewCooldownCounter -= Time.deltaTime;

                if (focusPreviewCooldownCounter <= 0)
                {
                    FocusPropCamera();
                    isFocusingPreview = false;
                }
            }
        }

        #region Camera Focus Methods
        private void FocusViewCamera(int index)
        {
            for (int i = 0; i < viewCameras.Length; i++)
            {
                if (i == index)
                {
                    viewCameras[i].gameObject.SetActive(true);
                    continue;
                }
                viewCameras[i].gameObject.SetActive(false);
            }
        }

        private void FocusPropCamera()
        {
            for (int i = 0; i < viewCameras.Length; i++)
            {
                viewCameras[i].gameObject.SetActive(false);
            }
            propCameraComponent.gameObject.SetActive(true);
        }
        #endregion
    }
}
