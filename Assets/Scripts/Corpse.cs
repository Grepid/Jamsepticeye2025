using UnityEngine;

public class Corpse : MonoBehaviour
{
    private BodyParts[] parts = new BodyParts[5];
    public LayerMask corpseLayer;
    public GameObject lArm;
    public GameObject rArm;
    public GameObject lLeg;
    public GameObject rLeg;
    public GameObject torso;
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

    private void WhichClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // click on part, check which one
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == corpseLayer)
            {
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
