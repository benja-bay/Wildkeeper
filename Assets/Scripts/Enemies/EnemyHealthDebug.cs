// #TEST

using Systems;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealthDebug : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Health _health;

        public Color colorStart = Color.white;
        public Color colorEnd = Color.red;

        [Range(0, 100)]
        public int percentage = 0;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _health = GetComponent<Health>();
        }

        void Update()
        {
            if (_health.MaxHealth == 0) return;

            float t = (float)_health.CurrentHealth / _health.MaxHealth;
            _spriteRenderer.color = Color.Lerp(colorStart, colorEnd, t);
        }
    }
}
