using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiTrigger : MonoBehaviour
{
    [field: SerializeField] public Texture2D GraffitiTexture { get; set; }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);
    }
}
