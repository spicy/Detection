using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class SpeedPickup : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.2f;
    [SerializeField] private float duration = 5f;
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
        ActionBasedContinuousMoveProvider provider = other.transform.GetComponent<ActionBasedContinuousMoveProvider>();
        if (provider == null) return;

        StartCoroutine(SpeedBoost(provider));
    }

    private IEnumerator SpeedBoost(ActionBasedContinuousMoveProvider provider)
    {
        provider.moveSpeed *= speedMultiplier;

        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(duration);

        provider.moveSpeed /= speedMultiplier;
        Destroy(gameObject);
    }
}
