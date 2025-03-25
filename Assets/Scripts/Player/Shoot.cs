using UnityEngine;


public class Shoot : MonoBehaviour
{
    private enum WeaponState
    {
        Single,
        Automatic,
        Mixed
    }

    public GameObject projectilePrefab;
    
    public Transform firePoint;
    
    public float shootForce = 20f;
    public Transform camera;
    public Transform player;
    
    public float projectileLifetime = 5f;
    
    public float spreadAngle = 5f;
    private WeaponState _weaponState;
    private int _stateWeaponNumber;
    
    void Start()
    {
        _weaponState = WeaponState.Automatic;
        _stateWeaponNumber = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _stateWeaponNumber += 1;
            if (_stateWeaponNumber > 2)
            {
                _stateWeaponNumber = 0;
            }
            _weaponState = (WeaponState)_stateWeaponNumber;
        }
        
        if (_weaponState == WeaponState.Automatic)
        {
            if (Input.GetButton("Fire1"))
            {
                ShootBullet();
            }
        }
        else if (_weaponState == WeaponState.Single)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootBullet();
            }
        }
        else if (_weaponState == WeaponState.Mixed)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                for (int i = 0; i < 3; i++)
                {
                    ShootBullet();
                }

            }
        }
    }

    void ShootBullet()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 rotation = camera.forward;
        
        if (rb != null)
        {
            Vector3 direction = CalculateSpreadDirection();
            rb.AddForce(direction * shootForce, ForceMode.Impulse);
        }
        
        Vector3 newDirection = new Vector3(rotation.x, 0, rotation.z).normalized;
        Quaternion newRotation = Quaternion.LookRotation(newDirection, Vector3.up);
        player.rotation = newRotation;
        Destroy(projectile, projectileLifetime);
    }

    private Vector3 CalculateSpreadDirection()
    {
        Vector3 baseDirection = firePoint.forward;

        float angleX = Random.Range(-spreadAngle, spreadAngle);
        float angleY = Random.Range(-spreadAngle, spreadAngle);

        Quaternion rotationX = Quaternion.Euler(angleX, 0, 0);
        Quaternion rotationY = Quaternion.Euler(0, angleY, 0);

        return rotationX * rotationY * baseDirection;
    }
}