using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camerasettings : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    bool isRight = true;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(isRight)
            {
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = .7f;
                isRight = false;
            }
            else if (!isRight)
            {
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = .3f;
                isRight = true;
            }
        }
    }
}
