using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class KingAi : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool runAway;
    public Transform player;
    public List<GameObject> guards;
    private Animator animator;
    public float maxhealth;
    public float maxTimerBeforeDestroy;

    private int speedHash;
    public float xpGivenOnDeath;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject[] guardArray = GameObject.FindGameObjectsWithTag("Guard");
        guards.AddRange(guardArray);

        speedHash = Animator.StringToHash("Speed");

    }
    private void Update()
    {
        RemoveMissingGameObjects();
        RunAway();

        animator.SetFloat(speedHash, agent.velocity.magnitude);
    }

    private void RunAway()
    {
        if (guards.Count <= 0)
        {
            Vector3 directionToPlayer = transform.position - player.transform.position;
            Vector3 newPosition = transform.position + directionToPlayer.normalized;

            agent.SetDestination(newPosition);
            print("RUNNNN");
        }
    }
    private void RemoveMissingGameObjects()
    {
        for (int i = guards.Count - 1; i >= 0; i--)
        {
            if (guards[i] == null)
            {
                guards.RemoveAt(i);
            }
        }
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
