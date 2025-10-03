using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StorageCounter : BaseInteractable
{
    public BodyParts storedPart;
    private DroppedBodyPart partPrefab;
    private DroppedBodyPart storedModel;
    public Transform attachmentPoint;

    private void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/DroppedBodyPart.prefab").Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            partPrefab = obj.Result.GetComponent<DroppedBodyPart>();
            SetupModel();
            Addressables.Release(obj);
        };
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            string container = storedPart == null ? "No Object" : storedPart.pt.ToString();
            print($"Storage counter has {container} in it");
        }
        UpdateMessage();
    }

    private void SetupModel()
    {
        storedModel = Instantiate(partPrefab);
        storedModel.GetComponent<Collider>().enabled = false;
        storedModel.gameObject.transform.position = attachmentPoint.position;
        storedModel.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        if (storedPart == null && PlayerController.instance.heldPart == null) return;
        if (storedPart != null) TakePart();
        else StorePart(PlayerController.instance.heldPart);

        //PlayerController.instance.heldPart = null;

        
    }

    private void UpdateMessage()
    {
        string message = string.Empty;
        if (storedPart == null) message = "E to Store Part";
        if (storedPart != null && PlayerController.instance.heldPart == null) message = "E to Pickup Stored Part";
        if (storedPart != null && PlayerController.instance.heldPart != null) message = "E to Swap Stored Part";


        PopupMessage = message;
    }

    public void StorePart(BodyParts part)
    {
        //Make it visible up here
        storedModel.gameObject.SetActive(true);
        storedModel.Initialise(part);

        storedPart = part;
        PlayerController.instance.heldPart = null;
    }

    public void TakePart()
    {
        BodyParts part = storedPart;

        if(PlayerController.instance.heldPart != null)
        {
            StorePart(PlayerController.instance.heldPart);
        }

        PlayerController.instance.PickupPart(part);
        storedModel.gameObject.SetActive(false);
        storedPart = null;
    }

    public void SwapPart()
    {

    }
}
