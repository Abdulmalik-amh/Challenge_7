using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeSound : MonoBehaviour
{
    
    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            SoundManager.Instance.MovementBridgeChannel.PlayOneShot(SoundManager.Instance.MovementBridge);
            
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            SoundManager.Instance.MovementGroundChannel.PlayOneShot(SoundManager.Instance.MovementGround);
        }  
    }
}
