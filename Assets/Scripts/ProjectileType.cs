using UnityEngine;

public enum Target {
	Random,
	Center,
	Closest
}

[CreateAssetMenu(fileName = "Projectile Type", menuName = "Game Data/Projectile Type")]
public class ProjectileType : ScriptableObject {
	public Target target;
	public float height;
	public float strength;
	public float anticipation;
	public GameObject prefab;
}