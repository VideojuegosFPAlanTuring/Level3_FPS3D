using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [Header("Enemy Data")]
    [SerializeField] private int currentLife;
    [SerializeField] private int maxLife;
    [SerializeField] private int enemyScorePoint;


    private NavMeshAgent agent;

    private WeaponController weaponController;

    //Player Target
    private Transform playerTransform;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        weaponController = GetComponent<WeaponController>();
    }

    private void Update()
    {
        SearchPlayer();
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
            }

            //shoot Player if distance is lower than 7m
            if (hit.distance <= 7f)
            {
                if (weaponController.CanShoot()) weaponController.Shoot();
            }
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

