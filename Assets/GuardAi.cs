using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float damageToGive;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform shootBarel;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float maxHealth;
    public float currentHealth;
    public float maxTimerBeforeDestroy;

    //ANIMATIONS
    private int runHash;
    private int attackHash;

    public bool isMoving;
    private Vector3 lastPos;

    public float xpGivenOnDeath;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    //    agent.enabled = false;
        animator = GetComponent<Animator>();

        runHash = Animator.StringToHash("Speed");
        attackHash = Animator.StringToHash("Attack");

        lastPos = transform.position;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }

        ChaseAnimation();
    }
   
    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (agent.enabled)
        {
            agent.SetDestination(player.position);
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, transform.position.y,player.position.z));
        shootBarel.transform.LookAt(player);    //  THIS MAKES SURE THAT THE BULLET CAN GO UP WHEN THE PLAYER IS ABOVE GROUND

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile,shootBarel.transform.position, shootBarel.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage * Time.deltaTime;


        if (currentHealth <= 0) Invoke(nameof(DestroyEnemy), maxTimerBeforeDestroy);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void ChaseAnimation()
    {
        
        var displacement = transform.position - lastPos;
        lastPos = transform.position;
        animator.SetFloat(runHash, displacement.magnitude);

        //   displacement.magnitude > 0.001; // return true if char moved 1mm
    }

    private void OnCollisionEnter(Collision collision)
    {
        /* agent.enabled = true;
         if (collision.gameObject.tag == "PlayerCollision")
         {
             collision.gameObject.GetComponent<PlayerControls>().TakeDamage(damageToGive);
         }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void OnDestroy()
    {
        ExperienceController experienceController = FindObjectOfType<ExperienceController>();
        if (experienceController != null)
        {
            experienceController.GainXpDefeating(xpGivenOnDeath);
            print("EXP GIVEN");
        }
    }
}
