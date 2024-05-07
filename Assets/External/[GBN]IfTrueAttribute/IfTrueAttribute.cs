using UnityEngine;
using System;

/// <summary>
/// If true show value
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class IfTrueAttribute : PropertyAttribute
{
    public string boolName { get; private set; }

    /// <summary>
    /// Make sure you text right value 
    /// </summary>
    /// <param name="boolName">Boolen value need to check</param>
    public IfTrueAttribute(string boolName)
    {
        this.boolName = boolName;
    }
}
