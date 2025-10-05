using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    public TextMeshProUGUI creditsText;
    public GameObject creditsButton;
    public GameObject devLogButton;
    void Start()
    {
        devLogButton.SetActive(false);
    }
    public void ShowEveryoneWhoWorkedOnThisGame()
    {
        creditsButton.SetActive(false);
        devLogButton.SetActive(true);
        creditsText.text = "THANK YOU SO MUCH FOR PLAYING OUR GAMEEEEE!!!!\n\n" +
            "Here's a whole big ol list of everyone who worked on this project! \n" +
            "zeltig - Team Lead\n" +
            "grepid - Programmer\n" +
            "jamesbuckett\n" +
            "mynameisnotjams\n" +
            "tsumisan18\n" +
            "sabao0184\n" +
            "derpymissile - Programmer\n" +
            "cm220\n" +
            "ebrahimcomps\n" +
            "i9xp2101\n" +
            "YIPPEEEE WAHOOOOOOOOOOOOOO" +
            "It is 1 am local time and I am so so tired";
    }

    public void DevLog()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=F0gMPIqCIdk");
    }
}
