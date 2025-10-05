using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(CameraPivoter))]
public class CameraPivoterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Face Target"))
        {
            CameraPivoter t = (CameraPivoter)target;
            t.PointToTarget();
        }
    }
}
#endif

public class CameraPivoter : MonoBehaviour
{
    [SerializeField] private Transform Lookat;

    
    public void PointToTarget()
    {
        if(Lookat == null)
        {
            Debug.LogError("Lookat is null");
            return;
        }
        transform.LookAt(Lookat);
    }
}
