using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    PlayerInfo playerInfo;
    CombatScript combatScript;

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

            // Combine layers 9 and 10 in the layer mask
            int layerMask = (1 << 10) + (1 << 9);
            int ShieldMask = (1 << 12);
            int ParryMask = (1 << 11);
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
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
            // Check if the hit object has a specific tag (e.g., "Shield")
            else if (hit.transform.TryGetComponent(out CombatScript Aenemy) && !hasDealtDamage.Contains(hit.transform.gameObject) && Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, ShieldMask))
            {
                // Handle the case where the hit object has the "Shield" tag
                Debug.Log("Hit a block shield!");
                Aenemy.getBlocked();
                combatScript.CancleAttack();
                combatScript.HitVFX(hit.point);

                // You may want to add further logic for shield interactions here
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