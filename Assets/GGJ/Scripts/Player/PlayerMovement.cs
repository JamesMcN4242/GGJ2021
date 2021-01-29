using UnityEngine;

public static class PlayerMovement
{
    public static Vector2 GetPlayerMovement(KeyCodeSet keyCodes)
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(keyCodes.m_upKeyCode)) input.y++;
        if (Input.GetKey(keyCodes.m_downKeyCode)) input.y--;

        if (Input.GetKey(keyCodes.m_rightKeyCode)) input.x++;
        if (Input.GetKey(keyCodes.m_leftKeyCode)) input.x--;

        return input;
    }

    public static void MovePlayer(Transform player, Transform facingDirection, Vector2 movement, float movementSpeed, float deltaTime)
    {
        Vector3 newPos = player.position;
        float moveDistance = deltaTime * movementSpeed;

        Vector3 forward = facingDirection.forward;
        forward.y = 0.0f;

        Vector3 right = facingDirection.right;
        right.y = 0.0f;

        newPos += moveDistance * right * movement.x;
        newPos += moveDistance * forward * movement.y;
        player.position = newPos;
    }

}
