using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class Setting : ScriptableObject
{
    public abstract SettingType SettingType { get; set; }
    public abstract object Value { get; set; }
    public virtual List<object> Options { get; } = new();
    public abstract void Load();
    public abstract void Save();
    public abstract void Cancel();
}
