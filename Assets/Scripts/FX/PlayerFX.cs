using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Screen Shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeDirection_light;
    public Vector3 shakeDirection_medium;
    public Vector3 shakeDirection_heavy;
    private CinemachineImpulseSource screenShake;

    [Header("Afterimage FX")]
    [SerializeField] private GameObject afterimagePrefab;
    [SerializeField] private float afterimageColorLosingSpeed;
    [SerializeField] private float afterimageCooldown;
    private float afterimageCooldownTimer;

    [Space]
    [SerializeField] private ParticleSystem dustFX;

    protected override void Awake()
    {
        base.Awake();

        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    protected override void Start()
    {
        base.Start();

        afterimageCooldownTimer = 0;
    }

    protected override void Update()
    {
        base.Update();

        afterimageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakeDirection)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakeDirection.x * player.facingDirection, _shakeDirection.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterimage()
    {
        if (afterimageCooldownTimer < 0)
        {
            //need to pass transform.rotation here to make afterimage flip together with player when dashing to the left
            GameObject newAfterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            newAfterimage.GetComponent<AfterimageFX>()?.SetupAfterImage(sr.sprite, afterimageColorLosingSpeed);

            afterimageCooldownTimer = afterimageCooldown;
        }
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
        {
            dustFX.Play();
        }
    }
}