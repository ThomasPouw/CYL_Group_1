using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 35f;

    int modifiedDamage
    {
        get { return (int)(damage * StatModifierHandler.instance.GetModifier(ModType.damage)); }
        set { }
    }

    int modifiedMaxAmmo
    {
        get { return (int)(maxAmmo * StatModifierHandler.instance.GetModifier(ModType.ammoClipSize)); }
        set { }
    }

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    Animator animator;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;
    AudioPlayer audioPlayer;
    [SerializeField] int maxAmmo = 30;
    int currentAmmo;
    [SerializeField] float reloadTimeSeconds = 2f;
    bool reloading;

    bool canShoot = true;

    private void OnEnable()
    {
        StartCoroutine(Switch());
    }

    private void OnDisable()
    {
        animator.SetBool("Aim", false);
    }

    void Start()
    {
        audioPlayer = GetComponent<AudioPlayer>();
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError($"Weapon '{name}' does not have an animator attached.");
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        DisplayAmmo();

        if (reloading) return;

        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetMouseButton(1))
        {
            AimDownSights();
        }
        else
        {
            CancelAim();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < modifiedMaxAmmo)
        {
            Reload();
        }
    }

    private void DisplayAmmo()
    {
        ammoText.text = $"{currentAmmo} / {modifiedMaxAmmo}";
    }

    IEnumerator Switch()
    {
        yield return new WaitForSeconds(0.05f);
        canShoot = false;
        audioPlayer.PlayByName("Switch");
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (currentAmmo >= 1)
        {
            DecreaseAmmo();
            PlayMuzzleFlash();
            ProcessRaycast();
            audioPlayer.PlayByName("Shoot");
        }

        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            target.ReduceHealth(modifiedDamage);
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(transform.position));
        Destroy(impact, 0.1f);
    }

    private void AimDownSights()
    {
        animator.SetBool("Aim", true);
        FPCamera.GetComponent<Animator>().SetBool("CameraZoom", true);
    }

    //called by [reloading weapon, releasing aim button, sprinting]
    public void CancelAim()
    {
        animator.SetBool("Aim", false);
        FPCamera.GetComponent<Animator>().SetBool("CameraZoom", false);
    }

    public void Reload()
    {
        //TODO: display reloading animation/notification

        reloading = true;
        Invoke("InstantReload", reloadTimeSeconds);
    }

    private void InstantReload()
    {
        //TODO: stop reloading animation/notification

        currentAmmo = modifiedMaxAmmo;
        reloading = false;
    }

    public void DecreaseAmmo()
    {
        currentAmmo--;
    }
}
