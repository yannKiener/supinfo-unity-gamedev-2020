using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownGun : MonoBehaviour, Gun
{

    public GameObject projectileGO;
    public Vector3 projectileStartingOffset;
    public Vector3 dispersionOffset;
    public float coolDownTime;

    public AudioClip shootSound;
    public float shootVolume = 1;

    private float timeElapsedSinceShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReadyToShoot())
        {
            timeElapsedSinceShot += Time.deltaTime;
        }
    }
    
    private bool isReadyToShoot()
    {
        return gameObject.activeSelf && timeElapsedSinceShot >= coolDownTime;
    }

    public void StartShooting(Vector3 targetPosition, bool isFriendly)
    {
        if (isReadyToShoot())
        {
            //Add dispersion
            targetPosition += new Vector3(
                Random.Range(-dispersionOffset.x, dispersionOffset.x), 
                Random.Range(-dispersionOffset.y, dispersionOffset.y), 
                Random.Range(-dispersionOffset.z, dispersionOffset.z)
                );

            GameObject projectileInstance = Instantiate(projectileGO, transform.position + projectileStartingOffset, Quaternion.LookRotation(targetPosition - transform.position));
            projectileInstance.GetComponent<Projectile>().SetFriendly(isFriendly);
            SoundManager.PlaySound(gameObject, shootSound, shootVolume);
            timeElapsedSinceShot = 0;
        }
        else
        {
            //Play "empty mag" sound ?
        }
    }

    public void StopShooting()
    {
       
    }
}
