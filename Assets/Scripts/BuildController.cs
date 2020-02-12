using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	public List<TowerType> towerTypes;
	public int gridSize;
	
	private int selected = 0;

	public int money;

	public static BuildController instance { get; private set; }

	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	void Update() {
		selected = selected + Mathf.FloorToInt(Input.mouseScrollDelta.y);
		while (selected >= towerTypes.Count) selected -= towerTypes.Count;
		while (selected < 0) selected += towerTypes.Count;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 hit = ray.origin + ray.direction * ray.origin.y / -ray.direction.y;
		if (ray.direction.y < 0f && Math.Abs(hit.x) - 1f < gridSize && Math.Abs(hit.z) - 1f < gridSize) {
			transform.position =
				new Vector3(Mathf.Clamp(Mathf.Round(hit.x), -gridSize, gridSize), 0f,
					Mathf.Clamp(Mathf.Round(hit.z), -gridSize, gridSize));
			if (Input.GetMouseButtonDown(0)) {
				Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up * .5f, new Vector3(.5f, .5f, .5f));
				if (colliders.Length <= 0) {
					if (money >= towerTypes[selected].cost) {
						GameObject tower = Instantiate(towerTypes[selected].prefab);
						tower.transform.position += transform.position;
						TowerController controller = tower.AddComponent<TowerController>();
						controller.UpdateType(towerTypes[selected]);
						money -= towerTypes[selected].cost;
					}
					else UIController.instance.Error("You can't afford that tower!");
				}
				else {
					bool fail = true;
					foreach (Collider collider in colliders) {
						if (!collider.isTrigger && collider.gameObject.GetComponent<TowerController>()) {
							fail = false;
							TowerType type = collider.gameObject.GetComponent<TowerController>().GetTowerType();
							if (type.upgrade) {
								if (money >= type.upgrade.cost) {
									GameObject tower = Instantiate(type.upgrade.prefab);
									tower.transform.position = collider.gameObject.transform.position;
									tower.transform.parent = transform.parent;
									TowerController controller = tower.AddComponent<TowerController>();
									controller.UpdateType(type.upgrade);
									Destroy(collider.gameObject);
									money -= type.upgrade.cost;
									break;
								}
								UIController.instance.Error("You can't afford this upgrade!");
							}
							else UIController.instance.Error("This tower isn't upgradable!");
						}
					}
					if (fail) UIController.instance.Error("You can't place a tower here!");
				}
			}
		}
		else {
			transform.position = Vector3.down;
		}
	}
}
