using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    CinemachineVirtualCamera cmCam;

    // Start is called before the first frame update
    void Start()
    {
        cmCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void FollowSwitch(Transform otherChar) {
        cmCam.m_Follow = otherChar;
    }
}
