using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class EndRound : MonoBehaviour
{
    public GameObject endPanel;
    public TextMeshProUGUI endText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndScreen()
    {
        endPanel.SetActive(true);
        //endText.text = $"Congratulations on finishing round {Records.requestNum + 1}!\nYou have successfully completed {Records.successfulReqs} out of {Records.requestNum + 1} requests!";
        endText.text = "Comparing with requirements...";
        StartCoroutine(DramaticPause());
    }

    IEnumerator DramaticPause()
    {
        int finalScore = 0;
        yield return new WaitForSeconds(1);
        endText.text = "Comparing with requirements....";
        yield return new WaitForSeconds(1);
        endText.text = "Comparing with requirements.....";
        yield return new WaitForSeconds(1);
        endText.text = "Comparing with requirements......";
        // compare
        foreach (BodyParts part in Records.currReqs.ShowBodyParts())
        {
            for (int i=0; i<Records.currZombieParts.Length; ++i) {
                BodyParts p = Records.currZombieParts[i];
                if (p==null) {
                    continue;
                }
                if (p.pt == part.pt)
                {
                    if (p.pt == BodyParts.PartType.Torso)
                    {
                        if (p.tv == part.tv)
                        {
                            // it's a match
                            finalScore++;
                            Records.currZombieParts[i] = null;
                        }
                    }
                    else
                    {
                        if (p.v == part.v)
                        {
                            finalScore++;
                            Records.currZombieParts[i] = null;
                        }
                    }
                }
            }
        }

        // debug
        foreach (BodyParts part in Records.currReqs.ShowBodyParts())
        {
            Debug.Log("Required body parts:");
            Debug.Log(part.pt + " " + part.v + "" + part.tv);
        }
        foreach (BodyParts part in Records.currZombieParts)
        {
            Debug.Log("Current body parts:");
            if (part == null) {
                continue;
            }
            Debug.Log(part.pt + " " + part.v + "" + part.tv);
        }
        // debug end

        endText.text = $"Body Parts matched up: {finalScore}/5";
        switch (finalScore)
        {
            case 0:
                endText.text += "\nHorrible Job!";
                break;
            case 1:
                endText.text += "\nOugh.";
                break;
            case 2:
                endText.text += "\nhmmmmmm";
                break;
            case 3:
                endText.text += "\nYou're not going bankrupt yet!";
                break;
            case 4:
                endText.text += "\nYou would make uncle proud!";
                break;
            case 5:
                endText.text += "\nYour did it";
                break;
        }

        yield return new WaitForSeconds(6);

        SceneChanger.instance.ChangeScene("Credits");
    }
}
