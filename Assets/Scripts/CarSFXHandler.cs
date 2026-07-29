using UnityEngine;

public class CarSFXHandler : MonoBehaviour
{
    [Header ("Audio sources")]
    [SerializeField] AudioSource tiresScreeachingAudioSource;
    [SerializeField] AudioSource engineAudioSource;
    [SerializeField] AudioSource carHitAudioSource;

    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    CarController carController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carController = GetComponentInParent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTireScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        float velocityMagnitude = carController.GetVelocityMagnitude();

        //Increase the engine volume as the car goes faster
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        //But keep a minimum level so it playes even if the car is idle
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);
        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        //To add more variation to the engine sound we also change the pitch
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf. Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTireScreechingSFX()
    {
        if (carController.IsTireScreeching(out float lateralvelocity, out bool isBraking))
        {
            //If the car is braking we want the tire screech to be louder and also change the pitch.
            if (isBraking)
            {
                tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                //If we are not braking we still want to play this screech sound if the player is drifting.
                tiresScreeachingAudioSource.volume = Mathf.Abs (lateralvelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralvelocity) * 0.1f;
            }
        }
        //Fade out the tire screech SFX if we are not screeching-
        else tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 0, Time.deltaTime * 10);
    }

    public void playHitSound(float relativeVelocity, float volume)
    {
        if (carHitAudioSource == null)
        {
            Debug.LogWarning("Car hit AudioSource is not assigned on CarSFXHandler.");
            return;
        }

        if (carHitAudioSource.clip == null)
        {
            Debug.LogWarning("Car hit AudioClip is not assigned on CarSFXHandler.");
            return;
        }

        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = Mathf.Clamp01(volume);
        carHitAudioSource.Play();
    }
}

