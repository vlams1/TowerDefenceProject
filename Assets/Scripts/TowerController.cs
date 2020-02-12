using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerController : MonoBehaviour {
    private TowerType type;
    private BoxCollider collider;
    private int health;
    private int maxhealth;
    private float cooldown;
    private Tween spawn;

    void Start() {
        collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(.9f, 2f, .9f);
        collider.center = Vector3.up;
        spawn = transform.DOScale(Vector3.one, .2f).From(Vector3.one * .5f);
    }

    private void OnDestroy() {
        spawn.Kill();
    }

    void Update() {
        cooldown = Mathf.Max(cooldown - Time.deltaTime, 0f);
        if (cooldown == 0f) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, type.range);
            List<GameObject> enemies = new List<GameObject>();
            foreach (Collider collider in colliders)
                if (collider.gameObject.GetComponent<EnemyController>())
                    enemies.Add(collider.gameObject);
            if (enemies.Count > 0) {
                Vector2 target = Vector2.zero;
                switch (type.projectile.target) {
                    case Target.Random:
                        GameObject enemy1 = enemies[Random.Range(0, enemies.Count)];
                        target.x = enemy1.transform.position.x - transform.position.x;
                        target.y = enemy1.transform.position.z - transform.position.z;
                        target += enemy1.GetComponent<EnemyController>().direction * type.projectile.anticipation;
                        target *= type.projectile.strength;
                        break;
                    case Target.Center:
                        foreach (GameObject enemy2 in enemies) {
                            target.x += enemy2.transform.position.x - transform.position.x;
                            target.y += enemy2.transform.position.z - transform.position.z;
                            target += enemy2.GetComponent<EnemyController>().direction * type.projectile.anticipation;
                        }
                        target /= enemies.Count;
                        target *= type.projectile.strength;
                        break;
                    case Target.Closest:
                        GameObject enemy = enemies[0];
                        foreach (GameObject enemy3 in enemies) {
                            if (Vector3.Distance(enemy3.transform.position, transform.position) <
                                Vector3.Distance(enemy.transform.position, transform.position))
                                enemy = enemy3;
                        }
                        target.x = enemy.transform.position.x - transform.position.x;
                        target.y = enemy.transform.position.z - transform.position.z;
                        target += enemy.GetComponent<EnemyController>().direction * type.projectile.anticipation;
                        target *= type.projectile.strength;
                        break;
                }
                GameObject projectile = Instantiate(type.projectile.prefab);
                projectile.transform.position = transform.position + Vector3.up;
                projectile.GetComponent<Projectile>().velocity = new Vector3(target.x, type.projectile.height, target.y);
                cooldown = type.cooldown;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, type.range);
    }

    public void UpdateType(TowerType newType) {
        type = newType;
        maxhealth = type.health;
        health = maxhealth;
    }

    public TowerType GetTowerType() {
        return type;
    }

    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            transform.DOScale(Vector3.zero, .2f);
            Destroy destroy = gameObject.AddComponent<Destroy>();
            destroy.destroy = .2f;
            Destroy(this);
        }
    }
}
