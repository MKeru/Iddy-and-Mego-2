using UnityEngine;

public class SPCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Awake()
    {
        transform.position = target.position + offset;
    }

    private void FixedUpdate()
    {
        transform.position = target.position + offset;
    }   
}
