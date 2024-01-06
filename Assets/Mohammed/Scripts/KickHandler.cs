using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickHandler : MonoBehaviour
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
            int layerMask = (1 << 10) + (1 << 9) + (1 << 11) + (1 << 12);
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                // Check if the hit object is an enemy and hasn't received damage yet
                if (hit.transform.TryGetComponent(out PlayerInfo Benemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    Benemy.Kicked();
                    hasDealtDamage.Add(hit.transform.gameObject);
                    //(ADD SOUND: SOWRD SLASHED PLAYER)

                    int num = Random.Range(0, 3);
                    if (num == 1)
                        SoundManager.Instance.sowrdCutChannel.PlayOneShot(SoundManager.Instance.sowrdCut_2);

                    if (num == 2)
                        SoundManager.Instance.sowrdCutChannel.PlayOneShot(SoundManager.Instance.sowrdCut_3);

                    if (num == 3)
                        SoundManager.Instance.sowrdCutChannel.PlayOneShot(SoundManager.Instance.sowrdCut_1);


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
