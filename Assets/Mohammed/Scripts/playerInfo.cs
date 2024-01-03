using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    Animator animator;

    public int maxHealth = 100;
    public int maxStamina = 50;
    public float staminaRegenRate = 5f;
    public float staminaRegenCooldown = 3f; // Cooldown time in seconds
    [SerializeField] GameObject hitVFX;

    public float health;
    public float stamina;
    private bool isStaminaRegenerating;
    private float staminaRegenTimer;
    private float lastStaminaChangeTime;
    public float regenerationTime;
    private float regenerationStartTime;
    public float regenerationRate;
    public float dashInterruptRegenTimeMultiplier = 2f;

    private void Start()
    {
        // Initialize player stats
        health = maxHealth;
        stamina = maxStamina;
        isStaminaRegenerating = false;
        lastStaminaChangeTime = Time.time;

        // Set the initial regeneration start time
        regenerationStartTime = Time.time + regenerationTime;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Update the stamina regeneration timer
        if (!isStaminaRegenerating)
        {
            staminaRegenTimer -= Time.deltaTime;

            // Check if the cooldown has ended
            if (staminaRegenTimer <= 0f)
            {
                isStaminaRegenerating = true;
                regenerationStartTime = Time.time + regenerationTime;
            }
        }

        // Update the stamina regeneration
        if (isStaminaRegenerating)
        {
            if (stamina < maxStamina && Time.time > regenerationStartTime)
            {
                // Calculate the interpolation factor based on the remaining regeneration time
                float interpolationFactor = Mathf.Clamp01((Time.time - regenerationStartTime) / regenerationTime);

                // Use Lerp to smoothly interpolate towards max stamina based on the factor
                stamina += regenerationRate * interpolationFactor * Time.deltaTime;

                // Clamp the stamina to ensure it doesn't exceed the maximum capacity
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }

            // Check if stamina has been used, if so, restart the regeneration wait
            if (Time.time - lastStaminaChangeTime < staminaRegenCooldown)
            {
                isStaminaRegenerating = false;
                staminaRegenTimer = staminaRegenCooldown;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        animator.SetTrigger("getHit"); 
    }

    public bool TakeStamina(float staminaCost)
    {
        if (stamina >= staminaCost)
        {
            stamina -= staminaCost;
            lastStaminaChangeTime = Time.time;

            // Reset the stamina regeneration timer when stamina is consumed
            staminaRegenTimer = staminaRegenCooldown;

            if (stamina == 0)
            {
                // If stamina reaches 0, restart the regeneration wait time with double duration
                isStaminaRegenerating = false;
                regenerationStartTime = Time.time + regenerationTime * dashInterruptRegenTimeMultiplier;
            }
        }
        else
        {
            Debug.Log("Not enough stamina!");
            stamina = 0; // Ensure stamina doesn't go below 0
            return false;
        }

        return true;
    }

    public void StartDash()
    {
        // If player dashes, interrupt regeneration
        isStaminaRegenerating = false;
        staminaRegenTimer = staminaRegenCooldown;
    }

    public void DisplayStats()
    {
        Debug.Log($"Health: {health}/{maxHealth}");
        Debug.Log($"Stamina: {stamina}/{maxStamina}");
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 1f);
    }

    public void Kicked()
    {
        animator.SetTrigger("Kicked");
        TakeDamage(10f);
    }
}
