using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Headaim : MonoBehaviour
{
    public Transform baseTarget;        // The base target
    private Transform currentTarget;
    private EnemyLockOn enemyLockOn;      // Assuming you have a script to handle enemy locking


    void Start()
    {
        enemyLockOn = GetComponent<EnemyLockOn>();
        
    }

    void Update()
    {
        if (enemyLockOn)
        {
            currentTarget = enemyLockOn.currentTarget;
        }
        else
        {
            currentTarget = baseTarget;
        }
        
    }
}
