using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField] private float foreceMagnitude;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        RaycastHit rHit;

        Physics.Raycast(transform.position, Vector3.down, out rHit);

        if (rigidbody != null)
        {
            Vector3 forceDir = hit.gameObject.transform.position - transform.position;
            forceDir.y = 0;
            forceDir.Normalize();

            if (rHit.collider.tag != rigidbody.gameObject.tag)
            {
                rigidbody.AddForce(forceDir * foreceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
