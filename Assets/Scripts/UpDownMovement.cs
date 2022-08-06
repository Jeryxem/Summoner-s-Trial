using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    public float upValue;
    public float time;

    void Start()
    {
        gameObject.LeanMoveY(transform.position.y  + upValue, time).setLoopPingPong();
    }
}
