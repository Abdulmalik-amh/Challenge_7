using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeSound : MonoBehaviour
{
    
    

    void OnCollisionEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            SoundManager.Instance.MovementBridgeChannel.PlayOneShot(SoundManager.Instance.MovementBridge);
            Debug.Log("Started Sound");
            
        }

    }

    void OnCollisionExit(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            SoundManager.Instance.MovementGroundChannel.PlayOneShot(SoundManager.Instance.MovementGround);
             Debug.Log("Ended Sound");
        }  
    }

   
}
