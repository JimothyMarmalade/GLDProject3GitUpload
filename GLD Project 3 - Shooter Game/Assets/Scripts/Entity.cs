using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float MaxHealth;
    public float MaxStamina = 100;
    public float CurrentStamina;
    public float CurrentHealth;
    public float StaminaBreakoff = 30;
    public bool CanBeDestroyed;
    public bool CanRun;

    public AudioSource PainSFX;
    public AudioClip[] PainSFXClips;

    public AudioSource DeathSFX;
    public AudioClip[] DeathSFXClips;

    public AudioSource IdleSFX;
    public AudioClip[] IdleSFXClips;
    public bool PlayIdleSFXClips;


    public bool HasIFrames = false;
    public float IFramesCooldown = 1;
    public bool IFramesActive = false;


    public virtual void TakeDamage(float dmg)
    {
        if (CanBeDestroyed && !IFramesActive)
        {
            CurrentHealth -= dmg;
            

            //Debug.Log("CurrentHealth: " + CurrentHealth);

            if (CurrentHealth <= 0)
            {
                PlayDeathSFX();
                Die();
            }

            PlayPainSFX();

            if (HasIFrames)
            {
                //Debug.Log("Calling ActivateIFrames()...");
                IEnumerator FD = ActivateIFrames(this.gameObject);
                StartCoroutine(FD);
            }

        }
    }

    public virtual void RecoverDamage(float dmg)
    {
        CurrentHealth += dmg;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public virtual void PlayIdleSFX()
    {
        if (IdleSFX != null && !IdleSFX.isPlaying && PlayIdleSFXClips)
        {
            int r = Random.Range(0, 3);
            if (r == 2)
            {
                int i = Random.Range(0, IdleSFXClips.Length - 1);
                if (IdleSFXClips[i] != null)
                {
                    IdleSFX.clip = IdleSFXClips[i];
                    IdleSFX.Play();
                }
            }
            Debug.Log("PlayIdleSFX() r = " + r);
        }
        else if (IdleSFX == null)
            Debug.Log("IdleSFX is null for " + gameObject.name + "!");
    }

    public virtual void PlayPainSFX()
    {
        if (PainSFX != null && !PainSFX.isPlaying)
        {
            int r = Random.Range(0, 3);
            if (r == 2)
            {
                int i = Random.Range(0, PainSFXClips.Length - 1);
                if (PainSFXClips[i] != null)
                {
                    PainSFX.clip = PainSFXClips[i];
                    PainSFX.Play();
                }
            }
            Debug.Log("PlayPainSFX() r = " + r);
        }
        else if (PainSFX == null)
            Debug.Log("PainSFX is null for " + gameObject.name + "!");
    }

    public virtual void PlayDeathSFX()
    {
        if (IdleSFX != null)
            IdleSFX.Stop();

        if (PainSFX != null)
            PainSFX.Stop();

        if (DeathSFX != null && !DeathSFX.isPlaying)
        {
            int i = Random.Range(0, DeathSFXClips.Length);
            if (DeathSFXClips[i] != null)
            {
                DeathSFX.clip = DeathSFXClips[i];
                DeathSFX.Play();
            }
        }
        else
            Debug.Log("DeathSFX is null for " + gameObject.name + "!");
    }



    public virtual void Die()
    {
        Debug.Log("Destroying " + gameObject.name);
        Destroy(gameObject, 4f);
    }

    public IEnumerator FlashDamage(GameObject obj, Color c)
    {
        //Debug.Log("FlashDamage()");

        Material objMat = obj.GetComponent<MeshRenderer>().material;
        Color originalColor = objMat.color;

        objMat.color = c;

        yield return new WaitForSeconds(0.1f);

        objMat.color = originalColor;
    }

    //Variant of FlashDamage used for Terrain Objects
    public IEnumerator TerrainFlashDamage(GameObject obj, Material mat)
    {
        Debug.Log("TerrainFlashDamage()");

        MeshRenderer OMR = obj.GetComponent<MeshRenderer>();

        Material OriginalMat = OMR.material;

        OMR.material = mat;

        yield return new WaitForSeconds(0.1f);

        obj.GetComponent<MeshRenderer>().material = OriginalMat;
    }

    public IEnumerator ActivateIFrames(GameObject obj)
    {
        //Debug.Log("ActivateIFrames()");
        IFramesActive = true;
        float cooldown = Time.time + IFramesCooldown;

        Material objMat = obj.GetComponent<MeshRenderer>().material;
        Color originalColor = objMat.color;
        
        while (Time.time < cooldown)
        {
            objMat.color = Color.white;
            yield return new WaitForFixedUpdate();
            objMat.color = originalColor;
            yield return new WaitForFixedUpdate();
        }

        IFramesActive = false;
        objMat.color = originalColor;
    }



}
