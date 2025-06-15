using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommonerAI : MonoBehaviour
{
    public float walkPointRange = 70;
    public LayerMask groundMask;
    public bool walkPointSet = false;
    public Vector3 walkPoint;
    public float waitSecond;
    public NavMeshAgent agent;

    public float maxTimerBeforeDestroy;
    public float maxhealth;
    public bool runAway;
    public float minDistance;
    public float maxDistance;
    public float runAwayRadius;

    private Animator animator;
    int runHash;


    public float xpGivenOnDeath;

    private GameObject player;
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        runHash = Animator.StringToHash("speed");
    }
    private void Update()
    {
        if (!runAway)
        {
            Wander();
        }
        else
        {
            RunAway();
        }

        animator.SetFloat(runHash, agent.velocity.magnitude);
    }
    private void Wander()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
            
   //     walkPoint.y = transform.position.y; // THIS MAKES SURE THAT THE MAGNITUDE OF THE X Y Z WALKPOINT IS NEAR THE WALKPOINT SETTED
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            Invoke(nameof(SetWalkPoint), waitSecond);

        }

    }
    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }
    private void RunAway()
    {
        walkPoint = Vector3.zero;
        walkPointSet = false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the commoner is too close to the player, move away from the player.
        if (distanceToPlayer < minDistance)
        {
            Vector3 directionToPlayer = transform.position - player.transform.position;
            Vector3 newPosition = transform.position + directionToPlayer.normalized * (minDistance - distanceToPlayer);
            agent.SetDestination(newPosition);
        }
        // If the commoner is too far from the player, move towards the player.
        else if (distanceToPlayer > maxDistance)
        {
            runAway = false;
            //       agent.SetDestination(player.transform.position);
        }
    }

    private void SetWalkPoint()
    {
        walkPointSet = false;
    }
    public void TakeDamage(float damage)
    {
        maxhealth -= damage * Time.deltaTime;

        if (maxhealth <= 0)
        {
            Destroy(gameObject, maxTimerBeforeDestroy);
        }
    }
    private void OnDestroy()
    {
        ExperienceController experienceController = FindObjectOfType<ExperienceController>();
        if (experienceController != null)
        {
            experienceController.GainXpDefeating(xpGivenOnDeath);
        }
    }
}
