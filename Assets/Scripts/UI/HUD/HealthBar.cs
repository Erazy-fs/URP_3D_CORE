using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Image _healthFill;

    private void Start()
    {
        if (_playerHealth is not null)
            _playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(float healthPercent)
    {
        _healthFill.fillAmount = healthPercent;
    }
}
