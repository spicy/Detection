using UnityEngine;
namespace Detection
{
    public interface ITakeDamage
    {
        public void TakeDamage(float damage);
        public void Die();
    }
}
