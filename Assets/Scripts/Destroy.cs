using UnityEngine;

public class Destroy : MonoBehaviour {
    public float destroy;
    private void Start() {
        Destroy(gameObject, destroy);
    }
}
