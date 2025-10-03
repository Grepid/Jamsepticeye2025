using UnityEngine;
using System.Collections.Generic;

public class Family : MonoBehaviour
{
    public GameObject zombiePrefab;
    private Zombie zomR;
    private Zombie zomG;
    private bool calledYet = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!calledYet) {
            CallMortician();
        }
    }

    void CallMortician()
    {
        calledYet = true;
        // give new corpse and requirement to player (maybe based on request number in the future)
        // Zombie requirementZom = new Zombie();
        // Zombie givenZom = new Zombie();
        // requirementZom.Initialize(BodyParts.PartType.Arms, vari:BodyParts.Variation.Missing, tvari:null);
        // givenZom.BuildOutRestOfBody(); // <- completely average corpse / zombie
        // duck it spaghetti code time
        zomR = Instantiate(zombiePrefab.GetComponent<Zombie>(), new Vector3(10000, 10000, 10000), Quaternion.identity, this.transform);
        zomR.Initialize(BodyParts.PartType.Arms, vari: BodyParts.Variation.Missing, tvari: null);
        zomG = Instantiate(zombiePrefab.GetComponent<Zombie>(), new Vector3(10000, 10000, 10000), Quaternion.identity, this.transform);
        zomG.BuildOutRestOfBody();

        Records.currReqs = zomR;
        Records.givenCorpse = zomG;

        // build sentences
        List<string> sentences = new List<string>();
        sentences.Add("Dear Funeral Director,");
        sentences.Add("Please prepare my grandpas corpse for today's funeral.");
        sentences.Add("He lost his arm to a sea creature years ago.");
        sentences.Add("What the body has: ");
        foreach (BodyParts part in Records.currReqs.ShowBodyParts())
        {
            sentences.Add(part.pt + " " + part.v + " " + part.tv);
        }

        DialogueManager dm = FindFirstObjectByType<DialogueManager>();
        Debug.Log(dm);
        dm.gameObject.SetActive(true);
        dm.UpdateSentences(sentences.ToArray());
        dm.DisplayNextSentence();
    }
}
