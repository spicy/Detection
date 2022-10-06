using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWeaponCollisionWithPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Ignore the collisions between layer 11 (weapons) and layer 10 (player)
        Physics.IgnoreLayerCollision(11, 10);
    }
}
