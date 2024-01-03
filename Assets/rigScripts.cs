using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class rigScripts : MonoBehaviourPun, IPunObservable
{

    RigBuilder rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<RigBuilder>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    //     if (stream.IsWriting)
    //     {
    //        stream.SendNext(rig);
    //    }
    //    else if (stream.IsReading)
    //    {
    //        transform.position = (Vector3)stream.ReceiveNext();
    //        transform.rotation = (Quaternion)stream.ReceiveNext();
    //    }
    }
}
