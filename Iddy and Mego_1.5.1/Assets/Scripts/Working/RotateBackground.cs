using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackground : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, speed*Time.fixedDeltaTime);
    }
}
