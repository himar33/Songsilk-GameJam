using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController charCon;
    public float speed = 2;
    bool charIsGrounded;

    Interactable interactableObject;

    public HUD hud;

    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (charCon)
        {
            handleMovement();

            handleInputs();

            charIsGrounded = charCon.isGrounded;
        }
    }

    void handleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.OnInteract();
        }
    }

    void handleMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 currentMovement = new Vector3(horizontal, charIsGrounded ? 0.0f : -1.0f, vertical) * Time.deltaTime * speed;
        charCon.Move(currentMovement);
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactableObject = interactable;
            hud.OpenMessagePanel(other.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponent <Interactable>();
        if (interactable != null)
        {
            hud.CloseMessagePanel();
            interactableObject = null;
        }
    }
}
