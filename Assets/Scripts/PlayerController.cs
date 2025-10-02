using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(CharacterController), typeof(InteractionSystem))]
public class PlayerController : MonoBehaviour
{
    public CharacterController cc { get; private set; }
    public InteractionSystem intSys { get; private set; }
    public LayerMask GroundLayers;
    public static PlayerController instance { get; private set; }
    private float timeDelay = 0.5f;
    [SerializeField] private float MoveSpeed = 10f;

    private void Awake()
    {
        instance = this;
        cc = GetComponent<CharacterController>();
        intSys = GetComponent<InteractionSystem>();

        GameObject shovel = this.transform.GetChild(0).gameObject;
        shovel.SetActive(false);
    }

    private void Update()
    {
        MovePlayer();
        HandleInputs();
        LookAtCursor();

        timeDelay -= Time.deltaTime;
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            intSys.TryInteract();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShovelTime();
        }
    }

    private void ShovelTime()
    {
        if (timeDelay <= 0f)
        {
            GameObject shovel = this.transform.GetChild(0).gameObject;
            shovel.SetActive(true);
            Vector3 originalPos = shovel.transform.position;
            Quaternion originalRot = shovel.transform.rotation;
            // Debug.Log(shovel.name);
            Sequence seq = DOTween.Sequence();
            seq.Join(shovel.transform.DOLocalMove(this.transform.GetChild(1).localPosition, 0.5f));
            seq.Join(shovel.transform.DOLocalRotateQuaternion(this.transform.GetChild(1).localRotation, 0.5f));

            seq.OnComplete(() =>
            {
                seq.Rewind();
                shovel.SetActive(false);
            });
            timeDelay = 0.5f;
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
