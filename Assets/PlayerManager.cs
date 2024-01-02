using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviourPun, IPunObservable
{
    PhotonView view;
    DefMovement defMovement;
    EnemyLockOn enemyLockOn;
    CombatScript combatScript;
    MouseAim mouseAim;
    animationEvents animationEv;

    RigBuilder rig;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        defMovement = GetComponent<DefMovement>();
        enemyLockOn = GetComponent<EnemyLockOn>();
        combatScript = GetComponent<CombatScript>();
        mouseAim = GetComponent<MouseAim>();
        animationEv = GetComponent<animationEvents>();
        rig = GetComponent<RigBuilder>();
    }
    // Start is called before the first frame update
    void Start()
    {
       rig.enabled = false;
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
        rig.enabled = true;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(transform.position);
    //        stream.SendNext(transform.rotation);
    //    }
    //    else if (stream.IsReading)
    //    {
    //        transform.position = (Vector3)stream.ReceiveNext();
    //        transform.rotation = (Quaternion)stream.ReceiveNext();
    //    }
    }
}
