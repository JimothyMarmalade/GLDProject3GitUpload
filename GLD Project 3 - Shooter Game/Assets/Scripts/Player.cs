using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public HealthBar UIHealthBar;
    public StaminaBar UIStaminaBar;
    public UIWeapons UIWeaponsBar;
    public GameOverPanel UIGameOverPanel;
    public PausePanel UIPausePanel;

    public GameManager GameManagerInstance;

    private Coroutine StaminaReg;


    public int EnemiesKilled;

    public int HeldWeapon;
    public int[] CurrentAmmo = new int[3];

    public bool HasPistol = true;
    public int CurrentPistolAmmo;
    public int StartingPistolAmmo;
    public int MaxPistolAmmo = 100;

    public bool HasShotgun = false;
    public int CurrentShotgunAmmo;
    public int StartingShotgunAmmo;
    public int MaxShotgunAmmo = 20;

    public bool HasSMG = false;
    public int CurrentSMGAmmo;
    public int StartingSMGAmmo;
    public int MaxSMGAmmo = 250;



    private void Start()
    {
        UIHealthBar.SetMaxHealth(MaxHealth);
        Debug.Log("Player Start(): calling UIStaminaBar.SetMaxStamina");
        UIStaminaBar.SetMaxStamina(MaxStamina);
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        

        if (CurrentHealth < 0)
            UIHealthBar.SetHealth(0);
        else
            UIHealthBar.SetHealth(CurrentHealth);
    }

    public override void Die()
    {
        UIGameOverPanel.gameObject.SetActive(true);
        UIGameOverPanel.DisplayLosingText();
        Destroy(gameObject, 2f);
    }

    public override void RecoverDamage(float dmg)
    {
        base.RecoverDamage(dmg);

        UIHealthBar.SetHealth(CurrentHealth);
    }

    public void UseStamina(float amount)
    {
        CurrentStamina -= amount;
        //Debug.Log("Stamina: " + CurrentStamina);

        if (CurrentStamina < 0)
            CurrentStamina = 0;
        if (CurrentStamina > MaxStamina)
            CurrentStamina = MaxStamina;

        UIStaminaBar.SetStamina(CurrentStamina);

        if (StaminaReg != null)
            StopCoroutine(StaminaReg);

        StaminaReg = StartCoroutine(RegenerateStamina());

    }

    public void SetPlayerStamina(float amount)
    {

        CurrentStamina = amount;
        Debug.Log("Stamina set to: " + CurrentStamina);

        if (CurrentStamina < 0)
            CurrentStamina = 0;
        if (CurrentStamina > MaxStamina)
            CurrentStamina = MaxStamina;

        UIStaminaBar.SetStamina(CurrentStamina);

        if (StaminaReg != null)
            StopCoroutine(StaminaReg);

        StaminaReg = StartCoroutine(RegenerateStamina());
    }

    public IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(2);

        while (CurrentStamina < MaxStamina)
        {
            CurrentStamina += MaxStamina / 100;
            UIStaminaBar.SetStamina(CurrentStamina);
            yield return new WaitForSeconds(0.1f);
        }

        StaminaReg = null;

    }

    public void SetCurrentWeapon(int i)
    {
        Debug.Log("Player's Held Weapon is " + i);
        HeldWeapon = i;
        //Make everything invisible that isn't owned, then make current weapon highlighted
        UIWeaponsBar.ShowPistolTransparent();

        if (HasShotgun)
            UIWeaponsBar.ShowShotgunTransparent();
        else
            UIWeaponsBar.HideShotgun();

        if (HasSMG)
            UIWeaponsBar.ShowSMGTransparent();
        else
            UIWeaponsBar.HideSMG();

        if (i == 0)
            UIWeaponsBar.ShowPistol();
        else if (i == 1)
            UIWeaponsBar.ShowShotgun();
        else if (i == 2)
            UIWeaponsBar.ShowSMG();


    }

    public void SubtractAmmo(int i)
    {
        CurrentAmmo[i] -= 1;
    }

    public void AddAmmo(int i, int ammoGained)
    {
        CurrentAmmo[i] += ammoGained;

        if (i == 0 && CurrentAmmo[0] > MaxPistolAmmo)
            CurrentAmmo[0] = MaxPistolAmmo;
        else if (i == 1 && CurrentAmmo[1] > MaxShotgunAmmo)
            CurrentAmmo[1] = MaxShotgunAmmo;
        else if (i == 2 && CurrentAmmo[2] > MaxSMGAmmo)
            CurrentAmmo[2] = MaxSMGAmmo;
    }

    public void UpdateWeaponsUI()
    {

        bool levelCheck = true;
        if (GameManagerInstance.CurrentLevel == 2)
            levelCheck = false;

        UIWeaponsBar.UpdateWeaponAmmo(CurrentAmmo[0], CurrentAmmo[1], CurrentAmmo[2]);
        UIWeaponsBar.UpdateObjective(EnemiesKilled, levelCheck);

        if (EnemiesKilled >= 25 && levelCheck)
        {
            UIGameOverPanel.gameObject.SetActive(true);
            UIGameOverPanel.DisplayWinningText();
            Time.timeScale = 0;
        }

    }
}
