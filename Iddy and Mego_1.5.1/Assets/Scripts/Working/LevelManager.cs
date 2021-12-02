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

    public Vector3 temp_spawnPoint;

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
        
        temp_spawnPoint = iddyPlayer.transform.position;
        
        cmCam = FindObjectOfType<CameraSwitcher>();
    }

    void Update() {

    }

    public void Respawn() {
        if (iddyPlayer.gameObject.activeSelf) {
            //iddyPlayer.gameObject.SetActive(false);
            iddyPlayer.transform.position = temp_spawnPoint;
            //iddyPlayer.gameObject.SetActive(true);
        }
        else if (megoPlayer.gameObject.activeSelf) {
            //megoPlayer.gameObject.SetActive(false);
            megoPlayer.transform.position = temp_spawnPoint;
            //megoPlayer.gameObject.SetActive(true);
        }
    }

    public void Switch() {
        if (iddyPlayer.gameObject.activeSelf) {
            //activate other character
            megoPlayer.gameObject.SetActive(true);
            //set spawn of now inactive character to spawn point
            iddyPlayer.transform.position = temp_spawnPoint;
            megoPlayer.transform.position = temp_spawnPoint;
            //deactivate current character
            iddyPlayer.gameObject.SetActive(false);
            //switch cinemachine follow target
            cmCam.FollowSwitch(megoPlayer.transform);
        }
        else if (megoPlayer.gameObject.activeSelf) {
            iddyPlayer.gameObject.SetActive(true);
            megoPlayer.transform.position = temp_spawnPoint;
            iddyPlayer.transform.position = temp_spawnPoint;
            megoPlayer.gameObject.SetActive(false);
            cmCam.FollowSwitch(iddyPlayer.transform);
        }
    }
}
