using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Info")]
    [SerializeField] private float activeTime;

    [Header("Particles")]
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject impactParticle;


    private int damage;

    public int Damage { get => damage; set => damage = value; }

    private void OnEnable()
    {
        StartCoroutine(DeactiveAfterTimer());
    }

    private IEnumerator DeactiveAfterTimer()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }

    //when the bullet collide with something (player, enemy , obstacle)
    private void OnTriggerEnter(Collider other)
    {
        //Deactive the bullet , available in ObjectPool
        gameObject.SetActive(false);

        //Enemy
        if (other.CompareTag("Enemy"))
        {
            //Blood Particles Instantiate
            GameObject particles = Instantiate(damageParticle, transform.position, Quaternion.identity);

            //Destroy particles after 2 seconds
            Destroy(particles, 2f);
            

            //Enemy Damage
            other.GetComponent<EnemyController>().DamageEnemy(damage);

        }else if (other.CompareTag("Player"))
        {
            //TODO Blood Planel corroutine player
            
            //TODO Reduce life to Player
        }
        else
        {
            //impact particles
            GameObject particles = Instantiate(impactParticle, transform.position, Quaternion.identity);

            //Destroy particles after 2 seconds
            Destroy(particles, 2f);

        }
    }


}
