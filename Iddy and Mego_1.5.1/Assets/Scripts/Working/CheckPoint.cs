using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    IddyController iddyPlayer;
    MegoController megoPlayer;
    LevelManager gameLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            iddyPlayer = FindObjectOfType<IddyController>();
        }
        else {
            iddyPlayer = FindObjectOfType<IddyController>();
            megoPlayer = FindObjectOfType<MegoController>();
        }

        gameLevelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            gameLevelManager.temp_spawnPoint = transform.position;
            Debug.Log("Spawn set");
        }
    }
}
