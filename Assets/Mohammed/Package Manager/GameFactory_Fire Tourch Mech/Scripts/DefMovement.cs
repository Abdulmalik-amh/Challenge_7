using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DefMovement : MonoBehaviour
{
    CharacterController controller;
    Animator anim;
    Transform cam;
    EnemyLockOn enemyLockOn;
    CombatScript combatScript;
    public PlayerInfo playerInfo; 
    public Slider healthSlider;    
    public Slider staminaSlider;   

    float speedSmoothVelocity;
    float speedSmoothTime = 0.1f; 
    float currentSpeed;
    float velocityY;
    Vector2 moveInput;
    Vector3 dir;

    [Header("Settings")]
    [SerializeField] float gravity = 25f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float walkingMultiplayer = .75f;
    [SerializeField] float crouchSpeedMultiplayer = .45f;
    [SerializeField] float sprintSpeedMultiplier = 1f; 
    [SerializeField] float dashSpeedMultiplier = 2f; 
    [SerializeField] float dashDistance = 5f;
    [SerializeField] float dashDuration = 0.3f;
    [SerializeField] float rotateSpeed = 3f;
    [SerializeField] float crouchTransitionSpeed = 3f;
    [SerializeField] float weightMultiplier = 1f;
    public bool lockMovement;
    bool isCrouching;
    bool isSprinting;
    bool isDashing;
    Vector3 dashDirection;

    public bool isTargting;

    public float stamina_Sprint = .1f;
    public float stamina_Dash = 4f;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        enemyLockOn = GetComponent<EnemyLockOn>(); // Add this line
    }


    public void HandleAllDefMovement()
    {
        GetInput();
        PlayerMovement();
        if (!lockMovement) PlayerRotation();
        DebugEverything();
        isTargting = enemyLockOn.IsTargeting();

        // Check for sprinting and reduce stamina if moving
        if (isSprinting && dir.magnitude > 0)
        {
            playerInfo.TakeStamina(stamina_Sprint); // Adjust the stamina cost as needed
        }

        // Check for dashing and reduce stamina
        if (isDashing)
        {
            playerInfo.TakeStamina(stamina_Dash); // Adjust the stamina cost as needed
        }

        // Update the sliders
        UpdateSliders();
    }
    void Update()
    {
        
    }



    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        dir = (forward * moveInput.y + right * moveInput.x).normalized;

        if (Keyboard.current.cKey.isPressed)
        {
            isCrouching = true;
            anim.SetFloat("Crouch", Mathf.Lerp(anim.GetFloat("Crouch"), -1f, Time.deltaTime * crouchTransitionSpeed));
        }
        else
        {
            isCrouching = false;
            anim.SetFloat("Crouch", Mathf.Lerp(anim.GetFloat("Crouch"), 1f, Time.deltaTime * crouchTransitionSpeed));
        }

        isSprinting = Keyboard.current.leftShiftKey.isPressed && playerInfo.stamina > 0;

        if (moveInput.magnitude > 0 && Keyboard.current.spaceKey.wasPressedThisFrame && lockMovement && playerInfo.stamina > 0)
        {
            StartDash();
        }

    }

    private void PlayerMovement()
    {
        float targetSpeed = CalculateTargetSpeed();

        if (controller.isGrounded)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        }

        if (velocityY > -10) velocityY -= Time.deltaTime * gravity;
        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY;

        if (isDashing)
        {
            velocity = dashDirection * maxSpeed * dashSpeedMultiplier; // Apply dash speed multiplier
        }
 

        controller.Move(velocity * Time.deltaTime);

        anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
        anim.SetFloat("CurrentSpeed", currentSpeed);
        anim.SetBool("Sprint", isSprinting);
        anim.SetBool("Dash", isDashing);
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return;
        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed);
    }

    private void StartDash()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashDirection = dir; // Set dash direction to current movement direction
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private void DebugEverything()
    {
        /*Debug.Log($"MoveInput: {moveInput}");
        Debug.Log($"Dir: {dir}");
        Debug.Log($"Current Speed: {currentSpeed}");
        Debug.Log($"Is Crouching: {isCrouching}");
        Debug.Log($"Is Sprinting: {isSprinting}");
        Debug.Log($"Is Dashing: {isDashing}");*/
    }

    private float CalculateTargetSpeed()
    {
        if (dir.magnitude == 0)
        {
            // Player is not moving, return 0 speed
            return 0f;
        }

        float baseSpeed = maxSpeed;
        float speedModifier = isSprinting ? sprintSpeedMultiplier : 1f;

        if (isCrouching)
        {
            speedModifier *= crouchSpeedMultiplayer;
        }

        return baseSpeed * speedModifier;
    }

    private void UpdateSliders()
    {
        // Update health slider value
        healthSlider.value = playerInfo.health / (float)playerInfo.maxHealth;

        // Update stamina slider value
        staminaSlider.value = playerInfo.stamina / (float)playerInfo.maxStamina;
    }


    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        return smoothTime / weightMultiplier;
    }
}
