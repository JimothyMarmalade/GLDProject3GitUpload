using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private Player player;
    public float BaseEnemyHealth = 5f;
    public float RandomHealthModifier = 2f;
    public float Damage = 1f;

    public GameObject[] BodyParts;
    private BoxCollider SelfCollider;
    private Rigidbody SelfRigidbody;

    public GameObject[] ItemDrops;

    public Transform ExplosionCenter;


    private void Start()
    {
        SelfCollider = GetComponent<BoxCollider>();
        SelfRigidbody = GetComponent<Rigidbody>();

        int chanceForInstakill = Random.Range(0, 20);
        if (chanceForInstakill == 20)
            MaxHealth = 1;
        else
            MaxHealth = BaseEnemyHealth += Mathf.Round(Random.Range(-RandomHealthModifier, RandomHealthModifier));

        CurrentHealth = MaxHealth;
        int chanceDoubleDamage = Random.Range(0, 3);
        if (chanceDoubleDamage == 3)
            Damage = 2;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        PlayIdleSFXClips = true;
        InvokeRepeating("PlayIdleSFX", 3f, 3f);


    }

    public override void TakeDamage(float dmg)
    {
        //Debug.Log("TakeDamage()");
        if (CanBeDestroyed && !IFramesActive)
        {
            PlayPainSFX();
            foreach (GameObject b in BodyParts)
            {
                //Debug.Log("Calling FlashDamage()...");
                IEnumerator FD = FlashDamage(b, Color.white);
                StartCoroutine(FD);
            }
        }

        base.TakeDamage(dmg);
    }

    public override void Die()
    {
        if (player != null)
            player.EnemiesKilled++;

        PlayIdleSFXClips = false;
        PlayDeathSFX();

        SelfCollider.enabled = false;
        GetComponent<Mover>().enabled = false;
        GetComponent<ObjFace>().enabled = false;
        //Destroy(SelfRigidbody);

        foreach (GameObject b in BodyParts)
        {
            b.AddComponent<Rigidbody>();
            b.GetComponent<BodyPart>().BeginFadeout();
        }

        SpawnItem();

        //GetComponent<Rigidbody>().Sleep();
        InvokeRepeating("CheckDestroy", 3f, 2f);
        Destroy(gameObject, 45);
    }

    private void CheckDestroy()
    {
        bool terminate = true;

        foreach (GameObject b in BodyParts)
            if (b != null)
                terminate = false;

        if (terminate)
            Destroy(gameObject);
    }

    private void SpawnItem()
    {
        //Default to an ammo drop if player's ammo is low
        if (player.GetComponent<Player>().CurrentAmmo[0] < 10)
        {
            GameObject itemDrop = Instantiate(ItemDrops[0]) as GameObject;
            itemDrop.transform.position = gameObject.transform.position;
        }
        //  1/5 chance to drop mob's "special" drop
        else if (Random.Range(0, 5) == 0)
        {
            GameObject itemDrop = Instantiate(ItemDrops[2]) as GameObject;
            itemDrop.transform.position = gameObject.transform.position;
        }
        //Else, flip a coin and drop a default drop
        else
        {
            GameObject itemDrop;
            if (Random.Range(0, 2) == 0)
                itemDrop = Instantiate(ItemDrops[0]) as GameObject;
            else
                itemDrop = Instantiate(ItemDrops[1]) as GameObject;

            itemDrop.transform.position = gameObject.transform.position;
        }
    }


}
