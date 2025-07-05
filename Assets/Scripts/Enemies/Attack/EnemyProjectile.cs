// ==============================
// EnemyProjectile.cs
// Controla el comportamiento del proyectil enemigo: movimiento, daño y colisión
// ==============================

using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int Damage; // Cantidad de daño que inflige el proyectil
    public float Speed; // Velocidad del proyectil
    private Vector2 _direction; // Dirección en la que se moverá el proyectil

    // Tiempo máximo que el proyectil estará activo antes de autodestruirse
    [SerializeField] private float _lifetime = 3f;

    // Asigna la dirección del proyectil desde el exterior
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    void Start()
    {
        // Programa la autodestrucción del proyectil después de cierto tiempo
        Destroy(gameObject, _lifetime);
    }

    void Update()
    {
        // Mueve el proyectil en la dirección asignada
        transform.Translate(_direction * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con el jugador, aplica daño y se destruye
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<PlayerController.PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
