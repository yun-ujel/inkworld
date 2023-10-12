using UnityEngine;
using Inkworld.Extensions;
using UnityEngine.XR.Interaction.Toolkit;

namespace Inkworld.Mechanics.PropCamera
{
    public class PropCameraFunctionality : MonoBehaviour
    {
        public event System.EventHandler<OnTakePictureEventArgs> OnTakePictureEvent;

        public class OnTakePictureEventArgs : System.EventArgs
        {
            public Texture2D PictureTexture { get; private set; }
            public float PictureScore { get; private set; }
            public int GraffitiID { get; private set; }

            public OnTakePictureEventArgs(Texture2D pictureTexture, float pictureScore, int graffitiID)
            {
                PictureTexture = pictureTexture;
                PictureScore = pictureScore;
                GraffitiID = graffitiID;
            }
        }

        [Header("Functionality")]
        [SerializeField, Range(0f, 1f)] private float takePictureCooldown;

        [Header("Visuals")]
        [SerializeField] private RenderTexture renderTexture;

        [Header("Layers")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask graffitiLayer;

        [Header("BoxCast Settings")]
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 size;

        public float CurrentScore { get; private set; }
        private int currentGraffitiID;

        void FixedUpdate()
        {
            Ray direction = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(direction, out RaycastHit groundHit, 100f, groundLayer))
            {
                if (CheckForGraffiti(direction, groundHit))
                {
                    return;
                }
            }
            currentGraffitiID = -1;
            CurrentScore = 0f;
        }

        private bool CheckForGraffiti(Ray direction, RaycastHit groundHit)
        {
            float groundDistance = groundHit.distance;
            bool hitGraffiti = Physics.Raycast(direction, out RaycastHit graffitiHit, groundDistance + 2f, graffitiLayer);

            if (hitGraffiti)
            {
                float distance = Vector3.SqrMagnitude(graffitiHit.point - graffitiHit.transform.position);
                float maxDistance = Mathf.Max(graffitiHit.transform.lossyScale.x, graffitiHit.transform.lossyScale.y, graffitiHit.transform.lossyScale.z) / 2f;

                maxDistance *= maxDistance;

                CurrentScore = distance.Remap(0, maxDistance, 1, 0);
                currentGraffitiID = graffitiHit.collider.gameObject.GetComponent<GraffitiTrigger>().GraffitiID;
            }

            return hitGraffiti;
        }

        public void Activate(ActivateEventArgs args)
        {
            TakePicture();
        }

        public void TakePicture()
        {
            Texture2D pictureTexture = renderTexture.ToTexture2D();

            OnTakePictureEvent.Invoke(this, new OnTakePictureEventArgs(pictureTexture, CurrentScore, currentGraffitiID));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + offset, size);
        }
    }
}
