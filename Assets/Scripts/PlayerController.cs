using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController), typeof(InteractionSystem))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private InteractionSystem intSys;
    public LayerMask GroundLayers;
    public static PlayerController instance { get; private set; }
    [SerializeField] private float MoveSpeed = 10f;

    private void Awake()
    {
        instance = this;
        cc = GetComponent<CharacterController>();
        intSys = GetComponent<InteractionSystem>();
    }

    private void Update()
    {
        MovePlayer();
        HandleInputs();
        LookAtCursor();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            intSys.TryInteract();
        }
    }


    private void MovePlayer()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveInput = moveInput.normalized;

        Vector3 moveVector = moveInput * MoveSpeed * Time.deltaTime;

        cc.Move(moveVector);
    }

    private void LookAtCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, GroundLayers, QueryTriggerInteraction.Ignore))
        {
            Vector3 lookPoint = hit.point;
            lookPoint.y = transform.position.y;
            transform.LookAt(lookPoint);
        }
        // transform.LookAt(Input.mousePosition);
    }
}
