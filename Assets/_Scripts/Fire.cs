using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float currentIntensity = 1.0f;
    [SerializeField] private ParticleSystem fireParticleSystem;
    private float startIntensity;
    private Vector3 startFireScale;



    [SerializeField] private ParticleSystem steamParticleSystem;
    private float startIntensitiesSteam;
    private ParticleSystemRenderer steamParticleRenderer;

    [SerializeField] private ParticleSystem smokeParticleSystem;

    private float timeLastWatered = 0f;
    [SerializeField] private float regenerateDelay;
    [SerializeField] private float regenerateRate;
    private bool _isLit = true;
    public bool isLit { get => _isLit; private set => _isLit = value; }

    private void Start()
    {
        startIntensity = fireParticleSystem.emission.rateOverTime.constant;
        startFireScale = fireParticleSystem.gameObject.transform.localScale;
        startIntensitiesSteam = steamParticleSystem.emission.rateOverTime.constant;

        var steamEmission = steamParticleSystem.emission;
        steamEmission.enabled = false;
        steamParticleRenderer = steamParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
        steamParticleRenderer.enabled = false;

        smokeParticleSystem.Pause();
    }


    private void Update()
    {
        if (isLit && currentIntensity < 1.0f && Time.time - timeLastWatered >= regenerateDelay)
        {
            currentIntensity += regenerateRate * Time.deltaTime;
            HideSteam();
            this.GetComponent<Collider>().enabled = true;
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
            FireOut();
            return true;
        }
        return false;

    }
    private void ChangeIntensity()
    {
        var emission = fireParticleSystem.emission;
        emission.rateOverTime = currentIntensity * startIntensity;

        Vector3 scale = currentIntensity * startFireScale;
        fireParticleSystem.gameObject.transform.localScale = scale;
    }

    private void FireOut()
    {
        isLit = false;
        HideSteam();
        this.GetComponent<Collider>().enabled = false;
        smokeParticleSystem.Play();
    }

    private void ShowSteam(Vector3 steamPosition)
    {
        if (!steamParticleRenderer.enabled) steamParticleRenderer.enabled = true;
        var emission = steamParticleSystem.emission;
        emission.enabled = true;
        emission.rateOverTime = 1.0f * startIntensitiesSteam;
        steamParticleSystem.transform.position = steamPosition;
        // steamParticleSystem.transform.localPosition = new Vector3(steamParticleSystem.transform.localPosition.x, 0, steamParticleSystem.transform.localPosition.z);
    }

    private void HideSteam()
    {
        var emission = steamParticleSystem.emission;
        emission.rateOverTime = 0.0f * startIntensitiesSteam;

        // var burst = emission.GetBurst(0);
        // burst.cycleCount = 1;
    }

}
