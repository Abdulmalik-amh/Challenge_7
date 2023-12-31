using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationEvents : MonoBehaviour
{
    DamageDealer damageDealer;
    PlayerInfo playerInfo;
    CombatScript combatScript;

    void Start()
    {
        damageDealer = GetComponentInChildren<DamageDealer>();
        playerInfo = GetComponent<PlayerInfo>();
        combatScript = GetComponent<CombatScript>();
    }

    public void StartDealDamage()
    {
        damageDealer.StartDealDamage();
    }

    public void EndDealDamage()
    {
        damageDealer.EndDealDamage();
    }
}
