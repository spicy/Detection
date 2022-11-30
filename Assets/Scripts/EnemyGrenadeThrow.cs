using System.Collections;
using UnityEngine;

public class EnemyGrenadeThrow : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    private Grenade grenade;
    private GameObject throwGrenade;
    private float gravity = Physics.gravity.magnitude;
    private WeaponInverseKinematics weaponIk;
    public Transform grenadeSpawnPoint;
    public float throwAngle = 45f;
    
    private void Start()
    {
        grenade = GetComponentInChildren<Grenade>();
        weaponIk = GetComponent<WeaponInverseKinematics>();
    }

    // This function is called when the grenade throw animation is played.
    // This function uses physics to launch the grenade. Grenade keeps moving after reaching target position
    public void Throw()
    {
        if (weaponIk.TargetTransform == null) return;

        GameObject newGrenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation, null);
        Physics.IgnoreCollision(newGrenade.GetComponent<Collider>(), transform.GetComponent<Collider>());
        Destroy(grenade.gameObject);
        newGrenade.GetComponentInChildren<GrenadeRing>().isConnected = false;

        // Throw direction
        Vector3 dirToThrow = -(grenadeSpawnPoint.position - weaponIk.TargetTransform.position);

        float temp = dirToThrow.y;
        dirToThrow.y = 0;

        // XZ length and angle of throw to radians
        float length = dirToThrow.magnitude;
        float theta = throwAngle * Mathf.Deg2Rad;

        // height of throw
        dirToThrow.y = length * Mathf.Tan(theta);
        length += temp / Mathf.Tan(theta);

        // force required to throw at distance
        float throwForce = Mathf.Sqrt(length * Physics.gravity.magnitude / Mathf.Sin(2 * theta));
        Vector3 velocity = throwForce * dirToThrow.normalized;

        // Addforce to grenade and launch
        newGrenade.GetComponent<Rigidbody>().AddRelativeForce(velocity, ForceMode.Impulse);
    }

    // This function directly moves the transform of the grenade. The grenade stops when it reaches the target position
    IEnumerator ThrowRoutine()
    {
        throwGrenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation, null);
        Physics.IgnoreCollision(throwGrenade.GetComponent<Collider>(), transform.GetComponent<Collider>());
        Destroy(grenade.gameObject);
        throwGrenade.GetComponentInChildren<GrenadeRing>().isConnected = false;

        // Projectile motion: simulating gravity, ignoring physics
        float distance = Vector3.Distance(throwGrenade.transform.position, weaponIk.TargetTransform.position);

        float velocity = distance / (Mathf.Sin(2 * throwAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(velocity) * Mathf.Cos(throwAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(velocity) * Mathf.Sin(throwAngle * Mathf.Deg2Rad);

        // time to target
        float duration = distance / Vx;

        throwGrenade.transform.rotation = Quaternion.LookRotation(weaponIk.TargetTransform.position - throwGrenade.transform.position);

        // time movement of grenade
        float t = 0f;
        while(t < duration)
        {
            throwGrenade.transform.Translate(0, (Vy - (gravity * t)) * Time.deltaTime, Vx * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
    }
}