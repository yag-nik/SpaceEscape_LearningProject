using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TurretState
{
    IDLE,
    SCANNING,
    SHOOTING,
    LAUNCHING
}

public class Turret : MonoBehaviour
{
    AudioSource turretAudio; //sfx
    public TurretState turretState = TurretState.IDLE;
    CooldownAction scanAction;
    CooldownAction shootAction;
    CooldownAction launchAction;
    bool isDamaged;
    public int hitPoints = 9;
    public ParticleSystem smokeVFX;
    public ParticleSystem explosionVFX;
    public AudioClip explosionSFX;


    private void Start()
    {
        isDamaged = false;

        turretAudio = GetComponent<AudioSource>();
        scanAction = GetComponent<CooldownAction>();
        scanAction.OnEnded.AddListener(ScanEnded);
        shootAction = GetComponent<ShootAction>();
        shootAction.OnEnded.AddListener(ShootEnded);
        launchAction = GetComponent<LaunchAction>();

        launchAction.OnEnded.AddListener(LaunchEnded);
        StartScan();
    }

    public void Damage(int amount)
    {
        if (isDamaged == false)
        {
            smokeVFX.gameObject.SetActive(true);
        }
        isDamaged = true;
        hitPoints -= amount; // sub from current hp

        if(hitPoints <= 0)
        {
            StartCoroutine(Die());
        }

    }
    IEnumerator Die()
    {
        turretAudio.clip = explosionSFX;
        turretAudio.Play();
        yield return null;
        explosionVFX.gameObject.SetActive(true);
        Destroy(gameObject, 1.2f);
    }

    void LaunchEnded()
    {
        StartScan();
    }

    void ScanEnded()
    {
        StartShoot();
    }
    void ShootEnded()
    {
        StartLaunch();
    }

    void StartScan()
    {
        turretState = TurretState.SCANNING;
        scanAction.Trigger();
    }

    void StartShoot()
    {
        turretState = TurretState.SHOOTING;
        shootAction.Trigger();
    }

    void StartLaunch()
    {
        turretState = TurretState.LAUNCHING;
        launchAction.Trigger();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartScan();
        }
    }
}
