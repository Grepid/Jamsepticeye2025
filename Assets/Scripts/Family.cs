using UnityEngine;
using System.Collections.Generic;

public class Family : MonoBehaviour
{
    // Note for myself on what Family does (and when it triggers)
    // Triggers when you interact with the Letter
    // First time it activates per round, it spawns the zoms
    // Next time it activates, time freezes and you just read
    public GameObject zombiePrefab;
    private Zombie zomR;
    private Zombie zomG;
    private int roundStart = -1;
    private List<string> currSentence = new List<string>();
    public static Family instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (roundStart != Records.requestNum)
        {
            CallMortician();
            roundStart = Records.requestNum;
        }
    }

    void CallMortician()
    {
        zomR = Instantiate(zombiePrefab.GetComponent<Zombie>(), new Vector3(10000, 10000, 10000), Quaternion.identity, this.transform);
        zomR.Initialize(BodyParts.PartType.Arms, vari: BodyParts.Variation.Pirate, tvari: null);
        zomG = Instantiate(zombiePrefab.GetComponent<Zombie>(), new Vector3(10000, 10000, 10000), Quaternion.identity, this.transform);
        zomG.BuildOutRestOfBody();

        Records.currReqs = zomR;
        Records.givenCorpse = zomG;

        // no further use; delete em so that errors don't clog up build
        Destroy(zomR.gameObject);
        Destroy(zomG.gameObject);

        // build currSentence
        currSentence.Clear();
        currSentence.Add("Dear Funeral Director,");
        currSentence.Add("Please prepare my grandpas corpse for today's funeral.");
        currSentence.Add("He lost his arm to a sea creature years ago. And then gained a pirate arm.");
        currSentence.Add("What the body has: ");
        foreach (BodyParts part in Records.currReqs.ShowBodyParts())
        {
            currSentence.Add(part.pt + " " + part.v + " " + part.tv);
        }

        DialogueManager dm = FindFirstObjectByType<DialogueManager>();
        Debug.Log(dm);
        dm.gameObject.SetActive(true);
        dm.UpdateSentences(currSentence.ToArray());
        dm.DisplayNextSentence();
        Records.initBodyArray = false;

        // Corpse.instance.Initialize();
    }

    public void ReadLetterAgain()
    {
        DialogueManager dm = FindFirstObjectByType<DialogueManager>();
        dm.gameObject.SetActive(true);
        dm.UpdateSentences(currSentence.ToArray());
        dm.DisplayNextSentence();
    }
}
