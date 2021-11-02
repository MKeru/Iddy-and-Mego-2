using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 tPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, tPos, smoothFactor * Time.fixedDeltaTime);
        transform.position = tPos;
    }
}