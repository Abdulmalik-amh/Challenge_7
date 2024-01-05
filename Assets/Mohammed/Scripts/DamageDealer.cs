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
    [SerializeField] int team; // Team 1 or Team 2

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();

        playerInfo = GetComponentInParent<PlayerInfo>();
        combatScript = GetComponentInParent<CombatScript>();
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

            int teamLayerMask = (team == 1) ? (1 << 9) : (1 << 10);
            int shieldMask = (1 << 12);

            // Exclude the GameObject to which this script is attached
            int layerMask = ~((team == 1) ? (1 << 10) : (1 << 9));

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, shieldMask))
            {
                Debug.Log("Hit Shield");

                if (hit.transform.root.TryGetComponent(out CombatScript enemyCombatScript))
                {
                    Debug.Log("CombatScript found on the hit object");
                    Debug.Log("Hit a block shield!");
                    enemyCombatScript.getBlocked();
                    combatScript.CancleAttack();
                    combatScript.HitVFX(hit.point);
                }
                else
                {
                    Debug.Log("CombatScript not found on the hit object");
                }
            }
            else if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform != null && hit.transform != transform && hit.transform.TryGetComponent(out PlayerInfo enemyPlayerInfo) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    enemyPlayerInfo.TakeDamage(weaponDamage);
                    Debug.Log("Damage dealt: " + weaponDamage);
                    enemyPlayerInfo.HitVFX(hit.point);
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

    }

    public void EndDealDamage()
    {
        //Debug.Log("End Dealing Damage");

        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}