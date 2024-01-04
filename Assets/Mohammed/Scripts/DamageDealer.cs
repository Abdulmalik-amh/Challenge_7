using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    PlayerInfo playerInfo;
    CombatScript combatScript;
    EnemyLockOn enemyLockOn;

    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();

        playerInfo = GetComponentInParent<PlayerInfo>();
        combatScript = GetComponentInParent<CombatScript>();
        enemyLockOn = GetComponent<EnemyLockOn>();
    }

    void Update()
    {
        DealAttack();
    }

    public void DealAttack()
    {
        if (canDealDamage)
        {
            RaycastHit hit;
            int ShieldMask = (1 << 12);
            // Combine layers 9 and 10 in the layer mask
            int layerMask = (1 << 10) + (1 << 9);
            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, ShieldMask))
            {
                if (hit.transform.TryGetComponent(out CombatScript Aenemy))
                {
                    // Handle the case where the hit object has the "Shield" tag
                    Debug.Log("Hit a block shield!");
                    Aenemy.getBlocked();
                    combatScript.CancleAttack();
                    combatScript.HitVFX(hit.point);

                    // You may want to add further logic for shield interactions here
                }
            }
            else if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                    // Check if the hit object is an enemy and hasn't received damage yet
                    if (hit.transform.TryGetComponent(out PlayerInfo Benemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                    {
                        Benemy.TakeDamage(weaponDamage);
                        Debug.Log("Damage dealt: " + weaponDamage);
                        Benemy.HitVFX(hit.point);
                        hasDealtDamage.Add(hit.transform.gameObject);
                    }
                
            }
        }
    }



    public void StartDealDamage()
    {
        //Debug.Log("Start Dealing Damage");
        canDealDamage = true;
        hasDealtDamage.Clear();
        enemyLockOn.FreezeTargetLock();
    }

    public void EndDealDamage()
    {
        //Debug.Log("End Dealing Damage");

        canDealDamage = false;
        enemyLockOn.UnfreezeTargetLock();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}