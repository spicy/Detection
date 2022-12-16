using UnityEngine;

public class Enemy : Combatant
{
    private Animator animator;
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    private AIWeaponManager weaponManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        weaponManager = GetComponent<AIWeaponManager>();
    }

    private void Start()
    {
        health = 100f;
        maxHealth = health;
        EnableRagDoll(false);
    }

    public override void Die()
    {
        // Launch weapon towards player or drop on ground if not targeting player
        weaponManager.LaunchWeapon();

        animator.enabled = false;
        EnableRagDoll(true);

        Destroy(gameObject, 1f);
    }

    private void EnableRagDoll(bool state)
    {
        SetRigidBodyKinematic(!state);
        SetColliders(state);
    }

    private void SetRigidBodyKinematic(bool newState)
    {
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = newState;
        }
        // main rigidbody opposite value
        GetComponent<Rigidbody>().isKinematic = !newState;
    }

    private void SetColliders(bool newState)
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = newState;
        }
        // main collider opposite value
        GetComponent<Collider>().enabled = !newState;
    }
}