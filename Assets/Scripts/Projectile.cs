using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Vector3 velocity;
    public float range;
    public int damage;
    public GameObject effectPrefab;

    private void Update() {
        velocity.y -= 10f * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        if (transform.position.y < 0) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach (Collider collider in colliders)
                if (collider.gameObject.GetComponent<EnemyController>()) collider.gameObject.GetComponent<EnemyController>().Damage(damage);
            GameObject effect = Instantiate(effectPrefab);
            effect.transform.position = transform.position;
            Destroy destroy = gameObject.AddComponent<Destroy>();
            destroy.destroy = 2f;
            Destroy(this);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
