using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    // gun properties
    public float range = 100f;
    public float damage = 10f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    // ammo stuff
    public int MaxAmmo = 100;
    public int MaxClip = 25;
    public int Ammo;
    public int Clip;
    public float reloadTime = 1f;
    bool isReloading;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject impactZombie;
    public Camera fpsCam;


    private float nextTimeToFire = 0f;
    // Use this for initialization
    void Start()
    {
        FillAmmo();

    }

    // Update is called once per frame
    void Update()
    {

        if (isReloading)
        {
            return;
        }
        if (Clip <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // reload button
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        Clip--;
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // Debug.Log(hit.transform.name);
            // check if it is zombie or world
            if (hit.transform.tag == "Zombie")
            {
                hit.transform.GetComponent<Zombie>().TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {

                hit.rigidbody.AddForce(-hit.normal * impactForce);
                GameObject impactGo = Instantiate(impactZombie, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
            else
            {

                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Relaoding...");
        yield return new WaitForSeconds(reloadTime);

        if (Clip == MaxClip)
        {
            // ur amo is full
            Debug.Log("ur amo is full");
        }
        else
        {
            // update ammo
            if (Ammo < MaxClip && Ammo > 0)
            {
                Clip += Ammo;
                Ammo = 0;

            }
            else if (Ammo >= MaxClip)
            {
                int leftAmo = MaxClip - Clip;
                Ammo -= leftAmo;
                Clip += leftAmo;
            }
            else if (Ammo == 0)
            {
                // no ammo left
                Debug.Log("ran out of ammo");
            }
        }
        isReloading = false;
    }

    public void FillAmmo()
    {
        Ammo = MaxAmmo;
        Clip = MaxClip;
    }
}
