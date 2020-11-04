using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Player
{
    private CharacterController Controller;
    private Camera MainCam;

    private Quaternion targetRotation;

    public Weapon CurrentWeapon;
    public Weapon[] Weapons;


    public Transform HandPosition;

    public float rotationSpeed = 450;
    public float walkSpeed = 35;
    public float runSpeed = 75;

    private Coroutine ActivePowerup;
    private WaitForSeconds PowerupTime = new WaitForSeconds(10f);
    public float superRunSpeed = 100;
    public bool SuperSpeedActive = false;
    public bool InfiniteAmmoActive = false;

    public bool isPaused = false;


    public float acceleration = 5;
    private Vector3 CurrentVelocityModifier;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        MainCam = Camera.main;

        UIHealthBar.SetMaxHealth(MaxHealth);
        UIHealthBar.SetHealth(MaxHealth);
        UIHealthBar.SetMaxHealth(MaxHealth);
        UIStaminaBar.SetMaxStamina(MaxStamina);

        CurrentHealth = MaxHealth;
        CurrentStamina = MaxStamina;
        HasPistol = true;

        UIWeaponsBar.MakeAllTransparent();

        EquipWeapon(0);
        CurrentAmmo[0] = StartingPistolAmmo;
        CurrentAmmo[1] = StartingShotgunAmmo;
        CurrentAmmo[2] = StartingSMGAmmo;

        UpdateWeaponsUI();
        EquipWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerInstance == null)
        {
            GameManagerInstance = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }


        //Character Movement
        CharacterMovement();

        //Check for weapon handling
        if (CurrentWeapon && !isPaused)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                FireWeapon(HeldWeapon);
            }
            else if (Input.GetButton("Fire1"))
                FireWeaponContinuous(HeldWeapon);
        }
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Input.GetKeyDown((i+1) + "") || Input.GetKeyDown("[" + (i+1) + "]"))
            {
                EquipWeapon(i);
                break;
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            if (isPaused)
            {
                isPaused = false;
                Time.timeScale = 1;
                UIPausePanel.gameObject.SetActive(false);
            }
            else
            {
                isPaused = true;
                Time.timeScale = 0;
                UIPausePanel.gameObject.SetActive(true);
            }
        }

        /*
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Forcing Player Stamina Damage");
            UseStamina(10);
        }
        */
    }

    void EquipWeapon(int i)
    {
        if (CheckHasWeapon(i))
        {
            if (CurrentWeapon)
            {
                Destroy(CurrentWeapon.gameObject);
            }

            CurrentWeapon = Instantiate(Weapons[i], HandPosition.position, HandPosition.rotation) as Weapon;
            SetCurrentWeapon(i);
            CurrentWeapon.transform.parent = HandPosition;
        }
    }

    bool CheckHasWeapon(int i)
    {
        if (i == 0 && HasPistol)
            return true;
        else if (i == 1 && HasShotgun)
            return true;
        else if (i == 2 && HasSMG)
            return true;
        else
            return false;

    }

    void FireWeapon(int i)
    {
        if (CurrentAmmo[i] > 0)
        {
            //SubtractAmmo(i);
            CurrentAmmo[i] = CurrentWeapon.Shoot(CurrentAmmo[i]);
            UpdateWeaponsUI();
        }
        else
        {
            Debug.Log("No Ammo!");
        }
    }

    void FireWeaponContinuous(int i)
    {
        if (CurrentAmmo[i] > 0)
        {
            //SubtractAmmo(i);
            CurrentAmmo[i] = CurrentWeapon.ShootContinuous(CurrentAmmo[i]);
            UpdateWeaponsUI();
        }
        else
        {
            Debug.Log("No Ammo!");
        }
    }


    private void CharacterMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = MainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, MainCam.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);


        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized;

        CurrentVelocityModifier = Vector3.MoveTowards(CurrentVelocityModifier, input, acceleration * Time.deltaTime);


        Vector3 motion = CurrentVelocityModifier;
        motion += Vector3.up * -8;

        if (Input.GetButtonDown("Run") && CurrentStamina >= StaminaBreakoff)
            CanRun = true;
        else if (Input.GetButtonUp("Run"))
            CanRun = false;

        if (SuperSpeedActive)
        {
            Controller.Move(motion.normalized * superRunSpeed * Time.deltaTime);
        }
        else
        {
            if (CanRun && Input.GetButton("Run") && CurrentStamina > 0)
            {
                UseStamina(MaxStamina / 1000);
                Controller.Move(motion.normalized * runSpeed * Time.deltaTime);
            }
            else
            {
                Controller.Move(motion.normalized * walkSpeed * Time.deltaTime);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "DamageZone")
        {
            float dmg = (other.gameObject.GetComponent<Enemy>().Damage);
            TakeDamage(dmg);
        }
        else if (other.tag == "Powerup")
        {
            int PowerupType = other.GetComponent<PowerUp>().PowerupID;
            Debug.Log("Powerup of type " + PowerupType + " Touched!");
            if (PowerupType == 0)
            {
                if (CurrentHealth < MaxHealth)
                {
                    RecoverDamage(2);
                    Destroy(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Powerup")
        {
            PowerUp p = other.GetComponent<PowerUp>();
            int PowerupType = p.PowerupID;
            Debug.Log("Powerup of type " + PowerupType + " Touched!");
            if (PowerupType == 0)
            {
                if (CurrentHealth < MaxHealth)
                {
                    RecoverDamage(2);
                    Destroy(other.gameObject);
                }
            }
            else if (PowerupType == 1)
            {
                if (ActivePowerup != null)
                    StopCoroutine(ActivePowerup);

                ActivePowerup = StartCoroutine(PowerupSuperSpeed());

                Destroy(other.gameObject);

            }
            else if (PowerupType == 2)
            {
                if (p.WeaponAmmoType == 0)
                {
                    int a = 6 + Random.Range(0, 8);
                    AddAmmo(p.WeaponAmmoType, a);
                    UpdateWeaponsUI();
                }
                else if (p.WeaponAmmoType == 1)
                {
                    if (!HasShotgun)
                    {
                        HasShotgun = true;
                        UIWeaponsBar.ShowShotgun();
                        AddAmmo(p.WeaponAmmoType, 5);
                    }
                    else
                    {
                        int a = 2 + Random.Range(0, 3);
                        AddAmmo(p.WeaponAmmoType, a);
                    }
                    UpdateWeaponsUI();
                }
                else if (p.WeaponAmmoType == 2)
                {
                    if (!HasSMG)
                    {
                        HasSMG = true;
                        UIWeaponsBar.ShowSMG();
                        AddAmmo(p.WeaponAmmoType, 50);
                    }
                    else
                    {
                        int a = 15 + Random.Range(10, 41);
                        AddAmmo(p.WeaponAmmoType, a);
                    }
                    UpdateWeaponsUI();
                }

                Destroy(other.gameObject);
            }
            else if (PowerupType == 3)
            {
                SuperAmmoPickup();
                Destroy(other.gameObject);
            }

        }

    }

    private IEnumerator PowerupSuperSpeed()
    {
        Debug.Log("Activating SuperSpeed...");
        SuperSpeedActive = true;
        if (CurrentStamina < MaxStamina)
            SetPlayerStamina(MaxStamina);

        yield return PowerupTime;

        Debug.Log("Deactivating Superspeed...");
        SuperSpeedActive = false;

        ActivePowerup = null;
    }

    private void SuperAmmoPickup()
    {
        AddAmmo(0, 50);
        AddAmmo(1, 10);
        AddAmmo(2, 150);
        UpdateWeaponsUI();
    }

}
