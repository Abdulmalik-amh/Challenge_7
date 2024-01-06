using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ManiCamera : MonoBehaviour
{
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        if (!view.IsMine)
        {
            Destroy(gameObject);
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
