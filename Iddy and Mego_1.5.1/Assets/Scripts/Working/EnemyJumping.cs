using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumping : MonoBehaviour
{
    Rigidbody2D rb;
    bool isJumping = false;
    [SerializeField] int jumpForce = 100;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping == false) {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump() {
        isJumping = true;
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(3f);
        isJumping = false;
    }
}
