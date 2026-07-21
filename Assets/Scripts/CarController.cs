using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarData carData;
    [SerializeField] private Gun gun;
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private int RamDamage; //Damage dealt to enemies when player rams into them

    float accelerationInput;
    float turningInput;
    float brakeInput;
    float rotationAngle;
    float velocityVsUp;

    Rigidbody2D carRigidbody;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody.linearVelocity);

        if (velocityVsUp > carData.maxSpeed && accelerationInput > 0) return;
        if (velocityVsUp < -carData.maxSpeed * 0.5f && accelerationInput < 0) return;
        if (carRigidbody.linearVelocity.sqrMagnitude > carData.maxSpeed * carData.maxSpeed && accelerationInput > 0) return;

        if (accelerationInput == 0)
        {
            if (brakeInput > 0 && carRigidbody.linearVelocity.sqrMagnitude > 0.001f)
            {
                Vector2 brakeForce = -carRigidbody.linearVelocity.normalized * carData.brakeFactor * brakeInput;
                carRigidbody.AddForce(brakeForce, ForceMode2D.Force);
                carRigidbody.linearDamping = Mathf.Lerp(carRigidbody.linearDamping, carData.dragFactor, Time.fixedDeltaTime * 5);
            }
            else
            {
                carRigidbody.linearDamping = Mathf.Lerp(carRigidbody.linearDamping, carData.dragFactor, Time.fixedDeltaTime * 3);
            }
        }
        else
        {
            carRigidbody.linearDamping = 0;
        }

        Vector2 force = transform.up * accelerationInput * carData.accelerationFactor;
        carRigidbody.AddForce(force, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float minSpeedBeforeTurningFactor = Mathf.Clamp01(carRigidbody.linearVelocity.magnitude / 8f);
        rotationAngle -= turningInput * carData.turningFactor * minSpeedBeforeTurningFactor;
        carRigidbody.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.linearVelocity, transform.right);
        carRigidbody.linearVelocity = forwardVelocity + rightVelocity * carData.driftFactor;
    }

    public void setInputVector(Vector2 inputVector, float brake)
    {
        turningInput = inputVector.x;
        accelerationInput = inputVector.y;
        brakeInput = brake;
    }

    public void freezeInPlace()
    {
        carRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void unfreeze()
    {
        carRigidbody.constraints = RigidbodyConstraints2D.None;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody.linearVelocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;

        return false;
    }

    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        if (playerHealth <= 0)
        {
            // Handle player death here
            Debug.Log("Player has died. Health: " + playerHealth);
            // You can add additional logic for game over, respawn, etc.
        }
    }
}