using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseInteractable : MonoBehaviour
{
    [SerializeField]private string _name;
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

    public abstract void Interact();
}
