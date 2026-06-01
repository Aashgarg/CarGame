using UnityEngine;

//Top down view camera that follow the car and also rotates when car is turning
public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        Quaternion targetRotation = carTransform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
        
}
