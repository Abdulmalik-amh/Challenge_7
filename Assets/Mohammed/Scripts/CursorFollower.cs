using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CursorFollower : MonoBehaviour
{

    PhotonView view;
    
    
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    public float mousePositionZ;
    void Update()
    {
        if (!view.IsMine)
            return;

        // Get the current mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Set a constant distance from the camera in the z-axis
        mousePosition.z = mousePositionZ;

        // Convert the screen position to a world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Set the game object's position to the world position
        transform.position = worldPosition;

    }

    
}
