using System;
using UnityEngine;

public interface IWeapon
{
    public int CurrentAmmoCount {get;set;}
    public int MaxAmmoCount {get;set;}
    public event Action<int> OnCurrentAmmoChanged;
}
