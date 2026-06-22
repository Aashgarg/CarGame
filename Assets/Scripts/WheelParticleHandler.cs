using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WheelParticleHandler : MonoBehaviour
{
    float particleEmissionRate = 0;
    CarController carController;
    ParticleSystem smokeParticle;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
        smokeParticle = GetComponent<ParticleSystem>();
        //Get the emission component
        particleSystemEmissionModule = smokeParticle.emission;
        particleSystemEmissionModule.rateOverTime = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (carController.IsTireScreeching(out float lateralvelocity, out bool isBraking))
        {
            if (isBraking)
            {
                particleEmissionRate = 30;
            }
            else
            {
                particleEmissionRate = Mathf.Abs(lateralvelocity) * 2;
            }
        }
    }
}
