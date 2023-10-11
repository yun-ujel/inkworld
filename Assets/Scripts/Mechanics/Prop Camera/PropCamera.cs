using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCamera : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask graffitiLayer;

    void FixedUpdate()
    {
        Ray direction = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(direction, out RaycastHit groundHit, 100f, groundLayer))
        {
            float maxDistance = groundHit.distance;
            bool hitGraffiti = Physics.Raycast(direction, out RaycastHit graffitiHit, maxDistance + 1f, graffitiLayer);

            if (hitGraffiti)
            {

            }
        }
    }
}
