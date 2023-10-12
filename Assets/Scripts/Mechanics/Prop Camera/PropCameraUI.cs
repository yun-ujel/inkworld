using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inkworld.Mechanics.PropCamera
{
    public class PropCameraUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PropCameraFunctionality propCameraVisual;

        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private RawImage overlay;

        [Header("Smoothing")]
        [SerializeField, Range(0f, 1f)] private float sliderSmoothTime;

        [Space]

        [SerializeField, Range(0f, 1f)] float blackOverlayDuration = 0.5f;
        [SerializeField, Range(0f, 1f)] float pictureOverlayDuration = 0.8f;

        private float value;
        private float valueVelocity;

        private enum OverlayState
        {
            none,
            picture,
            black
        }

        [Header("Overlay")]
        private int overlayState;
        private float overlayUpdateCountdown;
        private Texture2D overlayTexture;

        private void Start()
        {
            propCameraVisual.OnTakePictureEvent += OnTakePicture;
            SetOverlayState(OverlayState.none);
        }

        private void OnTakePicture(object sender, PropCameraFunctionality.OnTakePictureEventArgs args)
        {
            overlayTexture = args.PictureTexture;
            SetOverlayState(OverlayState.black);
        }

        private void SetOverlayState(OverlayState state)
        {
            if (state == OverlayState.black)
            {
                overlay.gameObject.SetActive(true);

                overlayState = (int)OverlayState.black;
                overlay.color = Color.black;

                overlayUpdateCountdown = blackOverlayDuration;
            }
            else if (state == OverlayState.none)
            {
                overlay.texture = null;
                overlay.color = Color.clear;
                overlay.gameObject.SetActive(false);

                overlayState = (int)OverlayState.none;
            }
            else if (state == OverlayState.picture)
            {
                overlay.texture = overlayTexture;
                overlay.color = Color.white;

                overlayState = (int)OverlayState.picture;

                overlayUpdateCountdown = pictureOverlayDuration;
            }
        }

        void Update()
        {
            value = Mathf.SmoothDamp(value, propCameraVisual.CurrentScore, ref valueVelocity, sliderSmoothTime);
            slider.value = value;

            UpdateOverlay();
        }

        void UpdateOverlay()
        {
            if (overlayState == (int)OverlayState.none)
            {
                return;
            }

            overlayUpdateCountdown -= Time.deltaTime;

            if (overlayUpdateCountdown <= 0)
            {
                switch(overlayState)
                {
                    case (int)OverlayState.picture:
                        SetOverlayState(OverlayState.none);

                        break;
                    case (int)OverlayState.black:
                        SetOverlayState(OverlayState.picture);

                        break;
                    default: 
                        return;
                }
            }
        }
    }
}
