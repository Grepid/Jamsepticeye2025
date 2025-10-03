using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using static BodyParts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(CharacterController), typeof(InteractionSystem))]
public class PlayerController : MonoBehaviour
{
    public CharacterController cc { get; private set; }
    public InteractionSystem intSys { get; private set; }
    public LayerMask GroundLayers;
    public static PlayerController instance { get; private set; }
    private float timeDelay = 0.5f;
    [SerializeField] private float MoveSpeed = 10f;

    public BodyParts heldPart;
    private DroppedBodyPart droppedPartPrefab;

    private void Awake()
    {
        instance = this;
        cc = GetComponent<CharacterController>();
        intSys = GetComponent<InteractionSystem>();

        GameObject shovel = this.transform.GetChild(0).gameObject;
        shovel.SetActive(false);

        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/DroppedBodyPart.prefab").Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            droppedPartPrefab= obj.Result.GetComponent<DroppedBodyPart>();
            Addressables.Release(obj);
        };
    }

    private void Update()
    {
        MovePlayer();
        HandleInputs();
        LookAtCursor();

        timeDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G))
        {
            string container = heldPart == null ? "No Object" : heldPart.pt.ToString();
            print($"Player is holding {container}");
        }
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            intSys.TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropPart();
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
        if (!this.transform.GetChild(0).gameObject.activeSelf) {
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

    public void PickupPart(DroppedBodyPart part)
    {
        if(heldPart != null)
        {
            DropPart(part);
        }

        GivePart(part.part);
        
    }

    public void GivePart(BodyParts part)
    {
        heldPart = part;
        //Give the character model the arm in the hand here
    }

    public DroppedBodyPart DropPart(DroppedBodyPart replace = null)
    {
        DroppedBodyPart newPart = Instantiate(droppedPartPrefab);
        if (replace != null)
        {
            //Instantiate a new DroppedPart here before assigning null

            newPart.transform.position = replace.transform.position;
            newPart.transform.rotation = replace.transform.rotation;
        }
        else
        {
            newPart.transform.position = transform.position;
        }
        newPart.Initialise(heldPart);

        ReleasePart();

        return newPart;
    }

    public BodyParts ReleasePart()
    {
        BodyParts part = heldPart;

        heldPart = null;

        //Remove the asset from the character's hands

        return part;
    }
}
