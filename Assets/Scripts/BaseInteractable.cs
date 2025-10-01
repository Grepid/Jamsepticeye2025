using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseInteractable : MonoBehaviour
{
    [SerializeField]private string _name;
    [SerializeField]private string _popupMessage;
    public string Name
    {
        get
        {
            return _name;
        }
        private set
        {
            _name = value;
        }
    }

    public string PopupMessage
    {
        get
        {
            return _popupMessage;
        }
        set
        {
            _popupMessage = value;
        }
    }
    public abstract void Interact();
}
