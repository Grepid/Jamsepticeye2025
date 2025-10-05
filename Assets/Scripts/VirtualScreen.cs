using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualScreen : GraphicRaycaster
{
    [Header("References")]
    public Camera screenCamera; // The camera rendering the virtual UI
    public GraphicRaycaster screenCaster; // The raycaster of the virtual UI canvas

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        if (screenCamera == null || screenCamera.targetTexture == null || screenCaster == null)
        {
            Debug.LogWarning($"[VirtualScreen] Missing reference on {gameObject.name}: " +
                             $"screenCamera={screenCamera}, targetTexture={screenCamera?.targetTexture}, screenCaster={screenCaster}");
            return;
        }

        // Use either the eventCamera or fallback to main camera
        Camera cam = eventCamera != null ? eventCamera : Camera.main;
        if (cam == null)
        {
            Debug.LogError("[VirtualScreen] No valid camera available for raycasting!");
            return;
        }

        Ray ray = cam.ScreenPointToRay(eventData.position);
        Debug.Log($"[VirtualScreen] Shooting ray from {cam.name} at {eventData.position}");

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"[VirtualScreen] Raycast hit: {hit.collider.name}");

            if (hit.collider != null && hit.collider.transform == transform)
            {
                Debug.Log("[VirtualScreen] Hit this virtual screen!");
                // your existing virtualPos code here...
            }
            else
            {
                Debug.Log("[VirtualScreen] Hit something else, not this object");
            }
        }
        else
        {
            Debug.Log("[VirtualScreen] Raycast hit nothing at all");
        }

    }
}
