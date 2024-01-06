using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimpleLockOn : MonoBehaviour
{
    PhotonView view;
    [SerializeField] Transform target;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }
    void OnEnable(){
        if(target == null) target = Camera.main.transform;
        StartCoroutine(LookAtTarget());
    }

    private IEnumerator LookAtTarget(){
        while(this.gameObject.activeInHierarchy){
            Vector3 _dir = target.position - transform.position;
            //_dir.y = 0;
            transform.rotation = Quaternion.LookRotation(_dir);
            yield return null;
        }
    }
}
