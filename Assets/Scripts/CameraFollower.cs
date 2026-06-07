using UnityEngine;

//Top down view camera that follow the car and also rotates when car is turning
public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zOffset;
    [SerializeField] Rigidbody2D carRigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        if (carTransform != null)
        {
            carRigidbody = carTransform.GetComponent<Rigidbody2D>();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        FollowCar();
        RotateCamera();
    }

    void FollowCar()
    {
        Vector3 targetPosition = new Vector3(carTransform.position.x, carTransform.position.y, zOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    void RotateCamera()
    {
        if (carRigidbody != null)
        {
            // Apply the car's angular velocity directly to the camera
            // This makes the camera rotate at the exact speed the car is rotating
            float angularVelocity = carRigidbody.angularVelocity;
            Quaternion deltaRotation = Quaternion.Euler(0, 0, angularVelocity * Time.deltaTime);
            transform.rotation = transform.rotation * deltaRotation;
            
            
            /*
            Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Mathf.Rad2Deg * Time.deltaTime);
            transform.rotation = transform.rotation * deltaRotation; */
        }
        else
        {
            // Fallback: Match the car's rotation with a fixed speed if no Rigidbody
            Quaternion targetRotation = carTransform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
        
}
