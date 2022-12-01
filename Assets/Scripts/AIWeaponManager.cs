using Detection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponManager : MonoBehaviour
{
    public struct NecessaryUseConditions
    {
        public int minRange;
        public int maxRange;
    }

    [SerializeField] private NecessaryUseConditions currentWeaponNecessaryUseConditions;
    private IDealsDamage dealsDamage;
    private IHasAIBehavior aiSpecificBehavior;

    public void Awake()
    {
        dealsDamage = GetComponentInChildren<IDealsDamage>();
        aiSpecificBehavior = GetComponentInChildren<IHasAIBehavior>();
    }

    public NecessaryUseConditions GetWeaponNecessaryUseConditions()
    {
        return currentWeaponNecessaryUseConditions;
    }

    public void DoAttack()
    {
        if (aiSpecificBehavior != null)
        {
            aiSpecificBehavior.DoAIBehavior();
        }   
        else
        {
            dealsDamage.Attack();
        }
    }


}
