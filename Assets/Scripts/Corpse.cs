using UnityEngine;
using TMPro;

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
    public TextMeshProUGUI currentHold;
    public Camera cam;

    private void Awake()
    {
        instance = this;
        parts = new BodyParts[5];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        corpseLayer = LayerMask.NameToLayer("Corpse");
    }

    // Update is called once per frame
    void Update()
    {
        WhichClicked();
    }

    public void Initialize() {
        foreach (BodyParts part in Records.givenCorpse.ShowBodyParts())
        {
            switch (part.pt)
            {
                case BodyParts.PartType.Torso:
                    torsoPart = part;
                    break;
                case BodyParts.PartType.Arms:
                    if (rArmPart == null)
                    {
                        rArmPart = part;
                    }
                    else
                    {
                        lArmPart = part;
                    }
                    break;
                case BodyParts.PartType.Legs:
                    if (rLegPart == null)
                    {
                        rLegPart = part;
                    }
                    else
                    {
                        lLegPart = part;
                    }
                    break;
            }
        }
    }

    private void WhichClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicked");
            // click on part, check which one
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.transform.name);
            }

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == corpseLayer)
            {
                Debug.Log(hit.transform.gameObject.name);
                // Debug.Log("clicked on: " + hit.transform.name);
                switch (hit.transform.name)
                {
                    case "Torso":
                        torso.SetActive(false);
                        break;
                    case "RightArm":
                        rArm.SetActive(false);
                        break;
                    case "LeftArm":
                        lArm.SetActive(false);
                        break;
                    case "RightFoot":
                        rLeg.SetActive(false);
                        break;
                    case "LeftFoot":
                        lLeg.SetActive(false);
                        break;
                }
            }
        }
    }
}
