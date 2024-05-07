#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TitleAttribute))]
public class TitleDecoratorDrawer : DecoratorDrawer
{
    TitleAttribute title;
    GUIStyle style = new GUIStyle();
    Color backgroundColor;

    public override void OnGUI(Rect position)
    {
        title = attribute as TitleAttribute;
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;

        ColorUtility.TryParseHtmlString(title.backgroundColor, out backgroundColor);

        EditorGUI.DrawRect(position, backgroundColor);
        EditorGUI.LabelField(position, $"[{title.titleName}]".Hightlight(title.titleColor), style);
    }
}
#endif