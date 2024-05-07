#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IfTrueAttribute))]
public class IfTruePropertyDrawer : PropertyDrawer
{
    IfTrueAttribute ifTrue;
    SerializedProperty boolName;
    float height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ifTrue = attribute as IfTrueAttribute;
        boolName = property.serializedObject.FindProperty(ifTrue.boolName);

        if (boolName == null)
        {
            Debug.LogError($"{"[".Bold()}{ifTrue.boolName.Rainbow()}{"]".Bold()} not {"found".BoldColor("red")}");
            return;
        }

        if (boolName.boolValue == true)
        {
            height = base.GetPropertyHeight(property, label);
            EditorGUI.PropertyField(position, property);
        }
        else
        {
            height = 0.0f;
        }
    }
}
#endif