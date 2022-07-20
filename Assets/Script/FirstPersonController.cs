using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && _characterController.isGrounded;
    
    [Header("Functional Options")] 
    [SerializeField] private bool canSprint;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canUseHeadBob = true;

    [Header("Controls")] 
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    
    [Header("Movement Parameters")] 
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;

    [Header("Look Parameters")] 
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")] 
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Headbob Parameters")] 
    [SerializeField] private float walkBobSpeed = 10f;
    [SerializeField] private float walkBobAmount = .1f;
    [SerializeField] private float sprintBobSpeed = 14f;
    [SerializeField] private float sprintBobAmount = .2f;
    private float _defaultYpos = 0;
    private float timer;

    private Camera _playerCamera;
    private CharacterController _characterController;

    private Vector3 _moveDirection;
    private Vector2 _currentInput;

    private float _rotationX = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        _defaultYpos = _playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (canJump)
            {
                HandleJump();
            }

            if (canUseHeadBob)
            {
                HandleHeadBob();
            }
            
            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        _currentInput = new Vector2((IsSprinting ? sprintSpeed : walkSpeed)* Input.GetAxis("Vertical"),(IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        
        var moveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) +
                        (transform.TransformDirection(Vector3.right) * _currentInput.y);
        _moveDirection.y = moveDirectionY;
    }
    private void HandleMouseLook()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        _rotationX = Mathf.Clamp(_rotationX, -upperLookLimit, lowerLookLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump)
            _moveDirection.y = jumpForce;
    }

    private void HandleHeadBob()
    {
        if(!_characterController.isGrounded) return;

        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (IsSprinting ? sprintBobSpeed : walkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(
                _playerCamera.transform.localPosition.x,
                _defaultYpos + Mathf.Sin(timer) * (IsSprinting ? sprintBobAmount : walkBobAmount), 
                _playerCamera.transform.localPosition.z);
        }
    }
    private void ApplyFinalMovements()
    {
        if (!_characterController.isGrounded)
            _moveDirection.y -= gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);
    }
}
