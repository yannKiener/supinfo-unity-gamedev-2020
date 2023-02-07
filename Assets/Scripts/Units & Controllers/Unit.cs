using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public int maxLife;
    public bool isFriendly;
    public bool isGroundUnit;
    public int pointOnDeath = 10;
    public List<CooldownGun> gunList;
    public float enemyMaxShootDelay;
    public ContinuousGun specialGun;
    public GameObject explosionParticle;
    public float explosionScale = 1;
    public GameObject impactParticle;
    public float impactScale = 1;

    public float timeFromZeroToMaxSpeed = 1f;
    public float maxMovementSpeed = 1f;
    public float minDelayMovementChange = 1;
    public float maxDelayMovementChange = 3;

    public AudioClip deathSound;
    public float deathVolume = 1;

    private float currentLife;
    private bool isAlive = true;
    private Quaternion baseShipRotation;
    private float destroyParticlesAfter = 5;
    private bool isEnemyShooting = false;
    private float horizontalSpeed;
    private float verticalSpeed;
    private int moveHorizontalTowards = 0;
    private int moveVerticalTowards = 0;
    private bool isEnemyMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
        baseShipRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isAlive)
        {
            UpdateLife();
            UpdateMovements();

            //Manage enemy "IA"
            if (!isFriendly)
            {
                UpdateEnemyShooting();
            }
        }
    }

    //Used to "deactivate" IA while spawning
    public void SetAlive(bool isAlive)
    {
        this.isAlive = isAlive;
    }

    private void UpdateMovements()
    {
        if (Time.timeScale != 0)
        {
            //Move ground units forward
            if (isGroundUnit)
            {
                if (isFriendly)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.forward * maxMovementSpeed;
                    maxMovementSpeed -= Time.deltaTime;
                }
                else
                {
                    GetComponent<Rigidbody>().velocity = Vector3.forward * - maxMovementSpeed;
                }
            }
            //Move enemy ships
            else if(!isFriendly)
            {
                //Cannot use rigidBody.Addforce with isKinematic :'(  
                if (!isEnemyMoving)
                {
                    StartCoroutine(ChangeDirectionAfterDelay(Random.Range(minDelayMovementChange, maxDelayMovementChange)));
                }

                float changeRatePerSecond = 1 / timeFromZeroToMaxSpeed * Time.deltaTime;

                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, moveHorizontalTowards, changeRatePerSecond);
                verticalSpeed = Mathf.MoveTowards(verticalSpeed, moveVerticalTowards, changeRatePerSecond);
                transform.Translate(new Vector3(horizontalSpeed * maxMovementSpeed, verticalSpeed * maxMovementSpeed, 0), transform);
            }
        }
    }

    IEnumerator ChangeDirectionAfterDelay(float time)
    {
        isEnemyMoving = true;
        yield return new WaitForSeconds(time);

        //can be : -1, 0, 1.
        moveHorizontalTowards = Random.Range(-1, 2);
        moveVerticalTowards = Random.Range(-1, 2);

        isEnemyMoving = false;
    }

    private void UpdateEnemyShooting()
    {
        if (!isEnemyShooting && GameUtils.IsPlayerAlive())
        {
            StartCoroutine(ShootAtPlayerAfterDelay(Random.Range(0, enemyMaxShootDelay), GameUtils.GetPlayerGameObject()));
        }
    }

    //Target gameObject is set with parameter in case we want to shoot something else than the player one day.
    IEnumerator ShootAtPlayerAfterDelay(float time, GameObject playerGameObject)
    {
        isEnemyShooting = true;
        yield return new WaitForSeconds(time);

        if (playerGameObject.activeSelf && playerGameObject.GetComponent<Unit>().IsAlive())
        {
            Shoot(playerGameObject.transform.position);
        }

        isEnemyShooting = false;
    }

    private void UpdateLife()
    {
        if(currentLife <= 0)
        {
            DestroyUnit();
        }
    }

    public void DestroyUnit()
    {
        isAlive = false;
        StopShooting();
        StopShootingSpecial();

        if (!isFriendly)
        {
            GameUtils.GetScore().Addcount(pointOnDeath);

            //Add gun on ground unit kill
            if (isGroundUnit && GameUtils.IsPlayerAlive())
            {
                GameUtils.GetPlayerGameObject().GetComponent<Unit>().AddGun();
                GameUtils.AddEnemyGroundKill();
            } else
            {
                GameUtils.AddShipKill();
            }
        } else
        {
            if (GameUtils.IsPlayerAlive())
            {
                GameUtils.AddFriendlyGroundKill();
                //Remove 2 gun if Friendly is Killed !
                GameUtils.GetPlayerGameObject().GetComponent<Unit>().RemoveGun();
                GameUtils.GetPlayerGameObject().GetComponent<Unit>().RemoveGun();
            }
        }

        //Destroy animation
        if (explosionParticle != null)
        {
            //Make ship disappear first
            makeUnitDisappear(gameObject);
            //And play explosion
            GameObject explosionParticleInstance = Instantiate(explosionParticle, transform);
            SoundManager.PlaySound(explosionParticleInstance, deathSound, deathVolume);
            Utils.PlayAllParticleSystemFrom(explosionParticleInstance, explosionScale, true, destroyParticlesAfter);

            //Destroy after animation + time offset for "other child effects"
            Destroy(gameObject, destroyParticlesAfter);
        } else
        {
            //If no explosion is set, destroy gameObject
            Destroy(gameObject);
        }
    }

    //Remove existing particles and MeshRenderer
    private void makeUnitDisappear(GameObject go)
    {
        MeshRenderer renderer = go.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Destroy(renderer);
        }
        ParticleSystem part = go.GetComponent<ParticleSystem>();
        if (part != null)
        {
            Destroy(part);
        }
        foreach(Transform child in go.transform)
        {
            makeUnitDisappear(child.gameObject);
        }
    }


    public void Shoot(Vector3 targetPosition)
    {
        if (isAlive)
        {
            foreach (Gun gun in gunList)
            {
                gun.StartShooting(targetPosition, isFriendly);
            }
        }
    }

    public void StopShooting()
    {
        foreach (Gun gun in gunList)
        {
            gun.StopShooting();
        }
    }

    public void ShootSpecial(Vector3 targetPosition)
    {
        if (isAlive && specialGun != null)
        {
            specialGun.StartShooting(targetPosition, isFriendly);
        }
    }

    public void StopShootingSpecial()
    {
        if(specialGun != null)
        {
            specialGun.StopShooting();
        }
    }

    //Used to 'counter' the model's base rotation
    public Quaternion GetShipBaseRotation()
    {
        return baseShipRotation;
    }

    //For adding & removing guns 
    public void AddGun()
    {
        //We iterate over guns and activate the first inactive.
        for(int i = 0; i < gunList.Count; i++)
        {
            CooldownGun currentGun = gunList[i];
            if (!currentGun.gameObject.activeSelf)
            {
                currentGun.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RemoveGun()
    {

        //We iterate over guns starting at the end  to deactivate the first active found.
        int stopAtCount = 0;
        //If it's the player (alive), leave 1 gun available!
        if (gameObject == GameUtils.GetPlayerGameObject() && isAlive)
        {
            stopAtCount = 1;
        }

        for (int i = gunList.Count -1 ; i >= stopAtCount; i--)
        {
            CooldownGun currentGun = gunList[i];
            if (currentGun.gameObject.activeSelf)
            {
                currentGun.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void AddSpecialGun()
    {
        if (!specialGun.gameObject.activeSelf)
        {
            specialGun.gameObject.SetActive(true);
        }
    }

    public void RemoveSpecialGun()
    {
        if (specialGun != null && specialGun.gameObject.activeSelf)
        {
            specialGun.gameObject.SetActive(false);
        }
    }

    public void Damage(Vector3 contactPoint, float damageCount)
    {
        if (isAlive)
        {
            if (impactParticle != null)
            {
                GameObject impactParticuleInstance = Instantiate(impactParticle, contactPoint, gameObject.transform.rotation);
                Utils.PlayAllParticleSystemFrom(impactParticuleInstance, impactScale, false, destroyParticlesAfter);
            }
            currentLife -= damageCount;
            //Debug.Log("Life left : " + currentLife);
        }
    }

    //Used only by player for now 
    //This is why Audioclip is passed through a parameter
    //Also, checks and cooldowns are managed by GameController.cs to avoid everyShip to have a cooldown running
    public void Repair(float repairCount, AudioClip repairSound = null, float repairVolume = 0, float fuelCost = 0)
    {
        if (isAlive)
        {
            currentLife += repairCount;
            if(currentLife > maxLife)
            {
                currentLife = maxLife;
            }
            SoundManager.PlaySound(gameObject, repairSound, repairVolume);
            UseFuel(fuelCost);
            //Debug.Log("Life left : " + currentLife);
        }
    }

    public void RefillFuel(float fuel)
    {
        specialGun.RefillFuel(fuel);
    }
    
    public void UseFuel(float fuel)
    {
        specialGun.UseFuel(fuel);
    }

    public ContinuousGun GetSpecialGun()
    {
        return specialGun;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public float GetHealthPercent()
    {
        return currentLife / maxLife;
    }

    private void OnCollisionEnter(Collision col)
    {
        Terrain terrainHit = col.gameObject.GetComponent<Terrain>();

        //InstantKill ships if hit terrain
        if (!isGroundUnit)
        {
            if (terrainHit != null)
            {
                //Debug.Log(name + " JUST HIT TERRAIN");
                currentLife = 0;
            }
        }
        else
        {
            //InstantKill ground units if hit something else
            if (terrainHit == null)
            {
                Unit unitCollision = col.gameObject.GetComponent<Unit>();
                if (unitCollision != null )
                {
                    if(isFriendly && !unitCollision.isFriendly && unitCollision.IsAlive())
                    {
                        currentLife = 0;
                    }
                    if (!isFriendly && unitCollision.isFriendly && unitCollision.IsAlive())
                    {
                        currentLife = 0;
                    }

                } else
                {
                    currentLife = 0;
                }
            }
        }
    }
}
