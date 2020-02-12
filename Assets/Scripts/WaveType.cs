using UnityEngine;

[CreateAssetMenu(fileName = "Wave Type", menuName = "Game Data/Wave Type")]
public class WaveType : ScriptableObject {
	public int chance;
	public int weight;
	public int min;
	public int max;
	public float dist;
	public EnemyType enemy;
}