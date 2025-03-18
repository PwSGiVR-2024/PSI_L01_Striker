using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    private bool IsSprinting => CanSprint && Input.GetKey(sprintKey);

    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;

    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !DuringCrouchAnimation && characterController.isGrounded;

    private bool IsCrouching;

    private bool DuringCrouchAnimation;

    private bool IsSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Functional Options")]
    [SerializeField] private bool CanSprint = true;
    [SerializeField] private bool CanJump = true;
    [SerializeField] private bool CanCrouch = true;
    [SerializeField] private bool CanUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;
    [SerializeField] private bool UseStamina = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 1.0f;
    [SerializeField] private float slopeSpeed = 6.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

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

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaUseMultiplier = 5;
    [SerializeField] private float timeBeforeStaminaRegenStart = 5;
    [SerializeField] private float staminaValueIncrement = 2;
    [SerializeField] private float staminaTimeIncrement = 0.1f;

    private float currentStamina;
    //private Coroutine regeneratingStamina;
    private string test;
    private Vector3 hitPointNormal;
    private float defaultYPosition;
    private float timer;
    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPosition = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
            if (CanJump) HandleJump();
            if (CanCrouch) HandleCrouch();
            if (CanUseHeadbob) HandleHeadbob();
            if (UseStamina) HandleStamina();
            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((IsCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"),
                                   (IsCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
                        (transform.TransformDirection(Vector3.right) * currentInput.y);
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
        if (!characterController.isGrounded) return;
        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (IsCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
            defaultYPosition + Mathf.Sin(timer) * (IsCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
            playerCamera.transform.localPosition.z);
        }
    }

    private void HandleStamina()
    {
        if (IsSprinting && currentInput != Vector2.zero)
        {
            currentStamina -= staminaUseMultiplier * Time.deltaTime;

            if (currentStamina < 0)
                currentStamina = 0;

            if (currentStamina <= 0)
                CanSprint = false;
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded) moveDirection.y -= gravity * Time.deltaTime;
        if (WillSlideOnSlopes && IsSliding) moveDirection = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if (IsCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f)) yield break;
        DuringCrouchAnimation = true;
        float timeElapsed = 0;
        float targetHeight = IsCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = IsCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;
        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;
        IsCrouching = !IsCrouching;
        DuringCrouchAnimation = false;
    }
}