using UnityEngine;
using TMPro;
  using UnityEngine.EventSystems;

public class Corpse : MonoBehaviour
{
    public static Corpse instance { get; private set; }
    private BodyParts[] parts;
    private BodyParts torsoPart = null;
    private BodyParts lArmPart = null;
    private BodyParts rArmPart = null;
    private BodyParts lLegPart = null;
    private BodyParts rLegPart = null;
    public LayerMask corpseLayer;
    public GameObject lArm;
    public GameObject rArm;
    public GameObject lLeg;
    public GameObject rLeg;
    public GameObject torso;
    public GameObject corpseLArm;
    public GameObject corpseRArm;
    public GameObject corpseLLeg;
    public GameObject corpseRLeg;
    public GameObject corpseTorso;
    public Camera cam;
    public GameObject droppablePrefab;

    private void Awake()
    {
        instance = this;
        parts = new BodyParts[5];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        corpseLayer = LayerMask.NameToLayer("Corpse");
        if (!Records.initBodyArray)
        {
            BodyParts[] givenCorpseParts = Records.givenCorpse.ShowBodyParts();
            for (int i = 0; i < givenCorpseParts.Length; i++)
            {
                Records.currZombieParts[i] = Records.givenCorpse.ShowBodyParts()[i];
            }
            Records.initBodyArray = true;
        }
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        WhichClicked();
    }

    public void Initialize()
    {
        foreach (BodyParts part in Records.currZombieParts)
        {
            switch (part.pt)
            {
                case BodyParts.PartType.Torso:
                    torsoPart = part;
                    ApplyVisual("ZombieBaseTorso/Torso", part);
                    break;
                case BodyParts.PartType.Arms:
                    if (rArmPart == null)
                    {
                        rArmPart = part;
                        ApplyVisual("ZombieBaseRightArm/RightArm", part);
                    }
                    else
                    {
                        lArmPart = part;
                        ApplyVisual("ZombieBaseLeftArm/LeftArm", part);
                    }
                    break;
                case BodyParts.PartType.Legs:
                    if (rLegPart == null)
                    {
                        rLegPart = part;
                        ApplyVisual("ZombieBaseRightLeg/RightLeg", part);
                    }
                    else
                    {
                        lLegPart = part;
                        ApplyVisual("ZombieBaseLeftLeg/LeftLeg", part);
                    }
                    break;
            }
        }
    }

    private void ApplyVisual(string path, BodyParts part)
    {
        var limb = transform.Find(path);
        if (limb == null)
        {
            return;
        }
        Debug.Log("found limb");

        var renderer = limb.GetComponent<SkinnedMeshRenderer>();
        if (renderer == null) return;

        // If this body part is Missing, hide the renderer and return
        // For torso parts the TorsoVariation is used, for arms/legs the Variation is used.
        if ((part.v != null && part.v == BodyParts.Variation.Missing) || (part.tv != null && part.tv == BodyParts.TorsoVariation.Missing))
        {
            // Disable rendering and clear mesh/material to avoid leftover visuals
            renderer.enabled = false;
            renderer.sharedMesh = null;
            renderer.sharedMaterial = null;
            return;
        }

        // Grab filename
        string meshName = GetFileNameFromBodyPart(part);

        // Load the FBX prefab from Resources
        GameObject meshPrefab = Resources.Load<GameObject>($"3D/{meshName}");
        if (meshPrefab == null)
        {
            Debug.LogWarning($"Mesh not found for {meshName}; defaulting to average BLEHHHHHHHHHHHHHHHH");
            return;
        }
        SkinnedMeshRenderer sourceRenderer = meshPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        if (sourceRenderer == null)
        {
            Debug.LogWarning($"No SkinnedMeshRenderer found on {meshName}");
            return;
        }

        // Assign the new mesh and material and ensure renderer is enabled
        renderer.sharedMesh = sourceRenderer.sharedMesh;
        renderer.sharedMaterial = sourceRenderer.sharedMaterial;
        renderer.enabled = true;
    }

