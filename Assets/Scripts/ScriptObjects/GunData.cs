using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public Sprite gunSprite;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 5f; // shots per second
    public int damage = 10;
}
