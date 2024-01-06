using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatScript : MonoBehaviour
{
    Animator animator;
    EnemyLockOn enemyLockOn;
    PlayerInfo playerInfo;

    [SerializeField] GameObject currentWeapon;
    [SerializeField] GameObject SideWeapon;
    [SerializeField] GameObject currentShield;
    [SerializeField] GameObject sideShield;
    [SerializeField] GameObject parryHitbox;
    [SerializeField] GameObject hitVFX;
    // Add a boolean variable to track whether the block button is held down
    private bool isBlocking = false;
    private bool isAttack = false;

    public int attackType = 0;
    // Rename the variable for the parry hitbox
    public int parryHitboxLayer = 10; // Set the default layer (adjust as needed)
    public float parryHitboxDuration = 0.5f; // Adjust as needed
    private float parryCooldown = 0.0f;
    public float block_Stamina = 1f;
    public float attack_Stamina = 1f;
    public float attackCooldownTimer = 2f;
    private bool abletoAttack = true;

    void Start()
    {
        // Assuming your animator component is attached to the same GameObject
        animator = GetComponent<Animator>();
        enemyLockOn = GetComponent<EnemyLockOn>();
        playerInfo = GetComponent<PlayerInfo>();

        // Set the layer of the parry hitbox
        parryHitbox.layer = parryHitboxLayer;
    }

    public void HandleAllCombatScripts()
    {
        EnableAttack();
        Block();
        holdWeapon();
        // Get cursor location for the Stance parameter
        int cursorLocation = enemyLockOn.GetCursorLocation();
        animator.SetInteger("Stance", cursorLocation);

        // Check for attacks
        if (Input.GetMouseButtonDown(0))
        {
            if (enemyLockOn.IsTargeting())
            {
                animator.SetInteger("AttackType", 1);
                attackType = 1;
                animator.SetTrigger("Attack");
                playerInfo.TakeStamina(attack_Stamina);
                isAttack = true;
                abletoAttack = false;

                //( ADD SOUND : ATTACKING )
                
                int num = Random.Range(0,2);
                if(num == 2)
                    SoundManager.Instance.GruntChannel.PlayOneShot(SoundManager.Instance.playerAttackGrunt_1);
                
                
                SoundManager.Instance.GruntChannel.PlayOneShot(SoundManager.Instance.playerAttackGrunt_2);
                
                
            }
        }
        
        // Check for block input (holding down the "E" key)
        if (Input.GetMouseButton(1) && !isBlocking && playerInfo.stamina != 0f)
        {
            // Set the blocking parameter in the animator
            isBlocking = true;
            animator.SetBool("IsBlocking", true);

            // Activate parry hitbox
            parryHitbox.SetActive(true);
            // Set a cooldown before the player can parry again
            parryCooldown = Time.time + parryHitboxDuration + 2.0f; // You can adjust the cooldown duration

            // (ADD SOUND : BLOCKING)
            SoundManager.Instance.shieldBlockingChannel.PlayOneShot(SoundManager.Instance.shieldRaiseSound);

        }
        else if (!Input.GetMouseButton(1))
        {
            // Reset the blocking parameter when "E" key is released
            isBlocking = false;
            animator.SetBool("IsBlocking", false);
        }

        if (isBlocking)
        {
            playerInfo.TakeStamina(block_Stamina);
        }

        // Deactivate parry hitbox after a certain duration
        if (parryHitbox.activeSelf && Time.time > parryCooldown - 1.0f)
        {
            parryHitbox.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CancleAttack();
        }
        else
        {
            animator.SetBool("Cancle", false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Kick");
        }
    }

    public void CancleAttack()
    {
        animator.SetBool("Cancle", true);
        abletoAttack = true;
    }

    void Update()
    {
    }

    public void EnableAttack()
    {
        abletoAttack = true;
    }

    // Add a Block method that can be called from other scripts or events
    public void Block()
    {
        if (!isBlocking && Time.time > parryCooldown)
        {
            // Set the blocking parameter in the animator
            isBlocking = true;
            animator.SetBool("IsBlocking", true);

            // Activate parry hitbox
            parryHitbox.SetActive(true);
            // Set a cooldown before the player can parry again
            parryCooldown = Time.time + parryHitboxDuration + 2.0f; // You can adjust the cooldown duration
        }
    }

    public bool returnBlock()
    {
        return isBlocking;
    }

    public void getBlocked()
    {
        animator.SetTrigger("Blocked");

        // (ADD SOUND : BLOCKED ATTACK)
         //SoundManager.Instance.shieldBlockingChannel.PlayOneShot(SoundManager.Instance.shieldBlockSound);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 1f);
    }

    public void holdWeapon()
    {
        if(enemyLockOn.IsTargeting())
        {
            currentWeapon.SetActive(true);
            SideWeapon.SetActive(false);

            currentShield.SetActive(true);
            sideShield.SetActive(false);
        }
        else
        {
            currentWeapon.SetActive(false);
            SideWeapon.SetActive(true);

            currentShield.SetActive(false);
            sideShield.SetActive(true);
        }
    }
}
