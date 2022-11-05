using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class HealthPickup : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player == null) return;

        player.InstantHeal();
        Destroy(gameObject);
    }
}
