using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerEnemy : MonoBehaviour
{
    Rigidbody2D rb;

    int direction = 1;
    bool cooldown = false;

    [SerializeField] float speed = 0.005f;
    [SerializeField] float sec = 3f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Move();
        if (cooldown == false) {
            StartCoroutine(ChangeDirection());
        }
    }

    void Move() {
        transform.Translate(direction*speed, 0, 0);
    }

    IEnumerator ChangeDirection() {
        cooldown = true;
        direction = direction * -1;
        yield return new WaitForSeconds(sec);
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        cooldown = false;
    }
}
