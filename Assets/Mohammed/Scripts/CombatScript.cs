using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatScript : MonoBehaviour
{
    Animator animator;
    EnemyLockOn enemyLockOn;
    PlayerInfo playerInfo;

    [SerializeField] GameObject currentWeapon;
    [SerializeField] GameObject currentShield;
    [SerializeField] GameObject parryHitbox;
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

    void Update()
    {
        // Get cursor location for the Stance parameter
        int cursorLocation = enemyLockOn.GetCursorLocation();
        animator.SetInteger("Stance", cursorLocation);

        // Check for attacks
        if (Input.GetMouseButtonDown(0))
        {
            if (enemyLockOn.IsTargeting())
            {
                //StartCoroutine(CheckAttackType());
                animator.SetInteger("AttackType", 1);
                attackType = 1;
                animator.SetTrigger("Attack");
                playerInfo.TakeStamina(attack_Stamina);
                isAttack = true;
                abletoAttack = false;
                StartCoroutine(AttackCoolDown());
            }
        }
        else if (Input.GetMouseButtonUp(0) && isAttack)
        {
            // Release of the left mouse button, stop the coroutine
            StopCoroutine(CheckAttackType());
        }

        if ( isAttack)
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
            abletoAttack = true;
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

    IEnumerator CheckAttackType()
    {
        float holdTime = 0f;

        while (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            yield return null;
        }

        if (holdTime < 0.5f)
        {
            // Short press, light attack
            animator.SetInteger("AttackType", 1);
            attackType = 1;
            animator.SetTrigger("Attack");
            playerInfo.TakeStamina(attack_Stamina);
            isAttack = true;
            abletoAttack = false;
            StartCoroutine(AttackCoolDown());
        }
        else
        {
            // Long press, heavy attack
            animator.SetInteger("AttackType", 2);
            attackType = 2;
            animator.SetTrigger("Attack");
            playerInfo.TakeStamina(attack_Stamina * 1.5f); // Adjust stamina cost as needed
            isAttack = true;
            abletoAttack = false;
            StartCoroutine(AttackCoolDown());
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
