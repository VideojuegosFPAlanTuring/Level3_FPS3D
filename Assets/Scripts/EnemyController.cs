using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{

    [Header("Enemy Data")]
    [SerializeField] private int currentLife;    
    [SerializeField] private int enemyScorePoint;

    [Header("Patrol")]
    [SerializeField] private GameObject patrolPointsContainer;
    private List<Transform> patrolPoints = new List<Transform>();
    private int destinationPoint = 0; //internal index to next destination
    private bool isChasing = false; //is Chasing Player


    private NavMeshAgent agent;

    private WeaponController weaponController;

    //Player Target
    private Transform playerTransform;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        weaponController = GetComponent<WeaponController>();

        //Take all the children of patrolPointContainer and add them to the patrolPoints List
        foreach (Transform child in patrolPointsContainer.transform)
            patrolPoints.Add(child);

        //First time go to Next Patrol
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        SearchPlayer();

        // Only patrol if not chasing the player
        if (!isChasing)
        {
            if (!agent.pathPending && agent.remainingDistance <= 1f)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    /// <summary>
    /// Enemy go to next destination Patrol Point
    /// </summary>
    private void GoToNextPatrolPoint()
    {
        // Choose the next point in the list as destination
        destinationPoint = (destinationPoint + 1) % patrolPoints.Count;

        // Not stop when reach the destination
        agent.stoppingDistance = 0.1f;

        // Go to the next point
        agent.SetDestination(patrolPoints[destinationPoint].position);
    }


    /// <summary>
    /// Enemy search player and go towards him
    /// </summary>
    private void SearchPlayer()
    {
        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check line of sight
        NavMeshHit hit;
        bool hasLoS = !agent.Raycast(playerTransform.position, out hit);

        if (hasLoS && distToPlayer <= 10f)
        {
            // Chase the player
            isChasing = true;
            agent.isStopped = false;
            agent.stoppingDistance = 5f;
            agent.SetDestination(playerTransform.position);
            transform.LookAt(playerTransform.position);

            // stop if too close
            if (distToPlayer < 5f)
                agent.isStopped = true;

            // Shoot if in range
            if (distToPlayer <= 7f)
            {
                if (weaponController.CanShoot()) weaponController.Shoot();
            }
        }
        else
        {
            // if not in sight or too far, stop chasing
            isChasing = false;
            agent.isStopped = false;
            agent.stoppingDistance = 0f;
        }
    }

    /// <summary>
    /// Handle when the enemy receive a bullet
    /// </summary>
    /// <param name="quantity">Damage quantity</param>
    public void DamageEnemy(int quantity)
    {
        currentLife -= quantity;
        if (currentLife <= 0)
        {    
            Destroy(gameObject); 
            //TODO Disapear Enemy with particles, fade out, and deactive enemy using Object Pool
        }
    }   
}

