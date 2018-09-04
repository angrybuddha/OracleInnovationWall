using UnityEngine;
using System.Collections;

public class TriggerAreaGroup : MonoBehaviour
{
    void OnDisable()
    {
        if (Core.Instance != null && Core.Instance._playerManager.ActivePlayers != null)
        {
            foreach (var player in Core.Instance._playerManager.ActivePlayers)
            {
                player.GetComponent<Player_Old>().PlayerAnswer = 0;
            }

            Core.Instance._playerManager.ActivePlayers.Clear();

            Core.Instance._playerManager.ResetAllPlayerModeTime();
        }
    }
}
