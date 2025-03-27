using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 3;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        lifetime = 5f;
    }

    private int collisionCount = 3;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //collision.gameObject.GetComponent<HealthControl>()?.ReceiveDamage(damage);
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damage);
        }
        if (--collisionCount == 0) Destroy(gameObject);
    }
}