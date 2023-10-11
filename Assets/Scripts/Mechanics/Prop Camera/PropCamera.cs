using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void FixedUpdate()
    {
        Ray direction = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(direction, out RaycastHit groundHit, 100f, groundLayer))
        {
            float maxDistance = groundHit.distance;
            //bool hitGraffiti = Physics.BoxCast(transform.position + offset, size / 2, transform.forward, out RaycastHit graffitiHit, Quaternion.identity, maxDistance + 2f, graffitiLayer);
            bool hitGraffiti = Physics.Raycast(direction, out RaycastHit graffitiHit, 100f, graffitiLayer);

            if (hitGraffiti)
            {
                float distance = Vector3.Distance(graffitiHit.point, graffitiHit.transform.position);
                slider.value = 1f;
                return;
            }
        }
        SetSliderValue(0f);
    }

    private void SetSliderValue(float dotProduct)
    {
        if (dotProduct >= 0.95f)
        {
            float remapped = dotProduct.Remap(0.95f, 1, 0, 1);
            slider.value = remapped;

            return;
        }
        slider.value = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