    private string GetFileNameFromBodyPart(BodyParts part)
    {
        string meshName = "";
        bool armed = false;
        bool leged = false;
        if (part.v == BodyParts.Variation.Average || part.tv == BodyParts.TorsoVariation.Average)
        {
            switch (part.pt)
            {
                case BodyParts.PartType.Arms:
                    if (!armed)
                    {
                        meshName = "ZombieBaseLeftArm";
                        armed = true;
                    }
                    else
                    {
                        meshName = "ZombieBaseRightArm";
                    }
                    break;
                case BodyParts.PartType.Legs:
                    if (!leged)
                    {
                        meshName = "ZombieBaseLeftLeg";
                        leged = true;
                    }
                    else
                    {
                        meshName = "ZombieBaseRightLeg";
                    }
                    break;
                case BodyParts.PartType.Torso:
                    meshName = "ZombieBaseTorso";
                    break;
            }
        }
        else
        {
            switch (part.pt)
            {
                case BodyParts.PartType.Arms:
                    if (!armed)
                    {
                        meshName = $"SpecialLimbs/{part.v}LeftArm";
                        armed = true;
                    }
                    else
                    {
                        meshName = $"SpecialLimbs/{part.v}RightArm";
                    }
                    break;
                case BodyParts.PartType.Legs:
                    if (!leged)
                    {
                        meshName = $"SpecialLimbs/{part.v}LeftLeg";
                        leged = true;
                    }
                    else
                    {
                        meshName = $"SpecialLimbs/{part.v}RightLeg";
                    }
                    break;
                case BodyParts.PartType.Torso:
                    meshName = "ZombieBaseTorso"; // putting this for now cuz no 3d stuff yet
                    break;
            }
        }
        return meshName;
    }

