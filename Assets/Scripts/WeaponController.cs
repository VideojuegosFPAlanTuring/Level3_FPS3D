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

    public float ShootRate { get => shootRate; set => shootRate = value; }

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

        //if is Player
        if (isPlayer)
        {
            //Create a Ray from Camera to the middle of the screen
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            Vector3 targetPoint;

            //Check if the ray hit with something and adjust direction
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(5); //Get a point at 5m
            }


            bullet.GetComponent<Rigidbody>().linearVelocity = (targetPoint - barrel.position).normalized * bulletSpeed;
        }
        //If Enemy
        else
        {
            //TODO random directions near the player
            //Give velocity to Bullet
            bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;
        }
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
