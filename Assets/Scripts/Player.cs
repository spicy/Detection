using System.Collections;
using UnityEngine;

public class Player : Combatant
{
    private float difficultyModifier = 1f;
    private float regenPerTick = 1f;
    private WaitForSeconds healTick;

    private void Start()
    {
        health = 200 * difficultyModifier;
        maxHealth = health;
        healTick = new WaitForSeconds(1f);

        StartCoroutine(RegenOverTime());
    }

    public override void Die()
    {
        StopCoroutine(RegenOverTime());

        Debug.Log("Dead");
    }

    public void InstantHeal()
    {
        health = maxHealth;
    }

    private IEnumerator RegenOverTime()
    {
        while(true)
        {
            if(health < maxHealth)
                health = Mathf.Clamp(health + regenPerTick, health, maxHealth);

            yield return healTick;
        }
    }
}