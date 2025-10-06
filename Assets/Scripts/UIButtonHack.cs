using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtonHack : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI playerInvText;
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            // SceneChanger.instance.ChangeScene("MGame1EmergencyTest");
            SceneChanger.instance.RemoveScene("Operation");
        });
    }
    void Update()
    {
        if (PlayerController.instance != null)
        {
            Debug.Log(PlayerController.instance);
            if (PlayerController.instance.heldPart == null) {
                playerInvText.text = $"Currently Holding:\nNothing";
            }else if (PlayerController.instance.heldPart.pt == BodyParts.PartType.Torso)
            {
                playerInvText.text = $"Currently Holding:\n{PlayerController.instance.heldPart.tv} {PlayerController.instance.heldPart.pt}";
            }
            else
            {
                playerInvText.text = $"Currently Holding:\n{PlayerController.instance.heldPart.v} {PlayerController.instance.heldPart.pt}";
            }
        }
        else
        {
            Debug.Log("how tf do you reference playercontroller");
        }
    }
}
