using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PopupUI : MonoBehaviour
{
    public float TimeBeforeAppearing = 1f;
    [Tooltip("Offset in X and Y positions assuming the mouse is in the top left of the screen, so the UI is on the bottom right corner of the mouse")]
    public Vector2 Offset;
    public RectTransform body;

    //[HideInInspector]public Sprite PopupImage;
    //[HideInInspector]public string PopupMessage;

    [SerializeField] private Image _popupImage;
    [SerializeField] private TextMeshProUGUI _popupText;


    public void Initialise(Sprite image,string message)
    {
        if(image != null)_popupImage.sprite = image;
        _popupText.text = message;
    }

    private bool isLeftSide => Input.mousePosition.x < Screen.width / 2f;
    private bool isTopSide => Input.mousePosition.y > Screen.height / 2f;

    private Vector2 desiredPos
    {
        get
        {
            Vector2 desiredPos = Input.mousePosition;

            Vector2 padding = Offset;
            padding.x *= isLeftSide ? 1 : -1;
            padding.y *= isTopSide ? 1 : -1;

            desiredPos += padding;
            return desiredPos;
        }
    }

    private Vector2 desiredPivot
    {
        get
        {
            Vector2 pivot = new Vector2();
            pivot.x = isLeftSide ? 0 : 1;
            pivot.y = isTopSide ? 1 : 0;
            return pivot;
        }
    }

    public void Awake()
    {
        AdjustPosition();
    }


    public void Update()
    {
        AdjustPosition();
    }

    private void AdjustPosition()
    {
        body.pivot = desiredPivot;
        body.position = desiredPos;
    }

    public static PopupUI CreatePopupUI()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        if (canvas == null) return null;
        PopupUI result = Instantiate(PlayerController.instance.intSys.PopupUIPrefab, canvas);
        return result;
    }
}
