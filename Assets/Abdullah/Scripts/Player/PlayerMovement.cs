using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public float speed =7f;
    Transform cam;
    public GameObject camPlayer;

    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
       characterController = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        if (!view.IsMine)
        {
            Destroy(camPlayer.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!view.IsMine)
            return;

        Move();

    }


    private void Move()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(h, 0f, v);

        if (movement.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        movement = cam.TransformDirection(movement);
        movement = new Vector3(movement.x, 0, movement.z);

        characterController.Move(movement * speed * Time.deltaTime);
    }
}
