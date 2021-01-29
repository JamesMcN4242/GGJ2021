using UnityEngine;

public static class CameraSystem
{
    private const float k_verticalRotSpeed = 3.0f;
    private const float k_horizontalRotSpeed = 3.0f;

    public static void UpdateCameraRotation(Transform camera, ref Vector3 eulerAngles)
    {
        eulerAngles.x -= Input.GetAxis("Mouse Y") * k_verticalRotSpeed;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -90, 90);
        eulerAngles.y += Input.GetAxis("Mouse X") * k_horizontalRotSpeed;

        camera.eulerAngles = eulerAngles;
    }
}
