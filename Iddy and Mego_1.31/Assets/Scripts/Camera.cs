using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update

    public float camDistOffset = 10;
    private Camera mainCamera;
    private GameObject player;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInfo = player.transform.transform.position;
        mainCamera.transform.position = new Vector3()
    }
}
