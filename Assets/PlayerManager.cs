using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    PhotonView view;
    DefMovement defMovement;
    EnemyLockOn enemyLockOn;
    CombatScript combatScript;
    MouseAim mouseAim;
    animationEvents animationEv;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        defMovement = GetComponent<DefMovement>();
        enemyLockOn = GetComponent<EnemyLockOn>();
        combatScript = GetComponent<CombatScript>();
        mouseAim = GetComponent<MouseAim>();
        animationEv = GetComponent<animationEvents>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine)
            return;

        enemyLockOn.HandleAlEnemylLockOn();
        mouseAim.HandleAllMouseAim();
    }

    private void FixedUpdate()
    {
        if (!view.IsMine)
            return;

        defMovement.HandleAllDefMovement();
        combatScript.HandleAllCombatScripts();
    }
}
