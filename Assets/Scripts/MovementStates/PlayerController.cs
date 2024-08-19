using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed = 2.0f;
    [SerializeField]
    public float runSpeed = 4f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    public float currentSpeed;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public Vector3 move;

    private Transform cameraTransform;
    private float rotationSpeed = 20f;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletBarrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    [SerializeField]
    private bool isAutoShooting = false;
    [SerializeField]
    private float shootInterval = 0.1f;
    
    private Coroutine autoShootCoroutine;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;


    MovementBaseStates currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();

    public float HorizontalInput, VerticalInput;

    public Animator animator;


    private GameManager gameManager;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SwitchState(Idle);
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        cameraTransform = Camera.main.transform;
    }


    private void OnEnable()
    {
        /*shootAction.performed += _ => ShootGun();*/
        shootAction.started += OnShootStarted;
        shootAction.canceled += OnShootCanceled;
    }


    private void OnDisable()
    {
        /*shootAction.performed -= _ => ShootGun();*/
        shootAction.started -= OnShootStarted;
        shootAction.canceled -= OnShootCanceled;
    }


    private void OnShootStarted(InputAction.CallbackContext context)
    {
        StartAutoShoot();
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        StopAutoShoot();
    }

    private void StartAutoShoot()
    {
        if (!isAutoShooting)
        {
            isAutoShooting = true;
            autoShootCoroutine = StartCoroutine(AutoShoot());
            AudioManager.instance.Play("Shoot");
        }
    }


    private void StopAutoShoot()
    {
        if (isAutoShooting)
        {
            isAutoShooting = false;
            StopCoroutine(autoShootCoroutine);
            AudioManager.instance.Stop("Shoot");
        }

        if(playerHealth.playerHP <= 0)
        {
            isAutoShooting = false;
            StopCoroutine(autoShootCoroutine);
            AudioManager.instance.Stop("Shoot");
            Debug.Log("Da chet");
        }
    }

    private IEnumerator AutoShoot()
    {
        yield return new WaitForSeconds(0.2f);

        while (isAutoShooting)
        {
            ShootGun();
            yield return new WaitForSeconds(shootInterval);
        }
    }


    private void ShootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletBarrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.forward + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = true;
        }
    }



    void Update()
    {
        //Cursor.visible = false;
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            animator.SetBool("isJump", false);
        }


        Vector2 input = moveAction.ReadValue<Vector2>();
        move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * currentSpeed);

        HorizontalInput = input.x;
        VerticalInput = input.y;


        animator.SetFloat("HorizontalInput", HorizontalInput);
        animator.SetFloat("VerticalInput", VerticalInput);
        animator.SetBool("isShooting", isAutoShooting);

        /*if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("isJump", true);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        currentState.UpdateState(this);

        if (gameManager.isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StopAutoShoot();
        }
        else if (!gameManager.isPaused) 
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    public void SwitchState(MovementBaseStates states)
    {
        currentState = states;
        currentState.EnterState(this);
    }
}
