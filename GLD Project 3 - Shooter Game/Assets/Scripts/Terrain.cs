using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : Entity
{
    public Material DamageMaterial;


    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public override void TakeDamage(float dmg)
    {
        if (CanBeDestroyed)
        {
            Debug.Log("Terrain.TakeDamage()");
            IEnumerator FD = TerrainFlashDamage(this.gameObject, DamageMaterial);
            StartCoroutine(FD);
            base.TakeDamage(dmg);
        }
    }

    public override void Die()
    {
        Destroy(gameObject, 0.5f);
    }
}