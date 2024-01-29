using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float turnRate;
    float targetSpeed;
    float currentSpeed;
    float rotation;

    bool rotatingReverse;

    float startPoint = 40f;
    float endPoint = 110f;

    private void Start()
    {
        rotation = startPoint;
        rotatingReverse = false;
    }

    void Update()
    {
        if (!rotatingReverse && rotation <= endPoint)
        {
            // Rotate
            targetSpeed = speed;
        }
        else if (!rotatingReverse && rotation >= endPoint)
        {
            // Reverse Rotation
            rotatingReverse = true;
            targetSpeed = -speed;
        }
        else if (rotatingReverse && rotation >= startPoint)
        {
            // Rotate in Reverse
            targetSpeed = -speed;
        }
        else if (rotatingReverse && rotation <= endPoint)
        {
            // Reverse Rotation
            rotatingReverse = false;
            targetSpeed = speed;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * turnRate);
        rotation += currentSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation, transform.rotation.eulerAngles.z);
    }
}
