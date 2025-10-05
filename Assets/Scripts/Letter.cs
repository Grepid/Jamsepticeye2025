using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Letter : BaseInteractable
{
    public override void Interact()
    {
        print($"Interacted with {gameObject.name}");
        // read letter again
        Family.instance.ReadLetterAgain();
    }
}
