using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Stamina _playerStamina;
    [SerializeField] private Image _staminaFill;

    private void Start()
    {
        if (_playerStamina is not null)
            _playerStamina.OnStaminaChanged += UpdateStaminaBar;
    }

    private void UpdateStaminaBar(float staminaPercent)
    {
        _staminaFill.fillAmount = staminaPercent;
    }
}
