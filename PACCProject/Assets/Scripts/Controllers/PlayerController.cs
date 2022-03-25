using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool _isCameraZoomed;
    private bool _isCollectig;

    //Vectors
    private Vector3 _movementVector;
    private Quaternion _rotationVector = Quaternion.identity;

    //Camera
    private Transform _cameraTransform;
    private Vector3 _cameraForward;

    [SerializeField] private CinemachineVirtualCamera _closeUpCamera;
    [SerializeField] private CinemachineFreeLook _normalCamera;
    #endregion

    #region Properties
    public bool CanMove { get => _canMove; set => _canMove = value; }
    #endregion

    #region Methods
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _canMove = true;
        _isCameraZoomed = false;
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _cameraTransform = GameObject.FindObjectOfType<Camera>().transform;

        _closeUpCamera.gameObject.SetActive(false);
        _normalCamera.gameObject.SetActive(true);
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

        if (_isCollectig)
        {
            if (InteractableObject.Instance != null)
            {
                var lookPos = InteractableObject.Instance.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _turnSpeed);
            }
        }

        if (InteractableObject.Instance == null && _closeUpCamera.gameObject.activeInHierarchy)
        {
            _canMove = !_canMove;
            _isCameraZoomed = !_isCameraZoomed;
            _isMoving = !_isCameraZoomed;
            ZoomIn();
        }
    }

    public void Interaction()
    {
        if (Input.GetKeyDown(_inspectKey) && InteractableObject.Instance != null)
        {
            //_isInteractionEnabled = false;
            _canMove = !_canMove;
            _isCameraZoomed = !_isCameraZoomed;
            _isMoving = !_isCameraZoomed;
            ZoomIn();
            /*_animator.SetBool(IS_INTERACTING, false);
             _animator.SetTrigger(COLLECT_TRIGGER);*/
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void ZoomIn()
    {
        if (InteractableObject.Instance != null)
        {
            if (_isCameraZoomed)
            {
                _closeUpCamera.LookAt = InteractableObject.Instance.transform;
                _animator.SetBool("IsMoving", false);
                _isCollectig = true;
                Collect();
            }
            else
            {
                AlertBoxScript script = InteractableObject.Instance.transform.parent.GetComponentInChildren<AlertBoxScript>();
                script.item.SetActive(false);

                _closeUpCamera.LookAt = null;
                _isCollectig = false;
            }

            _closeUpCamera.gameObject.SetActive(_isCameraZoomed);
            _normalCamera.gameObject.SetActive(!_isCameraZoomed);
        }
    }

    private void Collect()
    {
        if (InteractableObject.Instance != null)
        {
            AlertBoxScript script = InteractableObject.Instance.transform.parent.GetComponentInChildren<AlertBoxScript>();
            script.item.SetActive(true);
            script.itemNotebook.SetActive(true);
            //Destroy(InteractableObject.Instance.gameObject);
            //InteractableObject.Instance.CleanCurrent();
        }
    }

    private void EnableMovement()
    {
        _animator.SetBool(IS_INTERACTING, false);
        _canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(INTERACTABLE_TAG) && other.GetComponentInParent<InteractableObject>() != InteractableObject.Instance)
        {
            _isInteractionEnabled = true;
            other.GetComponentInParent<InteractableObject>().SetAsCurrent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(INTERACTABLE_TAG) && other.GetComponentInParent<InteractableObject>() == InteractableObject.Instance)
        {
            _isInteractionEnabled = false;
            InteractableObject.Instance.CleanCurrent();
        }
    }
    #endregion
}
