using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationEvents : MonoBehaviour
{
    DamageDealer damageDealer;
    PlayerInfo playerInfo;
    CombatScript combatScript;
    KickHandler KickHandler;

    void Start()
    {
        damageDealer = GetComponentInChildren<DamageDealer>();
        playerInfo = GetComponent<PlayerInfo>();
        combatScript = GetComponent<CombatScript>();
        KickHandler = GetComponentInChildren<KickHandler>();
    }



    public void StartDealDamage()
    {
        damageDealer.StartDealDamage();
    }

    public void EndDealDamage()
    {
        damageDealer.EndDealDamage();
    }

    public void EndAttack()
    {
        combatScript.EnableAttack();
    }

    public void StartKick()
    {
        KickHandler.EndDealDamage();
        Debug.Log("startkick");
    }

    public void EndKick()
    {
        KickHandler.EndDealDamage();
        Debug.Log("endkick");
    }
}
