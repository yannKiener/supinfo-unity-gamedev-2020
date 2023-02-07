using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is intended to be attached at the main Camera 
public class GameController : MonoBehaviour
{
    //And this GameObject is intended to be the player's ship.
    public GameObject shipGO;

    public float maxMovementSpeed = 1f;
    public float timeFromZeroToMax = 1f;
    public float speed = 15;


    public Texture2D barBackGround;
    public Texture2D fuelBarFull;
    public Texture2D healthBarFull;
    public Texture2D bossHealthBar;
    public Vector2 fuelBarPosition;
    public Vector2 fuelBarSize;
    public Vector2 healthBarPosition;
    public Vector2 healthBarSize;
    public List<AudioClip> repairSounds;
    public int repairCount = 100;
    public int fuelCost;
    public float repairCooldown = 1;

    public GameObject inGameMenu;
    public GameObject deathBackGround;

    public FixedJoystick androidShipController;
    public float cursorSpeedMultiplier;
    public FixedJoystick androidCursorController;
    public GameObject androidCursor;


    private Camera mainCamera;
    private Unit shipPlayer;
    private float horizontalSpeed;
    private float verticalSpeed;
    Vector3 targetPosition;
    private bool showDeathScreen = false;
    private bool showInGameMenu = false;
    private float repairTimer = 0;
    private bool startShootingNormal = false;
    private bool startShootingSpecial = false;


    private void Awake()
    {
        //To force tag on the player's ship
        shipGO.tag = "Player";
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        shipPlayer = shipGO.GetComponent<Unit>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (shipPlayer.IsAlive())
        {
            UpdateShooting();
            UpdateAim();
            UpdateMovements();
            //Move the camera
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        } else
        {
            showDeathScreen = true;
            deathBackGround.SetActive(showDeathScreen);
        }
         
    }

    private bool IsAndroid()
    {
        //return true;
        return SystemInfo.deviceType == DeviceType.Handheld;

    }
    //Ship movements controls
    private void UpdateMovements()
    {

        float moveHorizontalTowards = 0;
        float moveVerticalTowards = 0;
        if (IsAndroid())
        {
            moveHorizontalTowards = androidShipController.Horizontal;
            moveVerticalTowards = androidShipController.Vertical;
        } else
        {
            moveHorizontalTowards = Input.GetAxis("Horizontal");
            moveVerticalTowards = Input.GetAxis("Vertical");
        }

        float changeRatePerSecond = 1 / timeFromZeroToMax * Time.deltaTime;

        /*
        if (Input.GetKey(KeyCode.Z))
        {
            moveVerticalTowards = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVerticalTowards = -1.0f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            moveHorizontalTowards = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontalTowards = 1.0f;
        }
        */

        /*
        if (Input.GetKey(KeyCode.LeftShift))
        {
            changeRatePerSecond *= 2;
        }
        */

        horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, moveHorizontalTowards, changeRatePerSecond);
        verticalSpeed = Mathf.MoveTowards(verticalSpeed, moveVerticalTowards, changeRatePerSecond);

