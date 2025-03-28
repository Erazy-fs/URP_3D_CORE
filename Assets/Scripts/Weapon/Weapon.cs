using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    public int CurrentAmmoCount { get; set; }
    public int MaxAmmoCount { get; set; }
    public event Action<int> OnCurrentAmmoChanged;
    public Weapon()
    {
        MaxAmmoCount = 30;
        CurrentAmmoCount = 30;
    }

    public void AmmoCount(int bullets)
    {
        CurrentAmmoCount += bullets;
        OnCurrentAmmoChanged?.Invoke(CurrentAmmoCount);
    }

    public void Fire()
    {
        AmmoCount(-1);
    }
}
