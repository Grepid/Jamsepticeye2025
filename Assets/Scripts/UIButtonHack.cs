using UnityEngine;
using UnityEngine.UI;

public class UIButtonHack : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            // SceneChanger.instance.ChangeScene("MGame1EmergencyTest");
            SceneChanger.instance.RemoveScene("Operation");
        });
    }
}
