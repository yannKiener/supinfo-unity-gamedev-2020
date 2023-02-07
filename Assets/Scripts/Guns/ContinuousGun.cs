using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousGun : MonoBehaviour, Gun
{

    public GameObject projectileGO;
    public Vector3 projectileStartingOffset;
    public float maxFuel = 100;
    public float fuelConsumePerSec = 1;

    public AudioClip shootSound;
    public AudioClip stopShootingSound;
    public float shootVolume = 1;

    private float currentFuel;
    private bool isShooting = false;
    private GameObject projectileInstance;


    // Start is called before the first frame update
    void Start()
    {
        //currentFuel = maxFuel ;
    }

    public void StartShooting(Vector3 targetPosition, bool isFriendly)
    {
        if (isAbleToShoot())
        {
            if (!isShooting)
            {
                isShooting = true;
                projectileInstance =  Instantiate(projectileGO, transform.position + projectileStartingOffset, Quaternion.LookRotation(targetPosition - transform.position));
                projectileInstance.GetComponent<Projectile>().SetFriendly(isFriendly);
                currentFuel -= 1;
                SoundManager.StopSound(gameObject, stopShootingSound);
                SoundManager.PlaySound(gameObject, shootSound, shootVolume);
            } else
            {
                projectileInstance.transform.position = transform.position + projectileStartingOffset;
                projectileInstance.transform.LookAt(targetPosition);
                currentFuel -= Time.deltaTime * fuelConsumePerSec;
                //Debug.Log("Current fuel : " + currentFuel);
            }
        }
        else
        {

            //Auto stop shooting if not able
            if (isShooting)
            {
                StopShooting();
            }
            //Debug.Log("NOT ABLE TO SHOOT");
            //Play "No Fuel" sound ?
        }
    }

    public void StopShooting()
    {
        if (isShooting)
        {
            SoundManager.StopSound(gameObject, shootSound);
            SoundManager.PlaySound(gameObject, stopShootingSound, shootVolume);
        }
        GameObject.Destroy(projectileInstance);
        isShooting = false;
    }

    private bool isAbleToShoot()
    {
        return gameObject.activeSelf && currentFuel > 0;
    }

    public float GetFuelPercent()
    {
        return currentFuel / maxFuel;
    }

    public float GetCurrentFuel()
    {
        return currentFuel;
    }

    public void UseFuel(float fuel)
    {
        currentFuel -= fuel;
        if (currentFuel < 0)
        {
            currentFuel = 0;
        }
    }

    public void RefillFuel(float fuel)
    {
        currentFuel += fuel;
        if(currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

}
