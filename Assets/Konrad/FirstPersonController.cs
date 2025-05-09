using System;
using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // Movement flags and state
    public bool CanMove { get; set; } = true;
    private bool IsCrouching;
    private bool DuringCrouchAnimation;
    private bool wasSprinting = false;

    // Rotation and camera
    private float rotationX;
    private float defaultYPosition;
    private float defaultFOV;
    private Camera playerCamera;

    // Health and stamina
    private float currentHealth;
    private float currentStamina;
    private Coroutine regeneratingHealth;
    private Coroutine regeneratingStamina;

    // Movement calculation
    private Vector3 moveDirection;
    private Vector2 currentInput;
    private Vector3 hitPointNormal;

    // Timers
    private float timer;
    private float footstepTimer;

    // Zoom coroutine
    private Coroutine zoomRoutine;

    // Character controller
    private CharacterController characterController;

    // Event actions
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;
    public static Action<float> OnStaminaChange;

    // Input checks
    private bool IsSprinting => CanSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => CanJump && Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => CanCrouch && Input.GetKeyDown(crouchKey) && !DuringCrouchAnimation && characterController.isGrounded;
    private bool IsSliding => characterController.isGrounded
                              && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f)
                              && Vector3.Angle((hitPointNormal = slopeHit.normal), Vector3.up) > characterController.slopeLimit;

    // Footstep timing multiplier based on state
    private float CurrentOffset
    {
        get
        {
            if (IsCrouching) return baseStepSpeed * crouchStepMultiplier;
            if (IsSprinting) return baseStepSpeed * sprintStepMultiplier;
            return baseStepSpeed;
        }
    }

    [Header("Functional Options")]
    [SerializeField] private bool CanSprint = true;
    [SerializeField] private bool CanJump = true;
    [SerializeField] private bool CanCrouch = true;
    [SerializeField] private bool CanUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;
    [SerializeField] private bool UseStamina = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float slopeSpeed = 6.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Health Parameters")]
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float timeBeforeHealthRegenStarts = 20.0f;
    [SerializeField] private float healthValueIncrement = 0.5f;
    [SerializeField] private float healthTimeIncrement = 3.0f;

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float staminaUseMultiplier = 10.0f;
    [SerializeField] private float timeBeforeStaminaRegenStarts = 2.0f;
    [SerializeField] private float staminaValueIncrement = 1.0f;
    [SerializeField] private float staminaTimeIncrement = 0.02f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 10.0f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 14.0f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 6.0f;
    [SerializeField] private float crouchBobAmount = 0.025f;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30.0f;

    [Header("Audio Effects Parameters")]
    [SerializeField] private AudioClip breathingClip;
    [SerializeField] private AudioSource breathingAudioSource;
    [SerializeField] private float breathingDelay = 0.5f;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] terrainClips;

    private void OnEnable() => OnTakeDamage += applyDamage;
    private void OnDisable() => OnTakeDamage -= applyDamage;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        defaultYPosition = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;

        currentHealth = maxHealth;
        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!CanMove) return;

        HandleMovementInput();
        HandleMouseLook();
        if (CanJump) HandleJump();
        if (CanCrouch) HandleCrouch();
        if (CanUseHeadbob) HandleHeadbob();
        if (UseStamina) HandleStamina();
        if (canZoom) HandleZoom();
        if (useFootsteps) HandleFootsteps();

        if (wasSprinting && !IsSprinting)
            StartCoroutine(PlayBreathing());
        else if (!wasSprinting && IsSprinting && breathingAudioSource != null)
            breathingAudioSource.Stop();

        wasSprinting = IsSprinting;
        ApplyFinalMovements();
    }

    private void HandleMovementInput()
    {
        float speed = IsCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed;
        currentInput = new Vector2(speed * Input.GetAxis("Vertical"), speed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = transform.forward * currentInput.x + transform.right * currentInput.y;
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump) moveDirection.y = jumpForce;
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch) StartCoroutine(CrouchStand());
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded || currentInput == Vector2.zero)
            return;

        float bobSpeed = IsCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed;
        float bobAmount = IsCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount;

        timer += Time.deltaTime * bobSpeed;
        float newY = defaultYPosition + Mathf.Sin(timer) * bobAmount;
        playerCamera.transform.localPosition = new Vector3(
            playerCamera.transform.localPosition.x,
            newY,
            playerCamera.transform.localPosition.z);
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if (zoomRoutine != null) StopCoroutine(zoomRoutine);
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        else if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null) StopCoroutine(zoomRoutine);
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded || currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;
        if (footstepTimer > 0) return;

        if (Physics.Raycast(characterController.transform.position, Vector3.down, out RaycastHit hit, 5f)
            && hit.collider.CompareTag("Terrain"))
        {
            footstepAudioSource.PlayOneShot(
                terrainClips[UnityEngine.Random.Range(0, terrainClips.Length)]);
        }
        footstepTimer = CurrentOffset;
    }

    private void HandleStamina()
    {
        if (IsSprinting && currentInput != Vector2.zero)
        {
            if (regeneratingStamina != null) StopCoroutine(regeneratingStamina);
            currentStamina = Mathf.Max(0, currentStamina - staminaUseMultiplier * Time.deltaTime);
            OnStaminaChange?.Invoke(currentStamina);
            if (currentStamina <= 0) CanSprint = false;
        }
        else if (currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        if (WillSlideOnSlopes && IsSliding)
        {
            moveDirection = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void applyDamage(float damage)
    {
        currentHealth -= damage;
        OnDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
            KillPlayer();
        else
        {
            if (regeneratingHealth != null) StopCoroutine(regeneratingHealth);
            regeneratingHealth = StartCoroutine(RegenerateHealth());
        }
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        if (regeneratingHealth != null) StopCoroutine(regeneratingHealth);
        // TODO: Add death behavior
    }

    private IEnumerator CrouchStand()
    {
        if (IsCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        DuringCrouchAnimation = true;
        float timeElapsed = 0f;
        float startHeight = characterController.height;
        Vector3 startCenter = characterController.center;

        float targetHeight = IsCrouching ? standingHeight : crouchHeight;
        Vector3 targetCenter = IsCrouching ? standingCenter : crouchingCenter;

        while (timeElapsed < timeToCrouch)
        {
            float t = timeElapsed / timeToCrouch;
            characterController.height = Mathf.Lerp(startHeight, targetHeight, t);
            characterController.center = Vector3.Lerp(startCenter, targetCenter, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;
        IsCrouching = !IsCrouching;
        DuringCrouchAnimation = false;
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(timeBeforeHealthRegenStarts);
        WaitForSeconds delay = new WaitForSeconds(healthTimeIncrement);
        while (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + healthValueIncrement, maxHealth);
            OnHeal?.Invoke(currentHealth);
            yield return delay;
        }
        regeneratingHealth = null;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
        WaitForSeconds delay = new WaitForSeconds(staminaTimeIncrement);
        while (currentStamina < maxStamina)
        {
            if (currentStamina > 0) CanSprint = true;
            currentStamina = Mathf.Min(currentStamina + staminaValueIncrement, maxStamina);
            OnStaminaChange?.Invoke(currentStamina);
            yield return delay;
        }
        regeneratingStamina = null;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float startFOV = playerCamera.fieldOfView;
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float timeElapsed = 0f;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }

    private IEnumerator PlayBreathing()
    {
        yield return new WaitForSeconds(breathingDelay);
        breathingAudioSource.PlayOneShot(breathingClip);
    }

    // Public getters
    public float GetCurrentStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;
}
