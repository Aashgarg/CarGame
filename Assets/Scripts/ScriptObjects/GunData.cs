using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public Sprite gunSprite;
    public GameObject bulletPrefab;
    public float fireRate;
    public float fireForce;
    public int damage;
}
