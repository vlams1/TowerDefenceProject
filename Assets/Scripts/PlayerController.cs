using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Camera cam;
    private CharacterController cc;
    private Vector3 forward;
    private Vector3 right;
    public float speed = 2f;
    
    public static PlayerController instance { get; private set; }

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    
    void Start() {
        cam = Camera.main;
        cc = GetComponent<CharacterController>();
    }
    
    void Update() {
        forward = cam.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = cam.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);
        cc.Move(Vector3.Normalize(forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal")) * speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.5f, 4.5f), transform.position.y, Mathf.Clamp(transform.position.z, -4.5f, 4.5f));
        Collider[] colliders = Physics.OverlapSphere(transform.position, .3f);
        foreach (Collider collider in colliders) {
            if (collider.gameObject.GetComponent<EnemyController>()) {
                UIController.instance.GameOver();
                WaveController.instance.GameOver();
                foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
                    Destroy(enemy.gameObject);
                Destroy(BuildController.instance.gameObject);
                Destroy(collider.gameObject);
                Destroy(gameObject);
                break;
            }
        }
    }
}
