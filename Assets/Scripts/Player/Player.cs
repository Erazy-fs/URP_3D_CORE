using UnityEngine;

public class Player : MonoBehaviour
{
    private Health _health;
    // private Stamina _stamina;
    void Start()
    {
        if (_health == null)
        {
            _health = GetComponent<Health>();
            _health.maxHealth = 100f;
            _health.currentHealth = 100f;
        }

        // if (_stamina == null)
        // {
        //     _stamina = GetComponent<Stamina>();
        // }
    }
    // private bool _isSprinting;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _health.TakeDamage(5);
        }

        // if (Input.GetKey(KeyCode.LeftShift) && _stamina.CanSprint())
        // {
        //     _isSprinting = true;
        //     _stamina.StartSprint();
        // }
        // else
        // {
        //     _isSprinting = false;
        //     _stamina.StopSprint();
        // }

        // if (_isSprinting)
        //     _stamina.DrainStamina(Time.deltaTime);
    }
}
