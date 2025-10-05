using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MollieJones : BaseInteractable
{
    public override void Interact()
    {
        print($"Interacted with {gameObject.name}");
        // pull up the camera with the 3D model of the body parts
        Records.playerLastPos = PlayerController.instance.transform.position;
        // SceneChanger.instance.ChangeScene("Operation");
        // lets try this one more time
        SceneChanger.instance.AddScene("Operation");
    }
}
