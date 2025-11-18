using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Info")]
    [SerializeField] private float activeTime;

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

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
