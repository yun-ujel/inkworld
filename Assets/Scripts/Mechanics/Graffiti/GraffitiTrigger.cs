using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inkworld.Mechanics
{
    public class GraffitiTrigger : MonoBehaviour
    {
        [field: SerializeField] public int GraffitiID { get; set; }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
        }
    }

}