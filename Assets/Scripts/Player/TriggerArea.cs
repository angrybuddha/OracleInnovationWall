/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Trigger area logic for players.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TriggerArea : MonoBehaviour
{
    //Type of trigger area
    [SerializeField]
    private Player_Old.PlayerMode triggerPlayerStatus = Player_Old.PlayerMode.None;
    public Player_Old.PlayerMode TriggerPlayerStatus
    {
        get { return triggerPlayerStatus; }
        set { triggerPlayerStatus = value; }
    }

    //Answer for when a player is within that trigger area
    [SerializeField]
    private int triggerPlayerAnswer = 0;
    public int TriggerPlayerAnswer
    {
        get { return triggerPlayerAnswer; }
        set { triggerPlayerAnswer = value; }
    }

    private string collectionLayer = "Player";

    public void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    //When a player is within the trigger area
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(collectionLayer))
        {
            if (!CheckIfExists(other.gameObject.GetComponent<Player_Old>()))
            {
                var player = other.gameObject.GetComponent<Player_Old>();

                player.PlayerAnswer = TriggerPlayerAnswer;
                player.Mode = TriggerPlayerStatus;

                Core.Instance._playerManager.ActivePlayers.Add(player);
            }
        }
    }

    //When a player exists the trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(collectionLayer))
        {
            var player = other.gameObject.GetComponent<Player_Old>();

            player.PlayerAnswer = 0;
            player.Mode = Player_Old.PlayerMode.None;

            Core.Instance._playerManager.ActivePlayers = Core.Instance._playerManager.ActivePlayers.Where(p => p != player).ToList<Player_Old>();
        }
    }

    //Checks if the player exists
    bool CheckIfExists(Player_Old checkPlayer)
    {
        var list =
            from p in Core.Instance._playerManager.ActivePlayers
            where p == checkPlayer
            select p;

        if (list.Count() > 0)
        {
            return true;
        }

        return false;
    }
}
