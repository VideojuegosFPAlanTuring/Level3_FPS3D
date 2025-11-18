using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Transform cameraTransform;
    private WeaponController weaponController;

    [Header("Player Move Setting")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float gravity = Physics.gravity.y;

    //Input fields for movement and look actions
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation = 0;

    [Header("Camera Setting")]
    [SerializeField] private float lookSensibility = 1f;
    [SerializeField] private float maxLookUpAngle = 80f;
    [SerializeField] private float maxLookDownAngle = 50f;

    //Is Sprinting state
    private bool isSprinting;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        weaponController = GetComponent<WeaponController>();

        //Hide mouse coursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        LookAround();
    }

    #region INPUT SYSTEM
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //if Player is touching ground
        if (characterController.isGrounded)
        {
            //Calculate the require velocity for a jump
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        //when action started or manteined
        isSprinting = context.started || context.performed;
    }

    
    public void Shoot(InputAction.CallbackContext context)
    {
        if (weaponController.CanShoot()) weaponController.Shoot();
    }

    #endregion

    private void MovePlayer()
    {
        //Falling down
        if (characterController.isGrounded)
        {
            //Restart vertical velocity when touch ground
            verticalVelocity = 0f;
        }
        else
        {
            //when is falling down increment velocity with gravity and time
            verticalVelocity += gravity * Time.deltaTime;
        }

        //Vertical Move
        Vector3 move = new Vector3(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);

        //Horizontal Move
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        float targetSpeed = isSprinting ? speed * multiplier : speed;
        characterController.Move(moveDirection * targetSpeed * Time.deltaTime);

        //Apply gravity constantly to posibility Jump
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }

    /// <summary>
    /// Handles camera rotation based on Look Input
    /// </summary>
    private void LookAround()
    {
        //Horizontal rotation (X-axis) based on sensibility and input
        float horizontalRotation = lookInput.x * lookSensibility;
        transform.Rotate(Vector3.up * horizontalRotation);

        //Vertical rotation (Y-axis) with clamping to prevent over-rotation
        verticalRotation -= lookInput.y * lookSensibility;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookUpAngle, maxLookDownAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

    }

}