    private void WhichClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // check if it's clicking the UI
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
            Debug.Log("clicked");
            // click on part, check which one
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
            }

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == corpseLayer)
            {
                Debug.Log(hit.transform.gameObject.name);
                // Debug.Log("clicked on: " + hit.transform.name);
                switch (hit.transform.name)
                {
                    case "Torso":
                        if (PlayerController.instance.heldPart == null && torsoPart == null)
                        {
                            return;
                        }
                        if (PlayerController.instance.heldPart == null)
                        {
                            PlayerController.instance.heldPart = torsoPart;
                            torsoPart = new BodyParts(BodyParts.PartType.Torso, null, BodyParts.TorsoVariation.Missing);
                        }
                        else
                        {
                            // if held part is torso, swap
                            // else, drop torso and instantiate in scene with PlayerController
                            if (PlayerController.instance.heldPart.pt == BodyParts.PartType.Torso)
                            {
                                BodyParts temp = PlayerController.instance.heldPart;
                                PlayerController.instance.heldPart = torsoPart;
                                torsoPart = temp;
                            }
                            else
                            {
                                if (torsoPart.tv == BodyParts.TorsoVariation.Missing) {
                                    return;
                                }
                                DroppedBodyPart drop = null;
                                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                                drop = Instantiate(droppablePrefab.GetComponent<DroppedBodyPart>(), PlayerController.instance.transform.position + randomOffset, Quaternion.identity, PlayerController.instance.transform.parent);
                                drop.Initialise(torsoPart, GetFileNameFromBodyPart(torsoPart));
                                drop.GetComponent<Collider>().enabled = true;
                                torsoPart = new BodyParts(BodyParts.PartType.Torso, null, BodyParts.TorsoVariation.Missing);
                            }
                        }
                        ApplyVisual("ZombieBaseTorso/Torso", torsoPart);
                        break;
                    case "LeftArm":
                        // rArm.SetActive(false);
                        if (PlayerController.instance.heldPart == null && rArmPart == null)
                        {
                            return;
                        }
                        if (PlayerController.instance.heldPart == null)
                        {
                            PlayerController.instance.heldPart = rArmPart;
                            rArmPart = new BodyParts(BodyParts.PartType.Arms, BodyParts.Variation.Missing, null);
                        }
                        else
                        {
                            // if held part is arm, swap
                            // else, drop arm and instantiate in scene with PlayerController
                            if (PlayerController.instance.heldPart.pt == BodyParts.PartType.Arms)
                            {
                                BodyParts temp = PlayerController.instance.heldPart;
                                PlayerController.instance.heldPart = rArmPart;
                                rArmPart = temp;
                            }
                            else
                            {
                                if (rArmPart.v == BodyParts.Variation.Missing) {
                                    return;
                                }
                                DroppedBodyPart drop = null;
                                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                                drop = Instantiate(droppablePrefab.GetComponent<DroppedBodyPart>(), PlayerController.instance.transform.position + randomOffset, Quaternion.identity, PlayerController.instance.transform.parent);
                                drop.Initialise(rArmPart, GetFileNameFromBodyPart(rArmPart));
                                drop.GetComponent<Collider>().enabled = true;
                                rArmPart = new BodyParts(BodyParts.PartType.Arms, BodyParts.Variation.Missing, null);
                            }
                        }
                        ApplyVisual("ZombieBaseRightArm/RightArm", rArmPart);
                        break;
                    case "RightArm":
                        // lArm.SetActive(false);
                        if (PlayerController.instance.heldPart == null && lArmPart == null)
                        {
                            return;
                        }
                        if (PlayerController.instance.heldPart == null)
                        {
                            PlayerController.instance.heldPart = lArmPart;
                            lArmPart = new BodyParts(BodyParts.PartType.Arms, BodyParts.Variation.Missing, null);
                        }
                        else
                        {
                            // if held part is arm, swap
                            // else, drop arm and instantiate in scene with PlayerController
                            if (PlayerController.instance.heldPart.pt == BodyParts.PartType.Arms)
                            {
                                BodyParts temp = PlayerController.instance.heldPart;
                                PlayerController.instance.heldPart = lArmPart;
                                lArmPart = temp;
                            }
                            else
                            {
                                if (lArmPart.v == BodyParts.Variation.Missing) {
                                    return;
                                }
                                DroppedBodyPart drop = null;
                                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                                drop = Instantiate(droppablePrefab.GetComponent<DroppedBodyPart>(), PlayerController.instance.transform.position + randomOffset, Quaternion.identity, PlayerController.instance.transform.parent);
                                drop.Initialise(lArmPart, GetFileNameFromBodyPart(lArmPart));
                                drop.GetComponent<Collider>().enabled = true;
                                lArmPart = new BodyParts(BodyParts.PartType.Arms, BodyParts.Variation.Missing, null);
                            }
                        }
                        ApplyVisual("ZombieBaseLeftArm/LeftArm", lArmPart);
                        break;
                    case "LeftFoot":
                        // rLeg.SetActive(false);
                        if (PlayerController.instance.heldPart == null && rLegPart == null)
                        {
                            return;
                        }
                        if (PlayerController.instance.heldPart == null)
                        {
                            PlayerController.instance.heldPart = rLegPart;
                            rLegPart = new BodyParts(BodyParts.PartType.Legs, BodyParts.Variation.Missing, null);
                        }
                        else
                        {
                            // if held part is arm, swap
                            // else, drop arm and instantiate in scene with PlayerController
                            if (PlayerController.instance.heldPart.pt == BodyParts.PartType.Legs)
                            {
                                BodyParts temp = PlayerController.instance.heldPart;
                                PlayerController.instance.heldPart = rLegPart;
                                rLegPart = temp;
                            }
                            else
                            {
                                if (rLegPart.v == BodyParts.Variation.Missing) {
                                    return;
                                }
                                DroppedBodyPart drop = null;
                                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                                drop = Instantiate(droppablePrefab.GetComponent<DroppedBodyPart>(), PlayerController.instance.transform.position + randomOffset, Quaternion.identity, PlayerController.instance.transform.parent);
                                drop.Initialise(rLegPart, GetFileNameFromBodyPart(rLegPart));
                                drop.GetComponent<Collider>().enabled = true;
                                rLegPart = new BodyParts(BodyParts.PartType.Legs, BodyParts.Variation.Missing, null);
                            }
                        }
                        ApplyVisual("ZombieBaseRightLeg/RightLeg", rLegPart);
                        break;
                    case "RightFoot":
                        // lLeg.SetActive(false);
                        if (PlayerController.instance.heldPart == null && lLegPart == null)
                        {
                            return;
                        }
                        if (PlayerController.instance.heldPart == null)
                        {
                            PlayerController.instance.heldPart = lLegPart;
                            lLegPart = new BodyParts(BodyParts.PartType.Legs, BodyParts.Variation.Missing, null);
                        }
                        else
                        {
                            // if held part is arm, swap
                            // else, drop arm and instantiate in scene with PlayerController
                            if (PlayerController.instance.heldPart.pt == BodyParts.PartType.Legs)
                            {
                                BodyParts temp = PlayerController.instance.heldPart;
                                PlayerController.instance.heldPart = lLegPart;
                                lLegPart = temp;
                            }
                            else
                            {
                                if (lLegPart.v == BodyParts.Variation.Missing) {
                                    return;
                                }
                                DroppedBodyPart drop = null;
                                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                                drop = Instantiate(droppablePrefab.GetComponent<DroppedBodyPart>(), PlayerController.instance.transform.position + randomOffset, Quaternion.identity, PlayerController.instance.transform.parent);
                                drop.Initialise(lLegPart, GetFileNameFromBodyPart(lLegPart));
                                drop.GetComponent<Collider>().enabled = true;
                                lLegPart = new BodyParts(BodyParts.PartType.Legs, BodyParts.Variation.Missing, null);
                            }
                        }
                        ApplyVisual("ZombieBaseLeftLeg/LeftLeg", lLegPart);
                        break;
                }
            }
            // spaghetti code time
            Records.currZombieParts[0] = torsoPart;
            Records.currZombieParts[1] = lArmPart;
            Records.currZombieParts[2] = rArmPart;
            Records.currZombieParts[3] = lLegPart;
            Records.currZombieParts[4] = rLegPart;
            
            if (PlayerController.instance != null)
            {
                if (PlayerController.instance.heldPart == null)
                {
                    return;
                }
                if (PlayerController.instance.heldPart.v == BodyParts.Variation.Missing || PlayerController.instance.heldPart.tv == BodyParts.TorsoVariation.Missing)
                {
                    PlayerController.instance.heldPart = null;
                }
            }
        }
    }
}
