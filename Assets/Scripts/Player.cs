using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Combatant
{
    private float difficultyModifier = 1f;
    private float regenPerTick = 1f;
    private WaitForSeconds healTick;
    public int currentSceneIndex;
    public int deathScene;

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

        SceneManager.LoadScene(deathScene);
        SceneManager.UnloadSceneAsync(currentSceneIndex);
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