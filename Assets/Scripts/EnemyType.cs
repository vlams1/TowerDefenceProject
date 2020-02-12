using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Type", menuName = "Game Data/Enemy Type")]
public class EnemyType : ScriptableObject {
    public GameObject prefab;
    public float speed;
    public int health;
    public int strength;
    public int level;
}
