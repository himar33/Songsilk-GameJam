using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController charCon;
    public float speed = 2;
    bool charIsGrounded;

    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (charCon)
        {
            handleMovement();
            charIsGrounded = charCon.isGrounded;
        }
    }

    void handleMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 currentMovement = new Vector3(horizontal, charIsGrounded ? 0.0f : -1.0f, vertical) * Time.deltaTime * speed;
        charCon.Move(currentMovement);
    }
}
