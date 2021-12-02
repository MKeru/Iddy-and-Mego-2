using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class goaltrigger : MonoBehaviour
{
    CatController iddy;
    CatControllerMego mego;
    Basketball ball;

    Vector3 iddySpawn;
    Vector3 megoSpawn;
    Vector3 ballSpawn;

    void Start() {
        iddy = FindObjectOfType<CatController>();
        iddySpawn = iddy.transform.position;
        mego = FindObjectOfType<CatControllerMego>();
        megoSpawn = mego.transform.position;
        ball = FindObjectOfType<Basketball>();
        ballSpawn = ball.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Basketball")) {
            Debug.Log("Trigger entered.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Basketball")) {
            Debug.Log("Object within trigger.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Basketball")) {
            Debug.Log("Object exited trigger.");
            iddy.transform.position = iddySpawn;
            mego.transform.position = megoSpawn;
            ball.transform.position = ballSpawn;
        }
    }
}
