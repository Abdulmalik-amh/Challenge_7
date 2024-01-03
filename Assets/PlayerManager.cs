using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;
using System;

public class PlayerManager : MonoBehaviour
{
    PhotonView view;
    DefMovement defMovement;
    EnemyLockOn enemyLockOn;
    CombatScript combatScript;
    MouseAim mouseAim;
    animationEvents animationEv;

    RigBuilder rigBuilder;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        defMovement = GetComponent<DefMovement>();
        enemyLockOn = GetComponent<EnemyLockOn>();
        combatScript = GetComponent<CombatScript>();
        mouseAim = GetComponent<MouseAim>();
        animationEv = GetComponent<animationEvents>();
        rigBuilder = GetComponent<RigBuilder>();
    }
    // Start is called before the first frame update
    void Start()
    {
       rigBuilder.enabled = false;


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
        StartCoroutine(RigStart());
    }


    IEnumerator RigStart()
    {
        yield return new WaitForSeconds(4);
        rigBuilder.enabled = true;
    }


   
}
