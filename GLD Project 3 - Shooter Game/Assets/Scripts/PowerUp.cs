using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int PowerupID;
    public int WeaponAmmoType;
    public GameObject PowerupGFX;
    public Light PowerupLighting;

    private void Start()
    {
        if (PowerupID != 2)
            PowerupLighting.color = PowerupGFX.GetComponent<MeshRenderer>().material.color;
        else
            PowerupLighting.color = Color.white;

        Destroy(gameObject, 7.5f);
    }


}
