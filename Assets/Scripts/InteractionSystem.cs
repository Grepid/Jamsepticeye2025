using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionSystem : MonoBehaviour
{
    [Tooltip("The hovered Interactable on the previous frame")]
    private BaseInteractable lastHover;
    [Tooltip("The hovered Interactable on this current frame")]
    private BaseInteractable currentHover;

    public LayerMask InteractionRayLayers;
    [SerializeField]private PopupUI _popupUIPrefab;
    public PopupUI PopupUIPrefab
    {
        get { return _popupUIPrefab; }
        set { _popupUIPrefab = value; }
    }

    private PopupUI currentPopupUI;

    [SerializeField] private float interactRange;

    private void Update()
    {
        PerformRay();
        TryUpdatePopup();
    }

    private void PerformRay()
    {
        //Converts Mouse Position to a Ray Struct
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Fires the Ray
        if (Physics.Raycast(ray, out RaycastHit hit, 999, InteractionRayLayers, QueryTriggerInteraction.Ignore))
        {
            //Will try to get a component of Type BaseInteractable and operate on it if successful
            if(hit.collider.TryGetComponent(out BaseInteractable interactable))
            {
                //If the interactable hit is the same as latest known time, just does nothing new and returns
                if (interactable == currentHover) return;
            }
            currentHover = interactable;
        }
        else
        {
            currentHover = null;
        }

        //Runs hover functions for the current, and the previous interactable
        DoHover(lastHover, false);
        DoHover(currentHover, true);
    }
    private void TryUpdatePopup()
    {
        if (currentPopupUI == null || currentHover == null) return;
        string message = Vector3.Distance(PlayerController.instance.transform.position, currentHover.transform.position) > interactRange ? "You're too far to interact" : currentHover.PopupMessage;
        currentPopupUI.Initialise(null, message);
    }
    public void TryInteract()
    {
        if (currentHover == null) return;
        if(Vector3.Distance(PlayerController.instance.transform.position,currentHover.transform.position) > interactRange)
        {
            //Run something to say you're too far
            print($"You're too far away to interact");
            return;
        }
        currentHover.Interact();
    }

    /// <summary>
    /// Runs a hover function based off of the interactable passed through. If null it will do nothing
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="active"></param>
    private void DoHover(BaseInteractable interactable,bool active)
    {
        if (interactable == null) return;
        if(active) currentHover = interactable;

        if (active)
        {
            print($"{interactable.Name} was hovered");
            currentPopupUI = PopupUI.CreatePopupUI();
            //string message = Vector3.Distance(PlayerController.instance.transform.position, currentHover.transform.position) > interactRange ? "You're too far to interact" : interactable.PopupMessage;
            //currentPopupUI.Initialise(null, message);
        }
        else
        {
            if (currentPopupUI != null) Destroy(currentPopupUI.gameObject);
            print($"{interactable.Name} was UnHovered");
        }
    }

    private void LateUpdate()
    {
        lastHover = currentHover;
    }
}
