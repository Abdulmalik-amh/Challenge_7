using UnityEngine;
using UnityEngine.UI;

public class CursorFollower : MonoBehaviour
{
    public float mousePositionZ;
    void Update()
    {
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
