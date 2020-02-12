using UnityEngine;

public class CameraController : MonoBehaviour {
    private Vector2 rotation = new Vector2(45, 30);
    public float sensitivity = 1f;
    public float keysensitivity = 80f;
    void Update() {
        if (Input.GetMouseButton(1)) {
            rotation.x += Input.GetAxisRaw("Mouse X") * sensitivity;
            rotation.y -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        }
        rotation.x += Input.GetAxisRaw("Cam X") * keysensitivity * Time.deltaTime;
        rotation.y += Input.GetAxisRaw("Cam Y") * keysensitivity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, 20, 75);
        transform.eulerAngles = new Vector3(rotation.y, rotation.x, 0f);
        transform.position = transform.forward * -8f - transform.up * 1f;
    }
}
