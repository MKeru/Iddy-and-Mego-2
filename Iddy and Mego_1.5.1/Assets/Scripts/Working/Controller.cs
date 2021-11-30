using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public IddyController iddyPlayer;
    public MegoController megoPlayer;

    LevelManager gameLevelManager;

    private Vector2 movementInput = Vector2.zero;
    float horizontalValue;

    // Start is called before the first frame update
    void Start()
    {
        gameLevelManager = FindObjectOfType<LevelManager>();
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            iddyPlayer = FindObjectOfType<IddyController>();
        }
        else {
            iddyPlayer = FindObjectOfType<IddyController>();
            megoPlayer = FindObjectOfType<MegoController>();
            //megoPlayer.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = movementInput.x;
    }

    void FixedUpdate()
    {
        float dir = horizontalValue;

        if (iddyPlayer.gameObject.activeSelf) {
            iddyPlayer.Move(dir);
        }
        else if (megoPlayer.gameObject.activeSelf) {
            megoPlayer.Move(dir);
        }
    }

    public void OnMove(InputValue input) {
        Vector2 inputVec = input.Get<Vector2>();
        movementInput = new Vector2(inputVec.x, inputVec.y);
    }

    public void OnJump() {
        if (iddyPlayer.gameObject.activeSelf) {
            iddyPlayer.Jump();
        }
        else if (megoPlayer.gameObject.activeSelf) {
            megoPlayer.Jump();
        }
    }

    public void OnSwitch() {
        if (SceneManager.GetActiveScene().buildIndex != 1){
            gameLevelManager.Switch();
        }
    }

    public void OnSwipe() {
        if (megoPlayer.gameObject.activeSelf) {
            megoPlayer.Swipe();
        }
    }
}
