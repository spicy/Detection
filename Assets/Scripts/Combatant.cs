using UnityEngine;
using Detection;

public class Combatant : MonoBehaviour, ITakeDamage
{
    protected float health;
    protected float maxHealth;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    public virtual void Die()
    {

    }
}
