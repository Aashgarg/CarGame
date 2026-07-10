using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public Sprite gunSprite;
    public float fireRate;
    public float bulletSpeed;
    public int damage;
    public int maxAmmo;
}
