using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class DroppedBodyPart : BaseInteractable
{
    public BodyParts part;
    private bool initialised = false;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;


    private void Awake()
    {
        if (!initialised) Initialise(new BodyParts(BodyParts.PartType.Arms, BodyParts.Variation.Average));
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
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
        Name = (part.pt != BodyParts.PartType.Torso) ? $"{part.v + " " + part.pt}" : $"{part.tv + " " + part.pt}";
        PopupMessage = $"E to Pickup {Name}";

        //Make the model the appropriate one
        // SkinnedMeshRenderer limbRenderer;
        // if (part.v == BodyParts.Variation.Average) {
        //     if (part.pt == BodyParts.PartType.Torso) {
        //         limbRenderer = transform.Find();
        //     }
        // }

        // Mesh baked = new Mesh();
        // limbRenderer.BakeMesh(baked);

        // meshFilter.mesh = baked;

        // meshRenderer.materials = limbRenderer.sharedMaterials;
    }
}
