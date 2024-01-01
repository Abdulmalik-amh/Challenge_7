using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickHandler : MonoBehaviour
{
    PlayerInfo playerInfo;
    CombatScript combatScript;

    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;

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
            int layerMask = (1 << 9);

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                // Check if the hit object has a specific tag (e.g., "Shield")
                if (hit.transform.CompareTag("Shield"))
                {
                    if (hit.transform.TryGetComponent(out PlayerInfo enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                    {
                        enemy.Kicked();
                        hasDealtDamage.Add(hit.transform.gameObject);
                    }
                }

                //test
                if (hit.transform.TryGetComponent(out PlayerInfo test) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    test.Kicked();
                    hasDealtDamage.Add(hit.transform.gameObject);
                    Debug.Log("kicked");
                }
                Debug.Log("kicked");
            }
            Debug.Log("kicked");
        }
    }

    public void StartDealDamage()
    {
        Debug.Log("Start Dealing Damage");
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void EndDealDamage()
    {
        Debug.Log("End Dealing Damage");

        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
