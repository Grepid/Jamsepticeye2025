using UnityEngine;

public class CorpseInteract : BaseInteractable
{
    public override void Interact()
    {
        print($"Interacted with {gameObject.name}");
        Time.timeScale = 0;
        // pull up screen for operating on the body
    }
}