        if(Time.timeScale != 0)
        {
            shipGO.transform.Translate(new Vector3(horizontalSpeed * maxMovementSpeed, verticalSpeed * maxMovementSpeed, 0), transform);
        }
    }

    public void ToggleInGameMenu()
    {
        if (!showDeathScreen)
        {
            showInGameMenu = !showInGameMenu;
            if (showInGameMenu)
            {
                Time.timeScale = 0;
            } else
            {
                Time.timeScale = 1;
            }
            inGameMenu.SetActive(showInGameMenu);
        }
    }

    public void ToggleShootingNormal()
    {
        startShootingNormal = !startShootingNormal;
    }

    public void ToggleShootingSpecial()
    {
        startShootingSpecial = !startShootingSpecial;
    }

    public void PlayerShootNormal()
    {
        shipPlayer.Shoot(mainCamera.ScreenToWorldPoint(targetPosition));
    }

    public void PlayerShootSpecial()
    {
        shipPlayer.ShootSpecial(mainCamera.ScreenToWorldPoint(targetPosition));
    }

    public void PlayerStopShootingSpecial()
    {
        shipPlayer.StopShootingSpecial();
    }

    public void PlayerRepairShip()
    {
        if (repairTimer >= repairCooldown && shipPlayer.GetHealthPercent() < 1 && shipPlayer.GetSpecialGun().GetCurrentFuel() >= fuelCost)
        {
            shipPlayer.Repair(repairCount, Utils.GetRandomClip(repairSounds), 1, fuelCost);
            repairTimer = 0;
        }
    }

    //Move the ship around the mouse position projected on the "enemyGrid"
    private void UpdateAim()
    {
        if (IsAndroid())
        {
            androidCursor.transform.Translate(androidCursorController.Direction * cursorSpeedMultiplier);
            targetPosition = androidCursor.transform.position;
        } else
        {
            targetPosition = Input.mousePosition;
        }

        targetPosition.z = 35; //TODO : Faire en sorte que ca soit pas en dur mais par rapport a la position du "enemyGrid"
        shipGO.transform.LookAt(mainCamera.ScreenToWorldPoint(targetPosition));
        shipGO.transform.rotation = shipGO.transform.rotation * shipPlayer.GetShipBaseRotation();
    }

    private void UpdateShooting()
    {
        repairTimer += Time.deltaTime;

        if(IsAndroid())
        {
            if (startShootingNormal)
            {
                PlayerShootNormal();
            }
            if(startShootingSpecial)
            {
                PlayerShootSpecial();
            }
            if (!startShootingSpecial)
            {
                PlayerStopShootingSpecial();
            }
        } else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                PlayerShootNormal();
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                PlayerShootSpecial();
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                PlayerStopShootingSpecial();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleInGameMenu();
            }

            //Repair if cooldown OK, life is not full, and has enough fuel.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerRepairShip();
            }
        }


        //Test zone 
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            shipPlayer.AddSpecialGun();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            shipPlayer.RemoveSpecialGun();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
           //shipPlayer.RefillFuel(50);
           // GameObject.Find("EnemyGrid").GetComponent<SpawnManager>().spawnRandomBoss();
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            shipPlayer.AddGun();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            shipPlayer.RemoveGun();
        }
        */
    }

    //To draw healthbar & fuelBar 
    private void OnGUI()
    {
        if (shipPlayer.IsAlive())
        {
            //Health bar (bottom left)

            int healthBarX = (int)(Screen.width * healthBarPosition.x / 100);
            int healthBarY = (int)(Screen.height * healthBarPosition.y / 100);
            int healthBarSizeX = (int)(Screen.width * healthBarSize.x / 100);
            int healthBarSizeY = (int)(Screen.height * healthBarSize.y / 100);

            GUI.Box(new Rect(healthBarX, healthBarY, healthBarSizeX, healthBarSizeY), barBackGround);
            GUI.Box(new Rect(healthBarX, healthBarY, healthBarSizeX * shipPlayer.GetHealthPercent(), healthBarSizeY), healthBarFull);


            //Fuel bar (bottom right)
            int fuelBarX = (int)(Screen.width * fuelBarPosition.x / 100);
            int fuelBarY = (int)(Screen.height * fuelBarPosition.y / 100);
            int fuelBarSizeX = (int)(Screen.width * fuelBarSize.x / 100);
            int fuelBarSizeY = (int)(Screen.height * fuelBarSize.y / 100);
            GUI.Box(new Rect(fuelBarX, fuelBarY, fuelBarSizeX, fuelBarSizeY), barBackGround);

            float fuelPercent = shipPlayer.GetSpecialGun().GetFuelPercent();
            if (fuelPercent > 0)
            {
                GUI.Box(new Rect(fuelBarX, fuelBarY, fuelBarSizeX * shipPlayer.GetSpecialGun().GetFuelPercent(), fuelBarSizeY), fuelBarFull);
            }
        }

    }
}
