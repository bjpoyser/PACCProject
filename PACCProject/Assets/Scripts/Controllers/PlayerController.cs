using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constants
    private const string IS_MOVING_BOOL = "IsMoving";
    private const string IS_RUNNING_BOOL = "IsRunning";
    private const string COLLECT_TRIGGER = "CollectTrigger";
    private const string IS_INTERACTING = "IsInteracting";

    private const string INTERACTABLE_TAG = "InteractableObject";
    #endregion

    #region Variables
    [Header("Character Settings")]
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _turnSpeed;

    [Header("Keys")]
    [SerializeField] private KeyCode _inspectKey = KeyCode.E;
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;

    //Components
    private Animator _animator;
    private Rigidbody _rigidBody;

    //Controller Variables
    private bool _isMoving;
    private bool _canMove;
    private float _currentSpeed;
    private bool _isInteractionEnabled;
    private bool _isCollectig;

    //Vectors
    private Vector3 _movementVector;
    private Quaternion _rotationVector = Quaternion.identity;

    //Camera
    private Transform _cameraTransform;
    private Vector3 _cameraForward;
    #endregion

    #region Properties
    public bool CanMove { get => _canMove; set => _canMove = value; }
    #endregion

    #region Methods
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _canMove = true;
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        Interaction();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (_canMove)
        {
            //Input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //Direction
            if (_cameraTransform != null)
            {
                _cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1).normalized);
                _movementVector = verticalInput * _cameraForward + horizontalInput * _cameraTransform.right;
                _movementVector.Normalize();
            }

            //Animation   
            bool has_H_Input = !Mathf.Approximately(horizontalInput, 0);
            bool has_V_Input = !Mathf.Approximately(verticalInput, 0);

            _isMoving = has_H_Input || has_V_Input;
            _animator.SetBool(IS_MOVING_BOOL, _isMoving);
            _animator.SetBool(IS_RUNNING_BOOL, Input.GetKey(_runKey));

            //Speed
            float inputSpeed = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            if (Input.GetKey(_runKey)) _currentSpeed = _runningSpeed;
            else _currentSpeed = _walkingSpeed;

            //Movement and Rotation
            if (_isMoving)
            {
                Vector3 desiredForward = Vector3.RotateTowards(transform.forward, _movementVector, _turnSpeed * Time.deltaTime, 0f);
                _rotationVector = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredForward), _turnSpeed);
                _rigidBody.MoveRotation(_rotationVector);
                _rigidBody.MovePosition(_rigidBody.position + inputSpeed * _movementVector * _currentSpeed * Time.deltaTime);
            }
        }
    }

    private void Interaction()
    {
        if (_isInteractionEnabled && Input.GetKeyDown(_inspectKey))
        {
            _isInteractionEnabled = false;
            _canMove = false;
            _isMoving = false;
            _animator.SetBool(IS_INTERACTING, true);
            _animator.SetTrigger(COLLECT_TRIGGER);
        }
    }

    private void Collect()
    {
        if (InteractableObject.Instance != null)
        {
            InteractableObject.Instance.gameObject.GetComponentInChildren<AlertBoxScript>().alertImage.gameObject.SetActive(false);
            Destroy(InteractableObject.Instance.gameObject);
            InteractableObject.Instance.CleanCurrent();
        }
    }

    private void EnableMovement()
    {
        _animator.SetBool(IS_INTERACTING, false);
        _canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(INTERACTABLE_TAG) && other != InteractableObject.Instance)
        {
            _isInteractionEnabled = true;
            other.GetComponent<InteractableObject>().SetAsCurrent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == InteractableObject.Instance)
        {
            _isInteractionEnabled = false;
            InteractableObject.Instance.CleanCurrent();
        }
    }
    #endregion
}
