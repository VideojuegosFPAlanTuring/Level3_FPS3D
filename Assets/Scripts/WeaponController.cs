using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    [Header("Ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [Header("Performance")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootRate;
    [SerializeField] private int damage;

    private ObjectPool objectPool;
    private float lastShootTime;

    private bool isPlayer;

    private void Awake()
    {
        //Check if Ia am a Player
        isPlayer = gameObject.CompareTag("Player");

        //get objectPool
        objectPool = GetComponent<ObjectPool>();
        
    }

    /// <summary>
    /// Handle Weapon Shoot
    /// </summary>
    public void Shoot()
    {
        //update last shoot time
        lastShootTime = Time.time;

        if (!infiniteAmmo) currentAmmo--;

        //Get a new active bullet
        GameObject bullet = objectPool.GetGameObject();
        
        //position and rotation
        bullet.transform.position = barrel.position;
        bullet.transform.rotation = barrel.rotation;
        
        //assign damage
        bullet.GetComponent<BulletController>().Damage = damage;

        //Give velocity to Bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;

    }

    /// <summary>
    /// check if it is posible to shoot
    /// </summary>
    /// <returns></returns>
    public bool CanShoot()
    {
        //Check shootRate
        if (Time.time - lastShootTime >= shootRate)
        {
            //Check Ammo
            if (currentAmmo > 0 || infiniteAmmo)
            {
                return true;
            }

        }

        return false;
    }


}
