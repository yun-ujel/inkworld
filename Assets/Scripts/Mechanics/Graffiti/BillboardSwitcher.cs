using Inkworld.Mechanics.PropCamera;
using UnityEngine;

namespace Inkworld.Mechanics
{
    public class BillboardSwitcher : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private Texture2D[] unlockedBillboardTextures;

        [Header("References")]
        [SerializeField] private BillboardDisplay[] billboardDisplays;

        [Space]
        [SerializeField] private PropCameraFunctionality propCameraFunctionality;

        private void Start()
        {
            propCameraFunctionality.OnTakePictureEvent += OnTakePicture;
        }

        private void OnTakePicture(object sender, PropCameraFunctionality.OnTakePictureEventArgs args)
        {
            billboardDisplays[args.GraffitiID].TransitionBillboardTexture(unlockedBillboardTextures[args.GraffitiID]);
        }
    }
}