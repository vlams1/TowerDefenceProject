using UnityEngine;

public class EnemyController : MonoBehaviour {
    private GameObject target;
    private CharacterController cc;
    public float speed;
    public int health;
    public int strength;
    public int level;
    public Vector2 direction;
    private float cooldown;
    private void Start() {
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        target = PlayerController.instance.gameObject;
        cooldown = 0f;
    }

    private void Update() {
        cooldown = Mathf.Max(cooldown - Time.deltaTime, 0f);
        if (cooldown == 0f) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, .4f);
            foreach (Collider collider in colliders) {
                if (collider.gameObject.GetComponent<TowerController>()) {
                    collider.gameObject.GetComponent<TowerController>().Damage(strength);
                    cooldown = .5f;
                    break;
                }
            }
        }

        Vector3 pos = transform.position;
        cc.Move(Vector3.Normalize(target.transform.position - transform.position) * speed * Time.deltaTime);
        Vector3 dir = (transform.position - pos) / Time.deltaTime;
        direction.x = dir.x;
        direction.y = dir.z;
        transform.eulerAngles = new Vector3(0f, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg, 0f);
    }

    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            BuildController.instance.money += level * 50;
            UIController.instance.AddPoints(level);
            WaveController.instance.AddWeight(level);
            Destroy(gameObject);
        }
    }
}
