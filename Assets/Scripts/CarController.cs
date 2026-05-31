using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarData carData;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (velocityVsUp > carData.maxSpeed && accelerationInput > 0)
        {
            return;
        }

        if (velocityVsUp < -carData.maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        if (carRigidbody.linearVelocity.sqrMagnitude > carData.maxSpeed * carData.maxSpeed && accelerationInput > 0)
        {
            return;
        }

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
        float minSpeedBeforeTurningFactor = (carRigidbody.linearVelocity.magnitude / 8);
        minSpeedBeforeTurningFactor = Mathf.Clamp01(minSpeedBeforeTurningFactor);
        rotationAngle -= turningInput * carData.turningFactor * minSpeedBeforeTurningFactor;
        carRigidbody.MoveRotation(rotationAngle);
    }
    
    public void setInputVector(Vector2 inputVector, float brake)
    {
        turningInput = inputVector.x;
        accelerationInput = inputVector.y;
        brakeInput = brake;
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.linearVelocity, transform.right);

        carRigidbody.linearVelocity = forwardVelocity + rightVelocity * carData.driftFactor;
    }
}
