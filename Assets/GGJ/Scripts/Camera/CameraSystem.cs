using UnityEngine;

public static class CameraSystem
{
    private const float k_verticalRotSpeed = 3.0f;
    private const float k_horizontalRotSpeed = 3.0f;

    public static void UpdateCameraRotation(Transform camera)
    {
        Vector3 eulerAngles = camera.eulerAngles;
        eulerAngles.x -= Input.GetAxis("Mouse Y") * k_verticalRotSpeed;
        eulerAngles.y += Input.GetAxis("Mouse X") * k_horizontalRotSpeed;
        camera.eulerAngles = eulerAngles;
    }
}
