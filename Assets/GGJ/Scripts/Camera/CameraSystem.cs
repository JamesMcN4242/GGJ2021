using UnityEngine;

public static class CameraSystem
{
    private const float k_verticalRotSpeed = 3.0f;
    private const float k_horizontalRotSpeed = 3.0f;

    public static void UpdateCameraRotation(Transform player, ref Vector3 playerEulerAngles, Transform camera, ref Vector3 cameraEulerAngles)
    {
        cameraEulerAngles.x -= Input.GetAxis("Mouse Y") * k_verticalRotSpeed;
        cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, -90, 90);
        playerEulerAngles.y += Input.GetAxis("Mouse X") * k_horizontalRotSpeed;

        camera.localEulerAngles = cameraEulerAngles;
        player.eulerAngles = playerEulerAngles;
    }
}
