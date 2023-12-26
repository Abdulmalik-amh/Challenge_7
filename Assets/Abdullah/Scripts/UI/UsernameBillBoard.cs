using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameBillBoard : MonoBehaviour
{
    Camera mineCamera;

    // Update is called once per frame
    void Update()
    {
        if(mineCamera == null)
        {
            mineCamera = FindAnyObjectByType<Camera>();
        }

        if (mineCamera == null)
            return;

        transform.LookAt(mineCamera.transform);
        transform.Rotate(Vector3.up * 180f);
    }
}
