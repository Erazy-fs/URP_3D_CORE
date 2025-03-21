using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthChanged;

    [SerializeField] public float maxHealth = 100f;
    public float currentHealth = 100f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        // if (_currentHealth <= 0)
        // {
        //     Debug.Log("Игрок погиб!");
        //     Destroy(gameObject); // Удаление объекта при смерти
        // }
    }
}
