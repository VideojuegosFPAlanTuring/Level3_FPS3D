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
    }

    /// <summary>
    /// Enemy go to next destination Patrol Point
    /// </summary>
    private void GoToNextPatrolPoint()
    {
        if (agent.remainingDistance <= 0.5f)
        {
            //choose next destinationPoint in the List
            //cycling to the start if necessary
            destinationPoint = (destinationPoint + 1) % patrolPoints.Count;
        }
        //restart the stopping distance to 0 to posibility the Patrol
        agent.stoppingDistance = 0;

        //set the agent to the currently destination point
        agent.SetDestination(patrolPoints[destinationPoint].position);

        

    }


    /// <summary>
    /// Enemy search player and go towards him
    /// </summary>
    private void SearchPlayer()
    {
        NavMeshHit hit;
        //if no osbtacles between enemy and player
        if (!agent.Raycast(playerTransform.position, out hit))
        {
            //go towards player only if player is at 10m distance or lower
            if (hit.distance <= 10f)
            {
                agent.SetDestination(playerTransform.position);
                agent.stoppingDistance = 5f;
                transform.LookAt(playerTransform.position);
                isChasing = true; //Chasing Player

                //Stop Enemy at 5m
                if (hit.distance < 5f)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }


                //shoot Player if distance is lower than 7m
                if (hit.distance <= 7f)
                {
                    if (weaponController.CanShoot()) weaponController.Shoot();
                }
            }
            //if the player is more than 10f distance
            else
            {
                agent.isStopped = false;
                isChasing = false;                
            }            
        }
        //Player not in the Ray Cast
        else
        {
            agent.isStopped = false;
            isChasing = false;            
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

