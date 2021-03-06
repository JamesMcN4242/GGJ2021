using UnityEngine;

public static class PlayerMovement
{
    public enum MovementState
    {
        WALKING,
        CROUCHING,
        RUNNING
    }
    
    private static readonly LayerMask k_collisionLayer = ~LayerMask.GetMask("Player", "Ball");

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

    public static void MovePlayer(CharacterController player, Transform playerTrans, Transform facingDirection, Vector2 input, PlayerData playerData, MovementState movementType, PositionMono posMono, float powerupModifier, float deltaTime)
    {
        (float movementSpeed, Vector3 playerSize) = GetCurrentPlayerSpeedAndSize(movementType, playerData);
        float movementModifier = GetEnvironmentSpeedModifiers(playerTrans, playerSize);

        Vector3 forward = facingDirection.forward * input.y;
        forward.y = 0.0f;

        Vector3 right = facingDirection.right * input.x;
        right.y = 0.0f;

        Vector3 velocity = (forward + right).normalized * movementSpeed * movementModifier * powerupModifier;
        posMono.m_velocity = velocity;

        //Got to add gravity after or animator will always think we're running
        velocity += Physics.gravity;
        player.Move(velocity * deltaTime);
    }

    private static float GetEnvironmentSpeedModifiers(Transform player, Vector3 playerSize)
    {
        Collider[] colliders = Physics.OverlapBox(player.position, playerSize, player.rotation, int.MaxValue, QueryTriggerInteraction.Collide);
        foreach(Collider col in colliders)
        {
            Trap trap = col.GetComponent<Trap>();
            switch(trap)
            {
                case SlowTrap slowtrap when trap is SlowTrap:
                    return slowtrap.m_speedPercentageInTrap;
            }
        }

        return 1.0f;
    }

    public static Vector3 GetCurrentHeight(MovementState movementType, PlayerData playerData)
    {
        switch (movementType)
        {
            case MovementState.CROUCHING:
                return playerData.m_crouchSize;

            case MovementState.RUNNING:
            case MovementState.WALKING:
                return playerData.m_standingSize;

            default:
                Debug.LogError("Movement state not valid");
                return Vector3.one;
        }
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
