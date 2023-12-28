using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatScript : MonoBehaviour
{
    Animator animator;
    EnemyLockOn enemyLockOn;
    PlayerInfo playerInfo;

    // Add a boolean variable to track whether the block button is held down
    private bool isBlocking = false;
    private bool isAttack = false;

    // Rename the variable for the parry hitbox
    public GameObject parryHitbox;
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

    void Update()
    {
        // Get cursor location for the Stance parameter
        int cursorLocation = enemyLockOn.GetCursorLocation();
        animator.SetInteger("Stance", cursorLocation);

        // Check for attacks
        if (Input.GetMouseButtonDown(0))
        {
            if(abletoAttack && enemyLockOn.IsTargeting())
            {
                // Left click for light attack
                animator.SetInteger("AttackType", 1);
                playerInfo.TakeStamina(attack_Stamina);
                isAttack = true;
                abletoAttack = false;
                StartCoroutine(AttackCoolDown());
            }
        }
        else
        {
            // Reset the AttackType parameter if no attack input
            animator.SetInteger("AttackType", 0);
            isAttack = false;
        }

        if( isAttack)
        {
            playerInfo.TakeStamina(attack_Stamina);
        }

        // Check for block input (holding down the "E" key)
        if (Input.GetMouseButton(1) && !isBlocking && Time.time > parryCooldown && playerInfo.stamina != 0f)
        {
            // Set the blocking parameter in the animator
            isBlocking = true;
            animator.SetBool("IsBlocking", true);

            // Activate parry hitbox
            parryHitbox.SetActive(true);
            // Set a cooldown before the player can parry again
            parryCooldown = Time.time + parryHitboxDuration + 2.0f; // You can adjust the cooldown duration
            
        }
        else if (!Input.GetMouseButton(1))
        {
            // Reset the blocking parameter when "E" key is released
            isBlocking = false;
            animator.SetBool("IsBlocking", false);
        }

        if(isBlocking)
        {
            playerInfo.TakeStamina(block_Stamina);
        }

        // Deactivate parry hitbox after a certain duration
        if (parryHitbox.activeSelf && Time.time > parryCooldown - 1.0f)
        {
            parryHitbox.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("Cancle",true);
        }
        else
        {
            animator.SetBool("Cancle", false);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Kick");
        }
    }

    public IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCooldownTimer);
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
}
