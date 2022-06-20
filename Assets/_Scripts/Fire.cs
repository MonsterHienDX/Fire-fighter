using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float currentIntensity = 1.0f;
    [SerializeField] private ParticleSystem[] fireParticleSystems = new ParticleSystem[0];
    private float[] startIntensities = new float[0];
    [SerializeField] private ParticleSystem steamParticleSystem;
    private float startIntensitiesSteam;


    private float timeLastWatered = 0f;
    [SerializeField] private float regenerateDelay;
    [SerializeField] private float regenerateRate;
    private bool _isLit = true;
    public bool isLit { get => _isLit; private set => _isLit = value; }

    private void Start()
    {
        startIntensities = new float[fireParticleSystems.Length];
        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            startIntensities[i] = fireParticleSystems[i].emission.rateOverTime.constant;
        }

        startIntensitiesSteam = steamParticleSystem.emission.rateOverTime.constant;

        var steamEmission = steamParticleSystem.emission;
        steamEmission.enabled = false;
    }


    private void Update()
    {
        if (isLit && currentIntensity < 1.0f && Time.time - timeLastWatered >= regenerateDelay)
        {
            currentIntensity += regenerateRate * Time.deltaTime;
            HideSteam();
            ChangeIntensity();
        }
    }

    public bool TryExtinguish(float amount, Vector3 collidePoint)
    {
        timeLastWatered = Time.time;

        currentIntensity -= amount;
        ChangeIntensity();
        ShowSteam(collidePoint);
        if (currentIntensity <= 0)
        {
            isLit = false;
            HideSteam();
            return true;   // Fire out
        }
        return false;

    }
    private void ChangeIntensity()
    {
        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            var emission = fireParticleSystems[i].emission;
            emission.rateOverTime = currentIntensity * startIntensities[i];
        }
    }

    private void ShowSteam(Vector3 steamPosition)
    {
        var emission = steamParticleSystem.emission;
        emission.enabled = true;
        emission.rateOverTime = 1.0f * startIntensitiesSteam;
        steamParticleSystem.transform.position = steamPosition;
        steamParticleSystem.transform.localPosition = new Vector3(steamParticleSystem.transform.localPosition.x, 0, steamParticleSystem.transform.localPosition.z);
    }

    private void HideSteam()
    {
        var emission = steamParticleSystem.emission;
        emission.rateOverTime = 0.0f * startIntensitiesSteam;

        // var burst = emission.GetBurst(0);
        // burst.cycleCount = 1;
    }

}
