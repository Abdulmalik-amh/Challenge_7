using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class rigScripts : MonoBehaviourPun, IPunObservable
{

    Rig rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rig>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(rig.weight);
        //}
        //else if (stream.IsReading)
        //{
        //    rig.weight = (float)stream.ReceiveNext();
        //    //transform.rotation = (Quaternion)stream.ReceiveNext();
        //}
    }
}
