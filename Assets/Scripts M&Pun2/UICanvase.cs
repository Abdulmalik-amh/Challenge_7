using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UICanvase : MonoBehaviour
{

    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (!view.IsMine)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
