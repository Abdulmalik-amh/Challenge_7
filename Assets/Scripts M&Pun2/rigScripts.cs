using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class rigScripts : MonoBehaviour
{
    PhotonView view;
    Rig rig;
    // Start is called before the first frame update

    private void Awake()
    {
        rig = GetComponent<Rig>();
    }
    void Start()
    {
        if (!view.IsMine)
            return;
    }

    // Update is called once per frame
    void Update()
    {
       
    }


}
