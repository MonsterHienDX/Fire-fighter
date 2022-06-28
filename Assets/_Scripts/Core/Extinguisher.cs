using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extinguisher : SingletonMonobehaviour<Extinguisher>
{
    [SerializeField] private Transform waterTapTransform;
    [SerializeField] private ParticleSystem waterParticleSystem;
    private ParticleSystemRenderer waterParticleRenderer;
    [SerializeField] private bool useTurnOnRenderer;

    [SerializeField] private float waterCapacity;

    private float currentWaterAmount;
    [SerializeField] private Image waterAmountUI;
    private float startWaterIntensity;
    private AudioSource audioSource;
    private AudioClip wateringSound;
    private AudioClip endWaterSound;

    // [SerializeField] private FireWaterButton fireWaterButton;
    [SerializeField] private float controlSensitivity;
    [SerializeField] private RectTransform targetRectTransform;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        wateringSound = AudioManager.instance.GetSoundByName(SoundName.Watering);
        endWaterSound = AudioManager.instance.GetSoundByName(SoundName.SteamEndWater);

        currentWaterAmount = waterCapacity;
        startWaterIntensity = waterParticleSystem.emission.rateOverTime.constant;
        waterParticleRenderer = waterParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
        waterParticleRenderer.enabled = false;
        waterParticleSystem.Pause();
    }

    void Update()
    {
        if (!GameManager.instance.isPlaying)
        {
            StopWater();
            return;

        }

        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
            SetWaterTapDirection();
            HandleFireWater();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopWater();
        }
    }

    private void HandleFireWater()
    {
        if (currentWaterAmount > 0)
        {
            FireWater();
            currentWaterAmount -= Time.deltaTime;
            waterAmountUI.fillAmount = currentWaterAmount / waterCapacity;
        }
        else
        {
            StopWater();
            EventDispatcher.Instance.PostEvent(EventID.OutOfWater);
        }
    }

    private void FireWater()
    {
        if (!waterParticleRenderer.enabled) waterParticleRenderer.enabled = true;
        if (waterParticleSystem.isPaused) waterParticleSystem.Play();

        var emission = waterParticleSystem.emission;
        emission.rateOverTime = 1.0f * startWaterIntensity;
        var burst = emission.GetBurst(0);
        burst.count = 10;
        // burst.cycleCount = 0;
        emission.SetBurst(0, burst);

        if (
            Physics.Raycast(waterTapTransform.position, waterTapTransform.forward, out RaycastHit hit, 100f)
            && hit.collider.TryGetComponent(out Fire fire)
           )
        {

            if (fire.isLit)
                fire.TryExtinguish(hit.point);
        }
        PlayWateringSound();

        Debug.DrawRay(waterTapTransform.position, waterTapTransform.forward * 1000, Color.red);
    }

    private void StopWater()
    {
        var emission = waterParticleSystem.emission;
        emission.rateOverTime = 0.0f * startWaterIntensity;
        var burst = emission.GetBurst(0);
        burst.count = 0;
        // burst.cycleCount = 1;
        emission.SetBurst(0, burst);
        // PlayEndWaterSound();
        PauseWateringSound();
    }

    private void SetTargetPosition()
    {
        Vector2 joyVector = JoyStick.instance.GetJoyVector();
        targetRectTransform.anchoredPosition = new Vector2(
            targetRectTransform.anchoredPosition.x + joyVector.x * controlSensitivity / 21,
            targetRectTransform.anchoredPosition.y + joyVector.y * controlSensitivity / 21
        );

        Debug.LogWarning($"new position target sign: {targetRectTransform.anchoredPosition}");

    }

    private void SetWaterTapDirection()
    {
        // Vector3 lookAtPos =
        //     Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));

        // waterTapTransform.LookAt(Vector3.Lerp(lookAtPos, this.transform.forward, .1f));

        Vector2 joyVector = JoyStick.instance.GetJoyVector();
        Vector3 _newForward = new Vector3(
            waterTapTransform.forward.x + joyVector.x / controlSensitivity,
            waterTapTransform.forward.y + joyVector.y / controlSensitivity,
            waterTapTransform.forward.z
        );

        waterTapTransform.forward = _newForward;
        Debug.LogWarning($"waterTapTransform.rotation.x: {waterTapTransform.localRotation.x}");
        // ResetGunDirection();
    }

    private void ResetGunDirection()
    {
        var newRotate = new Quaternion(0, waterTapTransform.localRotation.y, waterTapTransform.localRotation.z,
            waterTapTransform.localRotation.w);

        waterTapTransform.localRotation = newRotate;
    }

    public void ReFillWater()
    {
        currentWaterAmount = waterCapacity;
        waterAmountUI.fillAmount = 1f;
    }

    private void PlayWateringSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = wateringSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PauseWateringSound()
    {
        // Debug.LogWarning("PauseWateringSound");
        audioSource.Pause();
    }

    private void PlayEndWaterSound()
    {
        CommonFunctions.PlayOneShotASound(audioSource, endWaterSound);
    }
}
