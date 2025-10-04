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
        if (!initialised) Initialise(new BodyParts(BodyParts.PartType.Arms, BodyParts.Variation.Average), "TorsoRig/TorsoMid/TorsoUpper/LeftArmRoot/ZombieBaseLeftArm/LeftArm");
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }


    
    public override void Interact()
    {
        PlayerController.instance.PickupPart(this);
        Destroy(gameObject);
    }

    public void Initialise(BodyParts part, string filename = null)
    {
        initialised = true;
        this.part = part;
        Name = (part.pt != BodyParts.PartType.Torso) ? $"{part.v + " " + part.pt}" : $"{part.tv + " " + part.pt}";
        PopupMessage = $"E to Pickup {Name}";

        if (filename != null)
        {
            //Make the model the appropriate one
            var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer == null)
            {
                return;
            }

            // Load the FBX prefab from Resources
            GameObject meshPrefab = Resources.Load<GameObject>($"3D/{filename}");
            Debug.Log(filename);
            if (meshPrefab == null)
            {
                Debug.LogWarning($"Mesh not found for {filename}; defaulting to average");
                return;
            }
            else
            {
                GameObject visual = Instantiate(meshPrefab, transform);
                visual.name = "Visual";
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localRotation = Quaternion.identity;
                visual.transform.localScale = Vector3.one;

                var skinned = visual.GetComponentInChildren<SkinnedMeshRenderer>();
                if (skinned != null && skinned.sharedMesh != null)
                {
                    // Get the mesh’s local-space center and flip it so it’s centered in the capsule
                    Vector3 offset = -skinned.sharedMesh.bounds.center;
                    skinned.transform.localPosition = offset;
                }
            }
            
            // SkinnedMeshRenderer sourceRenderer = meshPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            // if (sourceRenderer == null)
            // {
            //     Debug.LogWarning($"No SkinnedMeshRenderer found on {filename}");
            //     return;
            // }

            // // Assign the new mesh and material + copy bones over
            // renderer.sharedMesh = sourceRenderer.sharedMesh;
            // renderer.sharedMaterial = sourceRenderer.sharedMaterial;
        }
    }
}
