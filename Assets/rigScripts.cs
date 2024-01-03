using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class rigScripts : MonoBehaviour
{

    Rig rig;
    // Start is called before the first frame update

    private void Awake()
    {
        rig = GetComponent<Rig>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
