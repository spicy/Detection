using UnityEngine;

public class Enemy : Combatant
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        health = 100f;
        maxHealth = health;
        EnableRagDoll(false);
    }

    public override void Die()
    {
        animator.enabled = false;
        EnableRagDoll(true);

        // When the enemy goes ragdoll, the weapons fly everywhere
        // Working on fixing that after all prefabs are done
        
        //Destroy(gameObject);
    }

    private void EnableRagDoll(bool state)
    {
        SetRigidBodyKinematic(!state);
        SetColliders(state);
    }

    private void SetRigidBodyKinematic(bool newState)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = newState;
        }
        // main rigidbody opposite value
        GetComponent<Rigidbody>().isKinematic = !newState;
    }

    private void SetColliders(bool newState)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = newState;
        }
        // main collider opposite value
        GetComponent<Collider>().enabled = !newState;
    }
}