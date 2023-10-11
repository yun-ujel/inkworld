using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Inkworld.Extensions;
using TMPro;

namespace Inkworld.Mechanics
{
    public class PropCamera : MonoBehaviour
    {
        [Header("Layers")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask graffitiLayer;

        [Space]

        [Header("Box Settings")]
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 size;

        [Space]

        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI UGUI;

        void FixedUpdate()
        {
            Ray direction = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(direction, out RaycastHit groundHit, 100f, groundLayer))
            {
                float groundDistance = groundHit.distance;
                bool hitGraffiti = Physics.Raycast(direction, out RaycastHit graffitiHit, groundDistance + 2f, graffitiLayer);

                if (hitGraffiti)
                {
                    float distance = Vector3.SqrMagnitude(graffitiHit.point - graffitiHit.transform.position);
                    float maxDistance = Mathf.Max(graffitiHit.transform.lossyScale.x, graffitiHit.transform.lossyScale.y, graffitiHit.transform.lossyScale.z) / 2f;

                    maxDistance *= maxDistance;

                    slider.value = distance.Remap(0, maxDistance, 1, 0);

                    UGUI.text = $"Max: {maxDistance}, Current: {distance}";

                    return;
                }
            }
            slider.value = 0f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + offset, size);
        }
    }
}
