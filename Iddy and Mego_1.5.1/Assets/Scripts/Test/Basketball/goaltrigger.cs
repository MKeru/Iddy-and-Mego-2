using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class goaltrigger : MonoBehaviour
{
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
            SceneManager.LoadScene(2);
        }
    }
}
