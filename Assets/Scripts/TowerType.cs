using UnityEngine;

[CreateAssetMenu(fileName = "Tower Type", menuName = "Game Data/Tower Type")]
public class TowerType : ScriptableObject {
    public int cost;
    public GameObject prefab;
    public int health;
    public float range;
    public float cooldown;
    public ProjectileType projectile;
    public TowerType upgrade;
}
