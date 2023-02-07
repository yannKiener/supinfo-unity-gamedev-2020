using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float zSpawnDistance = 500;
    public Vector2 xSpawnRange;
    public Vector2 ySpawnRange;
    public float zMoveSpeed = 50;
    public float zLimit = 1;
    public List<GameObject> spawnSquadron;
    public float spawnSquadronTimer;
    public List<GameObject> spawnUnique;
    public float spawnUniqueTimer;
    public List<GameObject> spawnBoss;
    public float spawnBossTimer;
    public float spawnEnemyGroundTimer;
    public float spawnFriendlyGroundTimer;
    public Vector2 xSpawnRangeGround;
    public List<GameObject> spawnEnemyGround;
    public float zSpawnDistanceGround;
    public List<GameObject> spawnFriendlyGround;
    public float zSpawnDistanceFriendlyGround;
    public float destroyGroundAfterDelay;
    public GameObject fuelTank;
    public float spawnFuelTankTimer;

    public float divideTimersEveryPoints;
    public float divideTimersBy;

    private List<GameObject> spawningShips = new List<GameObject>();
    private readonly float destroyDelay = 2f;
    //Negative to let the player breathe a bit at the beginning
    private float spawnUniqueCooldown = -30;
    private float spawnBossCooldown = -30;
    private float spawnSquadronCooldown = -10;
    private float spawnEnemyGroundCoolDown = 0;
    private float spawnFriendlyGroundCoolDown = 0;
    private float spawnFuelTankCoolDown = 0;

    private void ReduceTimers()
    {
        if(divideTimersBy == 0)
        {
            Debug.Log("Haha. Very funny.");
        } else
        {
            spawnBossTimer /= divideTimersBy;
            spawnEnemyGroundTimer /= divideTimersBy;
            spawnFriendlyGroundTimer /= divideTimersBy;
            spawnSquadronTimer /= divideTimersBy;
            spawnUniqueTimer /= divideTimersBy;
        }
    }

    public void spawnRandomSquadron()
    {
        if (spawnSquadron.Count != 0)
        {
            GameObject instanciedGO = Instantiate(GetRandomFromList(spawnSquadron), transform);
            spawnShip(instanciedGO);
        }
    }


    public void spawnRandomUnique()
    {
        if (spawnUnique.Count != 0)
        {
            GameObject instanciedGO = Instantiate(GetRandomFromList(spawnUnique), transform);
            spawnShip(instanciedGO);
        }
    }

    public void spawnRandomBoss()
    {
        if (spawnBoss.Count != 0)
        {
            GameObject instanciedGO = Instantiate(GetRandomFromList(spawnBoss), transform);
            spawnShip(instanciedGO);
        }
    }

    private void spawnShip(GameObject go)
    {
        go.transform.localPosition = new Vector3(Random.Range(xSpawnRange.x, xSpawnRange.y), Random.Range(ySpawnRange.x, ySpawnRange.y), zSpawnDistance);
        go.tag = "EnemyShip";
        SetAllAliveTo(go, false);
        spawningShips.Add(go);
    }

    public void spawnRandomEnemyGround()
    {
        if (spawnEnemyGround.Count != 0)
        {
            GameObject instanciedGO = Instantiate(GetRandomFromList(spawnEnemyGround));
            instanciedGO.transform.position = new Vector3(Random.Range(xSpawnRangeGround.x, xSpawnRangeGround.y), -54, zSpawnDistanceGround + transform.position.z);
            Destroy(instanciedGO, destroyGroundAfterDelay);
        }
    }

    public void spawnRandomFriendlyGround()
    {
        if (spawnFriendlyGround.Count != 0)
        {
            GameObject instanciedGO = Instantiate(GetRandomFromList(spawnFriendlyGround));
            instanciedGO.transform.position = new Vector3(Random.Range(xSpawnRangeGround.x, xSpawnRangeGround.y), -55, zSpawnDistanceFriendlyGround + transform.position.z);
            StartCoroutine(DestroyFriendly(instanciedGO, destroyGroundAfterDelay));
        }
    }

    //To add friendly saved count on Destroy
    IEnumerator DestroyFriendly(GameObject friendInstance, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (friendInstance != null && friendInstance.activeSelf)
        {
            GameUtils.AddFriendlySaved();
            Destroy(friendInstance);
        }
    }

    public void spawnFuelTank()
    {
        if(fuelTank != null)
        {
            GameObject instanciedGO = Instantiate(fuelTank);
            instanciedGO.transform.position = new Vector3(Random.Range(xSpawnRangeGround.x, xSpawnRangeGround.y), -54f, zSpawnDistanceGround + transform.position.z);
        }
    }

    private GameObject GetRandomFromList(List<GameObject> gameObjects)
    {
        return gameObjects[Random.Range(0, gameObjects.Count)];
    }


    private void Update()
    {
        //If player gets to a point cap, we reduce every spawn timers (but not fuels).
        if(GameUtils.GetScore().GetCount() >= divideTimersEveryPoints)
        {
            divideTimersEveryPoints += divideTimersEveryPoints;
            ReduceTimers();
        }
        UpdateSpawn();
        UpdateSpawningShips();
    }

    private void UpdateSpawn()
    {

        if (spawnBossCooldown > spawnBossTimer)
        {
            spawnRandomBoss();
            spawnBossCooldown = 0;
        }

        if (spawnUniqueCooldown > spawnUniqueTimer)
        {
            spawnRandomUnique();
            spawnUniqueCooldown = 0;
        }
        //For squadrons, we force spawn them if played killed everything (But not at the beginning aka if spawnSquadronCooldown is negative);
        if ((GameObject.FindGameObjectsWithTag("EnemyShip").Length <= 0 && spawnSquadronCooldown >= 0) || spawnSquadronCooldown > spawnSquadronTimer)
        {
            spawnRandomSquadron();
            spawnSquadronCooldown = 0;
        }
        if (spawnEnemyGroundCoolDown > spawnEnemyGroundTimer)
        {
            spawnRandomEnemyGround();
            spawnEnemyGroundCoolDown = 0;
        }
        if (spawnFriendlyGroundCoolDown > spawnFriendlyGroundTimer)
        {
            spawnRandomFriendlyGround();
            spawnFriendlyGroundCoolDown = 0;
        }
        if (spawnFuelTankCoolDown > spawnFuelTankTimer)
        {
            spawnFuelTank();
            spawnFuelTankCoolDown = 0;
        }

        spawnBossCooldown += Time.deltaTime;
        spawnUniqueCooldown += Time.deltaTime;
        spawnSquadronCooldown += Time.deltaTime;
        spawnFuelTankCoolDown += Time.deltaTime;
        spawnEnemyGroundCoolDown += Time.deltaTime;
        spawnFriendlyGroundCoolDown += Time.deltaTime;
    }

    private void UpdateSpawningShips()
    {
        foreach(GameObject go in spawningShips)
        {

            go.transform.Translate(new Vector3(0, 0, zMoveSpeed /transform.localPosition.z * Time.deltaTime));

            //If ship has arrived, we set it alive.
            if (go.transform.localPosition.z <= zLimit)
            {
                SetAllAliveTo(go, true);
                //If GameObject is only a container, destroy it to avoid leaving a lot of useless gameObjects
                if(go.GetComponent<Unit>() == null)
                {
                    Destroy(go, destroyDelay);
                }
            }
        }
        //Remove ships that has arrived from updateList
        spawningShips.RemoveAll(go => go.transform.localPosition.z <= zLimit);
    }


    //Recursively set alive a ship or all ships in GameObject 
    private void SetAllAliveTo(GameObject go, bool isAlive)
    {
        if (go.GetComponent<Unit>() != null)
        {
            go.GetComponent<Unit>().SetAlive(isAlive);
            //If it's arrived, change the parent to this.transform if it's not already
            if(isAlive && !go.transform.parent.name.Equals("EnemyGrid"))
            {
                StartCoroutine(DetachParentAfterDelay(go));
            }
        } else
        {
            foreach (Transform child in go.transform)
            {
                SetAllAliveTo(child.gameObject, isAlive);
            }
        }
    }

    IEnumerator DetachParentAfterDelay(GameObject go)
    {
        yield return new WaitForSeconds(destroyDelay/2);
        go.transform.parent = transform;
        go.tag = "EnemyShip";
    }

}
