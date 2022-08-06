using UnityEngine;

public class SunRotation : MonoBehaviour
{
    void Start()
    {
        gameObject.LeanRotateZ(5f, 30f).setLoopPingPong();
    }
}
