using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
