using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{   
    //bullet props
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;

    

    //shootig
    public bool isShooting, readyToShoot;

    bool allowReset = true;
    public float shootingDelay = 2f;


    //burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //spread
    public float spreadIntensity;

    private Animator animator;

    // Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    


    //shooting modes
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft  = magazineSize;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundAK_47.Play();
        }

        if(currentShootingMode == ShootingMode.Auto)
        {
            //only true if holding left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            //clicking left mouse button once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {
            Reload();
        }

        //automatic reload
        if( readyToShoot && isShooting == false && isReloading ==  false && bulletsLeft <= 0 )
        {
            Reload();

        }

        if(readyToShoot && isShooting && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if(AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
        }
    }

    private void FireWeapon()
    {

        bulletsLeft--;

        readyToShoot = false;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        animator.SetTrigger("RECOIL");
        SoundManager.Instance.shootingSoundAK_47.Play();

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //make bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //make bullet go in pointed direction
        bullet.transform.forward = shootingDirection;

        //shoot bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity,ForceMode.Impulse);
        //destroy bullet
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        if(allowReset)
        {
            Invoke("ResetShot" , shootingDelay);
            allowReset =false;

        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft >1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

        
    }

    private void Reload()
    {

        SoundManager.Instance.reloadSoundAK_47.Play();

        isReloading= true;
        Invoke("ReloadCompleted", reloadTime);

    }

    private void ReloadCompleted()
    {
        bulletsLeft =  magazineSize;
        isReloading = false;

    }

    private void ResetShot()
        {
            readyToShoot = true;
            allowReset = true;
        }

    public Vector3 CalculateDirectionAndSpread()
    {
        //shooting from middle of the screen to see where we are pointing
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray,out hit))
        {   
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting in the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction =  targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);   
        float y = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);   

        //Return the shooting direction and spread
        return direction + new Vector3(x,y,0);

    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet,float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(bullet);
        }
}
