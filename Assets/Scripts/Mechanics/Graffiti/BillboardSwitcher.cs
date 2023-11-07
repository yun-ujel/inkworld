using Inkworld.Mechanics.PropCamera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inkworld.Mechanics
{
    public class BillboardSwitcher : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private Texture2D[] unlockedBillboardTextures;

        [Header("References")]
        [SerializeField] private RawImage[] billboardImages;

        [Space]
        [SerializeField] private PropCameraFunctionality propCameraFunctionality;

        private void Start()
        {
            propCameraFunctionality.OnTakePictureEvent += OnTakePicture;
        }

        private void OnTakePicture(object sender, PropCameraFunctionality.OnTakePictureEventArgs args)
        {
            
        }
    }
}