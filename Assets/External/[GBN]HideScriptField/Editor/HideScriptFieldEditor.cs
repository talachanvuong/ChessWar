using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object), true)]
[CanEditMultipleObjects]
public class HideScriptFieldEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "m_Script");
        serializedObject.ApplyModifiedProperties();
    }
}