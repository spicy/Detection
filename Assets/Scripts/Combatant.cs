using Detection;
using UnityEngine;

public class Combatant : MonoBehaviour, ITakeDamage
{
    [SerializeField] float health;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
