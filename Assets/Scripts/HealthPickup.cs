using UnityEngine;

[RequireComponent (typeof(Collider))]
public class HealthPickup : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.LookAt(player.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player == null) return;

        player.InstantHeal();
        Destroy(gameObject);
    }
}
