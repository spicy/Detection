
public class Enemy : Combatant
{
    private void Start()
    {
        health = 100f;
        maxHealth = health;
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}