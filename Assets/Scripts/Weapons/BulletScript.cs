// ==============================
// BulletScript.cs
// Controls bullet movement, lifetime, and collision with enemies or obstacles
// ==============================
using Systems;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Lifetime & Speed")]
    [SerializeField] private float _lifeTime = 3f; // Time before the bullet is destroyed automatically
    [SerializeField] private float _bulletSpeed = 20f; // Speed of the bullet

    [Header("Damage")]
    [SerializeField] private int _damage = 40; // Damage dealt to enemies on impact
    private void Start()
    {
        // Automatically destroy the bullet after a set lifetime
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        // Move the bullet forward in its local right direction
        transform.Translate(Vector2.right * _bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore collisions with the player
        if (other.CompareTag("Player"))
            return;

        // Deal damage to enemies on collision
        if (other.CompareTag("Enemy"))
        {
            
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
            }
            
            // Destroy bullet on impact
            Destroy(gameObject);
        }
        else if (other.isTrigger == false)
        {
            // Destroy bullet if it hits a non-trigger obstacle or wall
            Destroy(gameObject);
        }
    }
}
