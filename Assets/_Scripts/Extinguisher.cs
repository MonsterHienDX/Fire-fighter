using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private float amountExtinguishedPerSecond;
    [SerializeField] private Transform waterTapTransform;
    [SerializeField] private ParticleSystem waterParticleSystem;
    private ParticleSystemRenderer waterParticleRenderer;
    [SerializeField] private bool useTurnOnRenderer;

    [SerializeField] private float waterCapacity;
    private float currentWaterAmount;
    [SerializeField] private Image waterAmountUI;

    private float startWaterIntensity;

    private void Start()
    {
        Application.targetFrameRate = 60;
        currentWaterAmount = waterCapacity;
        startWaterIntensity = waterParticleSystem.emission.rateOverTime.constant;
        waterParticleRenderer = waterParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
        waterParticleRenderer.enabled = false;
        waterParticleSystem.Pause();
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
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
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopWater();
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

        SetWaterDirection();

        if (
            Physics.Raycast(waterTapTransform.position, waterTapTransform.forward, out RaycastHit hit, 100f)
            && hit.collider.TryGetComponent(out Fire fire)
           )
        {

            if (fire.isLit)
                fire.TryExtinguish(amountExtinguishedPerSecond * Time.deltaTime, hit.point);
        }
        Debug.DrawRay(waterTapTransform.position, waterTapTransform.forward * 1000, Color.red);
    }

    private void StopWater()
    {
        if (useTurnOnRenderer)
        {
            waterParticleRenderer.enabled = false;
        }
        else
        {
            var emission = waterParticleSystem.emission;
            emission.rateOverTime = 0.0f * startWaterIntensity;
            var burst = emission.GetBurst(0);
            burst.count = 0;
            // burst.cycleCount = 1;
            emission.SetBurst(0, burst);
        }
    }

    private void SetWaterDirection()
    {
        Vector3 lookAtPos =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        waterTapTransform.LookAt(lookAtPos);
        // ResetGunDirection();

    }

    private void ResetGunDirection()
    {
        var newRotate = new Quaternion(0, waterTapTransform.localRotation.y, waterTapTransform.localRotation.z,
            waterTapTransform.localRotation.w);

        waterTapTransform.localRotation = newRotate;
    }
}
