using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public IddyController iddyPlayer;
    public MegoController megoPlayer;
    public CameraSwitcher cmCam;

    bool tutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            iddyPlayer = FindObjectOfType<IddyController>();
            tutorial = true;
        }
        else {
            iddyPlayer = FindObjectOfType<IddyController>();
            megoPlayer = FindObjectOfType<MegoController>();

            //mego is inactive on game start, player always starts as iddy
            megoPlayer.gameObject.SetActive(false);
        }
        
        cmCam = FindObjectOfType<CameraSwitcher>();
    }

    public void Respawn() {
        if (iddyPlayer.gameObject.activeSelf) {
            iddyPlayer.gameObject.SetActive(false);
            iddyPlayer.transform.position = iddyPlayer.spawnPoint;
            iddyPlayer.gameObject.SetActive(true);
        }
        else if (megoPlayer.gameObject.activeSelf) {
            megoPlayer.gameObject.SetActive(false);
            megoPlayer.transform.position = megoPlayer.spawnPoint;
            megoPlayer.gameObject.SetActive(true);
        }
    }

    public void Switch() {
        if (iddyPlayer.gameObject.activeSelf) {
            //set spawn of now inactive character to spawn point
            iddyPlayer.transform.position = iddyPlayer.spawnPoint;
            //deactivate current character
            iddyPlayer.gameObject.SetActive(false);
            //activate other character
            megoPlayer.gameObject.SetActive(true);
            //switch cinemachine follow target
            cmCam.FollowSwitch(megoPlayer.transform);
        }
        else if (megoPlayer.gameObject.activeSelf) {
            megoPlayer.transform.position = megoPlayer.spawnPoint;
            megoPlayer.gameObject.SetActive(false);
            iddyPlayer.gameObject.SetActive(true);
            cmCam.FollowSwitch(iddyPlayer.transform);
        }
    }
}
