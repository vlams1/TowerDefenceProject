using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {
	public List<WaveType> waveTypes;
	public int gridSize;
	public float waveTime;
	private float waveTimer;
	private int level;

	public static WaveController instance { get; private set; }
	private void Awake() {
		if (instance == null) instance = this;
		else Destroy(gameObject);
	}

	private void Start() {
		waveTimer = waveTime;
		level = 1;
	}

	private void Update() {
		waveTimer -= Time.deltaTime;
		if (waveTimer <= 0f) {
			SpawnWave(level);
			level = 1;
			waveTimer = waveTime;
		}
	}

	public void AddWeight(int weight) {
		level += weight;
	}

	public void SpawnWave(int weight) {
		while (weight > 0) weight -= SpawnGroup(weight);
	}

	public int SpawnGroup(int weight) {
		List<WaveType> random = new List<WaveType>();
		foreach (WaveType waveType in waveTypes)
			if (waveType.weight <= weight)
				for (int i = 0; i < waveType.chance; i++) 
					random.Add(waveType);
		if (random.Count == 0) return weight;
		WaveType newWave = random[Random.Range(0, random.Count)];
		int amount = Random.Range(newWave.min, newWave.max + 1);
		if (amount == 1) SpawnEnemy(newWave.enemy, new Vector2(Random.Range(-gridSize, gridSize), 
			Random.Range(-gridSize, gridSize)));
		else {
			Vector2 origin = new Vector2(Random.Range(-gridSize+newWave.dist, gridSize-newWave.dist), 
				Random.Range(-gridSize+newWave.dist, gridSize-newWave.dist));
			for (int i = 0; i < amount; i++)
				SpawnEnemy(newWave.enemy, origin + Vector2.up * Mathf.Sin((float)i / amount * 360f * Mathf.Deg2Rad) 
				                           + Vector2.right * Mathf.Cos((float)i / amount * 360f * Mathf.Deg2Rad));
			}
		return newWave.weight;
	}

	public void SpawnEnemy(EnemyType newType, Vector2 position) {
		if (Mathf.Abs(position.x) > Mathf.Abs(position.y)) position.x = position.x < 0f ? -gridSize : gridSize;
		else position.y = position.y < 0f ? -gridSize : gridSize;
		GameObject enemy = Instantiate(newType.prefab);
		enemy.transform.parent = transform;
	    enemy.transform.position = new Vector3(position.x, enemy.transform.position.y, position.y);
	    EnemyController controller = enemy.AddComponent<EnemyController>();
	    controller.speed = newType.speed;
	    controller.health = newType.health;
	    controller.strength = newType.strength;
	    controller.level = newType.level;
	}

	public void GameOver() {
		waveTimer = 100f;
		Destroy(gameObject);
	}
}
