// ==============================
// EnemyAttackHandler.cs
// Controlador central de ataques del enemigo, maneja ataques melee y a distancia según el tipo configurado
// ==============================

using UnityEngine;

namespace Enemies
{
    // Este componente requiere que el GameObject tenga un EnemyController adjunto.
    // Se encarga de ejecutar los ataques del enemigo según su tipo principal (melee o a distancia).
    [RequireComponent(typeof(EnemyController))]
    public class EnemyAttackHandler : MonoBehaviour
    {
        // Referencia al controlador del enemigo (EnemyController)
        private EnemyController _enemy;

        // Temporizador para controlar el tiempo entre ataques
        private float _cooldownTimer;

        // Bandera para evitar múltiples activaciones del hitbox melee
        private bool _isMeleeActive;
        
        private EnemyAnimation _enemyAnimation;



        private void Awake()
        {
            // Se obtiene la referencia al EnemyController al inicio
            _enemy = GetComponent<EnemyController>();
            _enemyAnimation = GetComponent<EnemyAnimation>();
        }

        private void Update()
        {
            // Si no hay objetivo asignado, no se realiza ningún ataque
            if (_enemy.Target == null) return;

            // Calcula la distancia al objetivo (jugador)
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.Target.position);

            // Si el jugador está fuera del alcance de ataque, se desactiva el ataque melee
            if (distance > _enemy.AttackDistance)
            {
                DeactivateMelee();
                return;
            }

            // Incrementa el temporizador de cooldown
            _cooldownTimer += Time.deltaTime;

            // Ataca con proyectiles si el tipo de ataque es a distancia
            if (_enemy.PrimaryAttackType == AttackType.Range)
            {
                if (_cooldownTimer >= _enemy.DamageCooldown)
                {
                    _enemyAnimation?.PlayAttack();
                    
                    _cooldownTimer = 0f; // Reinicia el cooldown tras disparar
                }
            }
            // Ataca con hitbox si el tipo de ataque es melee
            else if (_enemy.PrimaryAttackType == AttackType.Melee)
            {
                if (_cooldownTimer >= _enemy.DamageCooldown)
                {
                    _enemyAnimation?.PlayAttack();
                    
                    _cooldownTimer = 0f; // Reinicia el cooldown tras activar el ataque
                }
            }
        }

        // Dispara un proyectil hacia el jugador
        private void FireProjectile()
        {
            if (_enemy.ProjectilePrefab == null || _enemy.Target == null) return;

            // Dirección hacia el jugador
            Vector2 direction = (_enemy.Target.position - _enemy.transform.position).normalized;
            if (direction == Vector2.zero) return;

            // Posición de aparición del proyectil ligeramente frente al enemigo
            Vector3 spawnPos = _enemy.transform.position + (Vector3)direction * 0.5f;

            // Instancia el proyectil
            GameObject proj = Instantiate(_enemy.ProjectilePrefab, spawnPos, Quaternion.identity);
            var projectile = proj.GetComponent<EnemyProjectile>();

            if (projectile != null)
            {
                projectile.SetDirection(direction);
                projectile.Speed = _enemy.ProjectileSpeed;
                projectile.Damage = _enemy.DamageAmount;

                // Evita colisión entre el enemigo y su propio proyectil
                Collider2D projCol = proj.GetComponent<Collider2D>();
                Collider2D enemyCol = _enemy.GetComponent<Collider2D>();
                if (projCol != null && enemyCol != null)
                    Physics2D.IgnoreCollision(projCol, enemyCol);
            }
        }

        // Activa el hitbox del ataque melee por una pequeña ventana de tiempo
        private void ActivateMelee()
        {
            if (_enemy.MeleeHitbox != null && !_isMeleeActive)
            {
                _enemy.MeleeHitbox.gameObject.SetActive(true);
                _isMeleeActive = true;

                // Se desactiva automáticamente tras 0.2 segundos
                Invoke(nameof(DeactivateMelee), 0.2f);
            }
        }

        // Desactiva el hitbox del ataque melee
        private void DeactivateMelee()
        {
            if (_enemy.MeleeHitbox != null && _isMeleeActive)
            {
                _enemy.MeleeHitbox.gameObject.SetActive(false);
                _isMeleeActive = false;
            }
        }
    }
}