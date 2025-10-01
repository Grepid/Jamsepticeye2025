using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TestInteractable : BaseInteractable
{
    public override void Interact()
    {
        print($"Interacted with {gameObject.name}");
    }
}
