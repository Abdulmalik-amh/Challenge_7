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

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                // Check if the hit object has a specific tag (e.g., "Shield")
                if (hit.transform.CompareTag("Shield"))
                {
                    // Handle the case where the hit object has the "Shield" tag
                    Debug.Log("Hit a block shield!");
                    hit.transform.GetComponent<CombatScript>().getBlocked();
                    // You may want to add further logic for shield interactions here
                }
                else
                {
                    // Check if the hit object is an enemy and hasn't received damage yet
                    if (hit.transform.TryGetComponent(out PlayerInfo enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                    {
                        enemy.TakeDamage(weaponDamage);
                        Debug.Log("Damage dealt: " + weaponDamage);
                        enemy.HitVFX(hit.point);
                        hasDealtDamage.Add(hit.transform.gameObject);
                    }
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