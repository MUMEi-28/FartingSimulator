using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // GLOBAL REFERENCES
    public PlayerActionsInput playerInputActions;
    private ExperienceController exp;
    public FartMeter fartMeter;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider playerCapsuleCollider;
    private BoxCollider playerBoxCollider;
    private Transform cam;
    public Rigidbody hipsRb;

    //  NPC
    public LayerMask npcMask;
    public LayerMask guardMask;

    public LayerMask kingMask;
    public Animator[] npcAnimators;

    [Header("Movements")]
    public float moveSpeed;
    public float jumpForce;
    public float fallMultiplier;
    public float rotationSpeed;
    public bool isGrounded;
    public bool hasHitGround;

    public LayerMask groundMask;

    private float jumpInput;
    private Vector2 input;
    private Vector3 movement;

    [Space]
    [Header("Ragdoll Movement")]
    public Collider[] ragdollColiders;
    public Rigidbody[] ragdollRigidBodies;
    public float ragdollWaitTime;
    public float timeSinceCollisionToGround;
    public float speedMultiplier;
    public bool ragdoll;

    private int speedHash;
    private int isGroundedHash;

    [Space]
    [Header("UI")]
    public GameObject gameOverUI;
    public GameObject castleEntranceUI;
    public float maxHealth;
    private float currentHealth;

    [Space]
    [Header("Fart")]
    public float fartingRange;
    public float fartDamage;
    public float fartingXp;
    public Collider[] commoners;
    public Collider[] guards;
    public ParticleSystem fartEffect;

    [Space]
    [Header("Character choosing")]
    public int currentCharacter = 0;
    public GameObject[] playersMesh;
    private void Awake()
    {

        playerInputActions = new PlayerActionsInput();
        playerInputActions.Enable();

        fartMeter = FindObjectOfType<FartMeter>();
        exp = FindObjectOfType<ExperienceController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerBoxCollider = GetComponent<BoxCollider>();
        cam = Camera.main.transform;


        currentCharacter = PlayerPrefs.GetInt("CurrentCharacter");
    }

    private void Start()
    {
        SetAnimationHash();
        ActivateChosenCharacter();


        currentHealth = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;

        PlayerStartingPoint();
    }
    private void Update()
    {
        playerCapsuleCollider.isTrigger = ragdoll;


        UpdateRagdollPosition();

        RagdollMode(ragdoll);


        FartingExperience();
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);

        BetterFalling();
        Fart();
        Move();

        PlayerAnimations();
        FartDamage();

    }
    private void LateUpdate()
    {
        //   RotateToCameraPosition();
    }
    private void PlayerStartingPoint()
    {
        GameObject spawnPosition = GameObject.FindGameObjectWithTag("StartingPoint");
        if (spawnPosition != null)
        {
            transform.position = spawnPosition.transform.position;
        }
        else
        {
            Debug.LogWarning("THERE IS NO STARTING POINT");
        }
    }
    private void FartDamage()
    {
        commoners = Physics.OverlapSphere(transform.position, fartingRange, npcMask);

        guards = Physics.OverlapSphere(transform.position, fartingRange, guardMask);

        if (jumpInput > 0.1f)
        {
            foreach (Collider col in commoners)
            {
                col.GetComponent<CommonerAI>().TakeDamage(fartDamage);
                col.GetComponent<CommonerAI>().runAway = true;
            }
            foreach (Collider col in guards)
            {
                col.GetComponent<GuardAi>().TakeDamage(fartDamage);
            }
            if (Physics.CheckSphere(transform.position, fartingRange, kingMask))
            {
                FindObjectOfType<KingAi>().TakeDamage(fartDamage);
            }
        }
    }
    private void FartingExperience()
    {
        if (jumpInput >= 0.1f && !isGrounded && fartMeter.currentFart > 0.1f)
        {
            exp.GainXpFarting(fartingXp);
        }
    }
    private void PlayerAnimations()
    {
        animator.SetFloat(speedHash, input.magnitude);

        animator.SetBool(isGroundedHash, !isGrounded);
    }
   
    #region Movement
    private void Move()
    {
        //IF RAGDOLL AND GROUNDED DON'T MOVE
        if (IsRagDolling() && isGrounded)
        {
            return;
        }

        input = playerInputActions.Player.Move.ReadValue<Vector2>();

        movement = new Vector3(input.x, 0, input.y);

        movement = movement.x * cam.transform.right.normalized + movement.z * cam.transform.forward.normalized;

        if (!IsRagDolling())
        {
            movement.y = 0;
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        }
        else
        {

            rb.velocity = new Vector3(movement.x * moveSpeed * speedMultiplier, rb.velocity.y, movement.z * moveSpeed * speedMultiplier);
        }

        if (input != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        /*rb.velocity = movement * moveSpeed;*/

    }
    private void Fart()
    {
        jumpInput = playerInputActions.Player.Fart.ReadValue<float>();
        //   rb.AddForce(Vector3.up * jumpForce * jumpInput, ForceMode.Impulse);
        float fart = PlayerPrefs.GetInt("CurrentFart");
      //  if (jumpInput > 0 && fartMeter.currentFart > 0)
        if (jumpInput > 0)
        {
            ragdoll = true;

            CancelInvoke("SetRagdollOff");  //  MAKES SURE THAT IT WON'T TURN OFF THE RAGDOLL WHEN THE WAIT TIME IS OVER


            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // MOVE

         //   hipsRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                 hipsRb.velocity = new Vector3(hipsRb.velocity.x, jumpForce, hipsRb.velocity.z); // FART

            fartEffect.Play();

            fartMeter.ReduceFart();

         //   ragdollCamera.Priority = 11;
        }
        else
        {
          //  ragdollCamera.Priority = 9;
        }
    }
    private void BetterFalling()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            //THIS WILL MAKE THE PLAYER FALL FASTER 
        }
    }
    #endregion

    #region Ragdoll
    private void RagdollMode(bool mode)
    {
        if (mode)
        {
            foreach (Rigidbody rb in ragdollRigidBodies)
            {
                rb.isKinematic = false; 
            }
            foreach (Collider col in ragdollColiders)
            {
                col.enabled = true;  // RETURNS FALSE IF IS NOT GROUNDED AND TRUE IF GROUNDED
            }
            animator.enabled = false;

        }
        else
        {
            foreach (Rigidbody rb in ragdollRigidBodies)
            {
                rb.isKinematic = true;
            }
            foreach (Collider col in ragdollColiders)
            {
                col.enabled = false;
            }
            animator.enabled = true;
        }

        foreach (Collider collider in ragdollColiders)
        {
            Physics.IgnoreCollision(playerBoxCollider, collider);
            Physics.IgnoreCollision(playerCapsuleCollider, collider);
        }
    }
   
    private void AlignPositionWithHips()
    {
        Transform hipsBone = ragdollColiders[0].transform;

        Vector3 originalHipsPosition = hipsBone.position;
        transform.position = hipsBone.position;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);

            hipsBone.position = originalHipsPosition;
        }

    }
    private void UpdateRagdollPosition()
    {
        if (ragdoll)
        {
            ragdollColiders[0].gameObject.transform.position = playerBoxCollider.transform.position;
        }
    }
    private void RagdollCollision(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            //      AlignPositionWithHips();

            Invoke("SetRagdollOff", ragdollWaitTime);
            Invoke("ResetRagdollRotation", ragdollWaitTime);
        }
    }
    private bool IsRagDolling()
    {
        foreach (Rigidbody rb in ragdollRigidBodies)
        {
            if (!rb.isKinematic)
            {
                return true;
            }
        }
        return false;
    }
    private void SetRagdollOff()    // METHOD USED IN INVOKE COLLISION AND JUMP
    {
        ragdoll = false;


        //      rb.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);

    }
    private void ResetRagdollRotation()
    {
        transform.rotation = Quaternion.Euler(0, transform.position.y, 0);
    }

    #endregion
    #region UI
    private void FartMeterUI()
    {
        fartMeter.currentFart = fartMeter.maxFart;
        fartMeter.UpdateFartMeter();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameOverUI.SetActive(true);

            Time.timeScale = 0;

            print("PLAYER DIED");
        }
    }
    #endregion

    #region Collision
    private void OnCollisionEnter(Collision collision)
    {
        FartMeterUI();
        RagdollCollision(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CastleEntrance"))
        {
            castleEntranceUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CastleEntrance"))
        {
            castleEntranceUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion

    #region StartMethods
    private void ActivateChosenCharacter()
    {
        for (int i = 0; i < playersMesh.Length; i++)
        {
            if (i == currentCharacter)
            {
                playersMesh[currentCharacter].SetActive(true);
            }
            else
            {
                playersMesh[i].SetActive(false);
            }
        }
    }
    private void SetAnimationHash()
    {
        speedHash = Animator.StringToHash("Speed");
        isGroundedHash = Animator.StringToHash("isGrounded");
    }
    #endregion
    private void OnDrawGizmos()
    {
        // Draw a line to show the distance of the raycast
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 1.1f);
        Gizmos.DrawWireSphere(transform.position, fartingRange);
    }
}
