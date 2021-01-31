using UnityEngine;

public static class LeverSystem
{
    public static void UpdateLeverInteractions(Transform player, Vector3 playerSize, KeyCodeSet inputKeys)
    {
        if (Input.GetKeyDown(inputKeys.m_interactKey))
        {
            Collider[] colliders = Physics.OverlapBox(player.position, playerSize * 0.5f, player.rotation, int.MaxValue, QueryTriggerInteraction.Collide);
            foreach (Collider col in colliders)
            {
                LeverMono lever = col.GetComponent<LeverMono>();
                if(lever != null)
                {
                    lever.RPCCallingToggle();
                }
            }
        }
    }
}
