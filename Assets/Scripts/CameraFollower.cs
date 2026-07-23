using UnityEngine;

//Top down view camera that follow the car and also rotates when car is turning
public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zOffset;
    [SerializeField] private float verticalOffset = 1.5f;
    [SerializeField] private float speedOffsetMultiplier = 0.08f;
    [SerializeField] private float maxSpeedOffset = 2f;
    [SerializeField] Rigidbody2D carRigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (carRigidbody == null && carTransform != null)
        {
            carRigidbody = carTransform.GetComponent<Rigidbody2D>();
        }
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
        float currentSpeed = carRigidbody != null ? carRigidbody.linearVelocity.magnitude : 0f;
        float dynamicOffset = verticalOffset + Mathf.Min(currentSpeed * speedOffsetMultiplier, maxSpeedOffset);

        Vector3 targetPosition = new Vector3(
            carTransform.position.x,
            carTransform.position.y + dynamicOffset,
            zOffset
        );
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    void RotateCamera()
    {
        if (carTransform != null)
        {
            Quaternion targetRotation = carTransform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
        
}
