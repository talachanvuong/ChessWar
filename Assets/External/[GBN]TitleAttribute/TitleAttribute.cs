using UnityEngine;
using System;

/// <summary>
/// Write a title, easier for organize
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public class TitleAttribute : PropertyAttribute
{
    public string titleName { get; private set; }
    public string titleColor { get; private set; }
    public string backgroundColor { get; private set; }

    public TitleAttribute(string titleName, string titleColor, string backgroundColor)
    {
        this.titleName = titleName;
        this.titleColor = titleColor;
        this.backgroundColor = backgroundColor;
    }

    public TitleAttribute(string titleName)
    {
        this.titleName = titleName;
        this.titleColor = "#000000";
        this.backgroundColor = "#FDEFF4";
    }
}