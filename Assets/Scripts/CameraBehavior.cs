using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform car;

    public float smoothTime = 0.2f;
    public float maxMouseLead = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (car == null)
            return;

        // Convert mouse position to world position
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -transform.position.z;

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(mouseScreen);

        // Limit how far the mouse can pull the camera
        Vector3 carToMouse = mouseWorld - car.position;

        carToMouse = Vector3.ClampMagnitude(
            carToMouse,
            maxMouseLead
        );

        Vector3 limitedMousePosition =
            car.position + carToMouse;

        // Midpoint between car and mouse
        Vector3 targetPosition =
            (car.position + limitedMousePosition) / 2f;

        // Keep camera Z position unchanged
        targetPosition.z = transform.position.z;

        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}