using UnityEngine;
using System;

public class Stamina : MonoBehaviour
{
    public event Action<float> OnStaminaChanged;
    [SerializeField] public float maxStamina = 100f;
    [SerializeField] public float sprintDrain = 30f;  // Сколько стамины уходит в секунду
    [SerializeField] public float staminaRegen = 20f; // Скорость восстановления в секунду

    private float _currentStamina;
    private bool _isSprinting;

    private void Awake()
    {
        _currentStamina = maxStamina;
    }

    private void Update()
    {
        if (!_isSprinting)
        {
            RecoverStamina();
        }
    }

    public bool CanSprint()
    {
        return _currentStamina > 0;
    }

    public void StartSprint()
    {
        _isSprinting = true;
    }

    public void StopSprint()
    {
        _isSprinting = false;
    }

    public void DrainStamina(float deltaTime)
    {
        if (_isSprinting && _currentStamina > 0)
        {
            _currentStamina -= sprintDrain * deltaTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
            OnStaminaChanged?.Invoke(_currentStamina / maxStamina);
        }
    }

    private void RecoverStamina()
    {
        if (_currentStamina < maxStamina)
        {
            _currentStamina += staminaRegen * Time.deltaTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
            OnStaminaChanged?.Invoke(_currentStamina / maxStamina);
        }
    }
}
