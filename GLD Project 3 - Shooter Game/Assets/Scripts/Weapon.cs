using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform SpawnPoint;
    public LayerMask CollisionMask;

    public float GunID;

    public enum FireType {FullAuto, SemiAuto}
    public FireType FiringMode;

    public float FireSpeed;
    private float FiringCooldown;
    public float WeaponDamage = 1;

    public AudioSource WeaponSFX;
    public AudioSource OutOfAmmo;

    public LineRenderer BulletTrail;

    public void Start()
    {
        if (GetComponent<LineRenderer>())
        {
            BulletTrail = GetComponent<LineRenderer>();
            BulletTrail.enabled = false;

        }
    }


    public int Shoot(int currentammo)
    {
        if (CanShoot())
        {
            Debug.Log("Shooting");
            Ray ray = new Ray(SpawnPoint.position, SpawnPoint.forward);
            RaycastHit hit;

            float shotDistance = 30;

            if (Physics.Raycast(ray, out hit, shotDistance, CollisionMask))
            {
                shotDistance = hit.distance;

                if (hit.collider.GetComponent<Entity>())
                {
                    hit.collider.GetComponent<Entity>().TakeDamage(WeaponDamage);
                }


            }
            //Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1);

            FiringCooldown = Time.time + FireSpeed;

            WeaponSFX.Play();

            if (BulletTrail)
            {
                StartCoroutine("RenderBulletTrails", ray.direction * shotDistance);
            }
            return currentammo -= 1;
        }
        else
        {
            if (currentammo <= 0)
                OutOfAmmo.Play();
            return currentammo;
        }

    }

    public bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < FiringCooldown)
            canShoot = false;

        return canShoot;
    }

    public int ShootContinuous(int i)
    {
        if (FiringMode == FireType.FullAuto)
            return Shoot(i);
        else
            return i;
    }

    IEnumerator RenderBulletTrails(Vector3 endPoint)
    {
        BulletTrail.enabled = true;
        BulletTrail.SetPosition(0, SpawnPoint.position);
        BulletTrail.SetPosition(1, SpawnPoint.position + endPoint);

        yield return new WaitForFixedUpdate();

        BulletTrail.enabled = false;

    }

}
