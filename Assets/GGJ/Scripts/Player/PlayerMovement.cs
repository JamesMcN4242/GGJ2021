using UnityEngine;

public static class PlayerMovement
{
    public enum MovementState
    {
        WALKING,
        CROUCHING,
        RUNNING
    }

    public static MovementState GetMovementState(MovementState currentState, KeyCodeSet keyCodes)
    {
        if (Input.GetKey(keyCodes.m_runKey)) return MovementState.RUNNING;

        if(Input.GetKeyDown(keyCodes.m_crouchToggleKey))
        {
            return currentState == MovementState.CROUCHING ? MovementState.WALKING : MovementState.CROUCHING;
        }

        return currentState == MovementState.RUNNING ? MovementState.WALKING : currentState;
    }

    public static Vector2 GetPlayerMovement(KeyCodeSet keyCodes)
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(keyCodes.m_upKey)) input.y++;
        if (Input.GetKey(keyCodes.m_downKey)) input.y--;

        if (Input.GetKey(keyCodes.m_rightKey)) input.x++;
        if (Input.GetKey(keyCodes.m_leftKey)) input.x--;

        return input;
    }

    public static void MovePlayer(Transform player, Transform facingDirection, Vector2 movement, PlayerData playerData, MovementState movementType, float deltaTime)
    {
        Vector3 newPos = player.position;

        (float movementSpeed, Vector3 playerSize) = GetCurrentPlayerSpeedAndSize(movementType, playerData);
        float moveDistance = deltaTime * movementSpeed;

        Vector3 forward = facingDirection.forward;
        forward.y = 0.0f;

        Vector3 right = facingDirection.right;
        right.y = 0.0f;

        newPos += moveDistance * right * movement.x;
        newPos += moveDistance * forward * movement.y;

        if (Physics.CheckBox(newPos, playerSize * 0.5f, player.rotation, int.MaxValue))
        {
            return;
        }

        player.position = newPos;
    }
    
    private static (float movementSpeed, Vector3 playerSize) GetCurrentPlayerSpeedAndSize(MovementState movementType, PlayerData playerData)
    {
        switch (movementType)
        {
            case MovementState.CROUCHING:
                return (playerData.m_crouchSpeed, playerData.m_crouchSize);

            case MovementState.RUNNING:
                return (playerData.m_runSpeed, playerData.m_standingSize);

            case MovementState.WALKING:
                return (playerData.m_walkSpeed, playerData.m_standingSize);

            default:
                Debug.LogError("Movement state not valid");
                return (0f, Vector3.one);
        }
    }
}
