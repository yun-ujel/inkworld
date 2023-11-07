using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inkworld.Mechanics.PropCamera
{
    public class PropCameraUI : MonoBehaviour
    {
        #region Parameters
        [Header("References")]
        [SerializeField] private PropCameraFunctionality propCameraVisual;

        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private bool enableSlider;
        [Space]
        [SerializeField] private RawImage overlay;
        [SerializeField] private Image border;

        [Header("Smoothing")]
        [SerializeField, Range(0f, 1f)] private float sliderSmoothTime;

        [Space]

        [SerializeField, Range(0f, 1f)] float blackOverlayDuration = 0.5f;
        [SerializeField, Range(0f, 10f)] float previewOverlayDuration = 0.8f;

        private float value;
        private float valueVelocity;

        private Color validColor = new Color(0f, 1f, 0.3298969f);

        private enum OverlayState
        {
            none,
            preview,
            black
        }

        [Header("Overlay")]
        private int overlayState;
        private float overlayUpdateCountdown;
        private Texture2D overlayTexture;
        #endregion

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

        void Update()
        {
            UpdateSlider();

            UpdateOverlay();
        }

        #region Slider Overlay
        private void UpdateSlider()
        {
            if (enableSlider)
            {
                value = Mathf.SmoothDamp(value, propCameraVisual.CurrentScore, ref valueVelocity, sliderSmoothTime);
                slider.value = value;
            }
        }
        #endregion

        #region Picture Overlay
        private void SetOverlayState(OverlayState state)
        {
            if (state == OverlayState.black)
            {
                overlay.gameObject.SetActive(true);

                overlayState = (int)OverlayState.black;
                overlay.color = Color.black;
                border.color = Color.clear;

                overlayUpdateCountdown = blackOverlayDuration;
            }
            else if (state == OverlayState.none)
            {
                overlay.texture = null;
                overlay.color = Color.clear;
                overlay.gameObject.SetActive(false);

                overlayState = (int)OverlayState.none;
            }
            else if (state == OverlayState.preview)
            {
                // Preview sets it to clear, so ViewCameraSwitcher can display a billboard onto the Render Texture
                overlay.texture = null;
                overlay.color = Color.clear;
                border.color = Color.clear;
                overlay.gameObject.SetActive(false);

                overlayState = (int)OverlayState.preview;

                overlayUpdateCountdown = previewOverlayDuration;
            }
        }

        void UpdateOverlay()
        {
            if (overlayState == (int)OverlayState.none)
            {
                if (propCameraVisual.CurrentScore > 0f)
                {
                    border.color = validColor;
                }
                else
                {
                    border.color = Color.white;
                }
                return;
            }

            overlayUpdateCountdown -= Time.deltaTime;

            if (overlayUpdateCountdown <= 0)
            {
                switch(overlayState)
                {
                    case (int)OverlayState.preview:
                        SetOverlayState(OverlayState.none);

                        break;
                    case (int)OverlayState.black:
                        SetOverlayState(OverlayState.preview);

                        break;
                    default: 
                        return;
                }
            }
        }
        #endregion
    }
}
