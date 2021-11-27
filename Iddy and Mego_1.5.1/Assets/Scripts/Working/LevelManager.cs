using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Player gamePlayer;
    // Start is called before the first frame update
    void Start()
    {
        gamePlayer = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn() {
        gamePlayer.gameObject.SetActive(false);
        gamePlayer.transform.position = gamePlayer.spawnPoint;
        gamePlayer.gameObject.SetActive(true);
    }
}
