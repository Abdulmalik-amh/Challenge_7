using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; set;}
    //ENV Channel
    // public AudioSource EnvironmentChannel;
    // public AudioClip WindSound;
    // public AudioClip RainSound;

    //PlayerHurtChannel includes CutSounds
    public AudioSource sowrdCutChannel;
    public AudioClip sowrdCut_1;
    public AudioClip sowrdCut_2;
    public AudioClip sowrdCut_3;
    //PlayerGruntChannel includes GruntSounds
    public AudioSource GruntChannel;
    public AudioClip playerAttackGrunt_1;
    public AudioClip playerAttackGrunt_2;
    

    //Player Dashing Channel
    public AudioSource dashChannel;
    public AudioClip dashSound;

    //Player Dashing Channel
    public AudioSource shieldBlockingChannel;
    public AudioClip shieldRaiseSound;
    public AudioClip shieldBlockSound;

    //PlayerMovementChannel
    public AudioSource MovementGroundChannel;
    public AudioClip MovementGround;
    public AudioSource MovementBridgeChannel;
    public AudioClip MovementBridge;

    //Player hitted channel and sounds 
    public AudioSource gettingHitChannel;
    public AudioClip playerHitted_1;
    public AudioClip playerHitted_2;
   
   
   //Player Exhausted Channel
    public AudioSource exhaustedChannel;
    public AudioClip exhaustedSound;

    //Player Dieng Channel
    public AudioSource DieChannel;
    public AudioClip DieSound;

    
    //Player Kick Channel
    public AudioSource kickChannel;
    public AudioClip kickSound;


    private void Awake() {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }

        else{
            Instance = this;
        }
    }
    // SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);
}