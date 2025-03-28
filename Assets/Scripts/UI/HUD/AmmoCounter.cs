using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{   
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private Weapon _weapon;

    void Start()
    {
        _weapon.OnCurrentAmmoChanged += UpdateAmmoCount;
        _ammoText.text = $"{_weapon.CurrentAmmoCount} / {_weapon.MaxAmmoCount}";
    }

    public void UpdateAmmoCount(int currentAmmoCount)
    {
        _ammoText.text = $"{currentAmmoCount} / {_weapon.MaxAmmoCount}";
    }
}
