using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionSystem : MonoBehaviour
{
    private BaseInteractable lastHover,currentHover;

    private void Update()
    {
        PerformRay();

    }

    private void PerformRay()
    {
        //DoHover(lastHover, false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 999, ~0, QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.TryGetComponent(out BaseInteractable interactable))
            {
                if (interactable == currentHover) return;
            }
            currentHover = interactable;
        }
        else
        {
            currentHover = null;
        }

        DoHover(lastHover, false);
        DoHover(currentHover, true);
    }

    public void TryInteract()
    {
        if (currentHover == null) return;

        currentHover.Interact();
    }


    private void DoHover(BaseInteractable interactable,bool active)
    {
        if (interactable == null) return;
        if(active) currentHover = interactable;

        if (active)
        {
            print($"{interactable.Name} was hovered");
        }
        else
        {
            print($"{interactable.Name} was UnHovered");
        }
    }

    private void LateUpdate()
    {
        lastHover = currentHover;
    }
}
