using UnityEngine;

public class AnimatorLayerController : MonoBehaviour
{
    Animator anim;
    EnemyLockOn enemyLockOn;
    DefMovement defMovement;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyLockOn = GetComponent<EnemyLockOn>();
        defMovement = GetComponent<DefMovement>();
    }

    void Update()
    {
        UpdateLayerWeights();
    }

    void UpdateLayerWeights()
    {
        // Check if the player is targeting
        bool isTargeting = enemyLockOn.IsTargeting();

        // Set the weights based on targeting status
        //float baseLayerWeight = isTargeting ? 0 : 1;
        //float upperLayerWeight = isTargeting ? 1 : 0;
        float lowerLayerWeight = isTargeting ? 1 : 0;

        // Set the weights in the Animator
        //anim.SetLayerWeight(0, baseLayerWeight); // Base layer
        //anim.SetLayerWeight(1, upperLayerWeight); // Upper layer
        anim.SetLayerWeight(2, lowerLayerWeight); // Lower layer
    }
}
