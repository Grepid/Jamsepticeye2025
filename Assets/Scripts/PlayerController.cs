using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    [SerializeField] private float MoveSpeed = 10f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        moveInput = moveInput.normalized;

        Vector3 moveVector = moveInput * MoveSpeed * Time.deltaTime;

        cc.Move(moveVector);
    }
}
