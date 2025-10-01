using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController), typeof(InteractionSystem))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private InteractionSystem intSys;
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
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        moveInput = moveInput.normalized;

        Vector3 moveVector = moveInput * MoveSpeed * Time.deltaTime;

        cc.Move(moveVector);
    }
}
