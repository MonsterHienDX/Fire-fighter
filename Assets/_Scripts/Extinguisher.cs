using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private float amountExtinguishedPerSecond;
    [SerializeField] private Transform waterTapTransform;
    [SerializeField] private ParticleSystem waterParticleSystem;

    private float startWaterIntensity;

    private void Start()
    {
        startWaterIntensity = waterParticleSystem.emission.rateOverTime.constant;
    }


    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            FireWater();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopWater();
        }
    }

    private void FireWater()
    {
        SetWaterDirection();

        var emission = waterParticleSystem.emission;
        if (!emission.enabled) emission.enabled = true;
        emission.rateOverTime = 1.0f * startWaterIntensity;

        if (
            Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, 100f)
            && hit.collider.TryGetComponent(out Fire fire)
           )
        {
            Debug.DrawRay(waterTapTransform.position, this.transform.forward, Color.red);

            if (fire.isLit)
                fire.TryExtinguish(amountExtinguishedPerSecond * Time.deltaTime, hit.point);
        }
    }

    private void StopWater()
    {
        var emission = waterParticleSystem.emission;
        emission.rateOverTime = 0.0f * startWaterIntensity;
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
