using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class DroppedBodyPart : BaseInteractable
{
    public BodyParts part;

    private bool initialised = false;
    

    private void Awake()
    {
        if(!initialised)Initialise(new BodyParts(BodyParts.PartType.Arms));
    }


    
    public override void Interact()
    {
        PlayerController.instance.PickupPart(this);
        Destroy(gameObject);
    }

    public void Initialise(BodyParts part)
    {
        initialised = true;
        this.part = part;
        Name = $"{part.pt}";
        PopupMessage = $"E to Pickup {Name}";

        //Make the model the appropriate one
    }
}
