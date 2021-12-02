using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFlipMove : MonoBehaviour
{
    VaseLocation pos;

    Vector3 vasePos;
    Vector3 currentPos;

    void Start() {
        pos = FindObjectOfType<VaseLocation>();
        currentPos = transform.position;
    }

    void FixedUpdate() {
        vasePos = pos.transform.position;
        currentPos.x = vasePos.x - 0.4f;
        currentPos.y = vasePos.y + 0.03f;
        transform.position = currentPos;
    }
}
