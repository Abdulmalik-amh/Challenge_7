using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MouseAim : MonoBehaviour
{
    EnemyLockOn enemyLockOn;
    CombatScript combatScript;

    [SerializeField] GameObject Laim;
    [SerializeField] GameObject Raim;

    private bool isAim;
    private bool isShield;

    private MultiAimConstraint LmultiAimConstraint;
    private MultiAimConstraint RmultiAimConstraint;
    void Start()
    {
        enemyLockOn = GetComponent<EnemyLockOn>();
        combatScript = GetComponent<CombatScript>();

        LmultiAimConstraint = Laim.GetComponent<MultiAimConstraint>();
        RmultiAimConstraint = Raim.GetComponent<MultiAimConstraint>();
    }

    public void HandleAllMouseAim()
    {
        isAim = enemyLockOn.IsTargeting();
        isShield = combatScript.returnBlock();

        if (isAim)
        {
            if (isShield)
            {
                RmultiAimConstraint.weight = .15f;
                LmultiAimConstraint.weight = 1f;
            }
            else
            {
                RmultiAimConstraint.weight = 1f;
                LmultiAimConstraint.weight = .25f;
            }
        }
        else
        {
            RmultiAimConstraint.weight = 0.1f;
            LmultiAimConstraint.weight = 0.1f;
        }
    }
    void Update()
    {
        
    }

}
