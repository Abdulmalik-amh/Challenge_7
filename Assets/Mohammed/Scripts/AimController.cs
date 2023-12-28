using System.Collections;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Adjustable Parameters")]
    [SerializeField, Range(0.1f, 10f)] private float rotationSpeed = 5f;

    private void Update()
    {
        AimHands();
    }

    private void AimHands()
    {
        Vector3 cursorPosition = GetCursorWorldPosition();

        if (cursorPosition != Vector3.zero)
        {
            RotateTowardsCursor(leftHand, cursorPosition);
            RotateTowardsCursor(rightHand, cursorPosition);
        }
    }

    private Vector3 GetCursorWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void RotateTowardsCursor(Transform hand, Vector3 cursorPosition)
    {
        Vector3 direction = cursorPosition - hand.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        hand.rotation = Quaternion.Slerp(hand.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
