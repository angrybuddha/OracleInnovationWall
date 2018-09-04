using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using RawPlayer = MQTTListener.RawPlayer;

public class PlayerManager : MonoBehaviour {

    [Serializable]
    public class PlayerColor {
        public Player player = null;
        public Color color = Color.red;
    }

    [SerializeField]
    List<PlayerColor> m_playerColorList = new List<PlayerColor>();
    public List<PlayerColor> PlayerColorList {
        get { return m_playerColorList; }
    }

    [SerializeField]
    float m_playerUpdateTime = .1f;

    List<Player> m_activePlayers = new List<Player>();
    public List<Player> ActivePlayers {
        get { return m_activePlayers; }
    }

    bool m_initialized = false;

    static PlayerManager m_instance = null;
    public static PlayerManager Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<PlayerManager>();
            }
            return m_instance;
        }
    }

    public void Init() {
        if (m_initialized) {
            return;
        }

        foreach (PlayerColor playerColor in m_playerColorList) {
            Player player = playerColor.player;
            player.Awake();
            player.SetActiveColor(playerColor.color);
        }

        m_initialized = true;
        StartCoroutine(UpdatePlayerInfo());
    }

    void SetPlayerActive(int playerIndex, bool isActive, RawPlayer rawPlayer = null) {
        Player player = m_playerColorList[playerIndex].player;
        SetPlayerActive(player, isActive, rawPlayer);
    }

    void SetPlayerActive(Player player, bool isActive, RawPlayer rawPlayer = null) {
        if(rawPlayer != null) {
            player.RawPlayer = rawPlayer;
        }
        
        if (isActive) {
            if (!m_activePlayers.Contains(player)) {
                m_activePlayers.Add(player);
                Vector3 worldPos = player.transform.position;
                player.SetWorldPosition(ref worldPos);
                player.transform.position = worldPos;

                player.gameObject.SetActive(true);
                player.ShowInactive(true);
                player.ShowCountdown(false);
            }
        }
        else if (m_activePlayers.Contains(player)) {
            m_activePlayers.Remove(player);
            player.gameObject.SetActive(false);
            PanelManager.Instance.RemovePlayerFromAnyPanel(player);
        }
    }

    //Need to convert because kinect view sometimes bigger then screen view...
    public float GetScreenPlayerX(float playerX){
        float percX = StartupSettings.Instance.ScreenPercentX;
        float halfPercX = percX / 2f;

        float minPerc = .5f - halfPercX;

        float screenX = (playerX - minPerc) / (percX);

        return screenX;
    }

    public void UpdatePlayerPositions() {
        List<RawPlayer> rawPlayers = MQTTListener.Instance.RawPlayers;

        //For showing player cubes...
        foreach (PlayerColor playerColor in m_playerColorList) {
            Player player = playerColor.player;
            bool playerIsInactive = true;
            foreach (RawPlayer rawPlayer in rawPlayers) {
                //If the player should be active...
                if (player.RawPlayer.Id == rawPlayer.Id) {
                    playerIsInactive = false;
                    SetPlayerActive(player, true, rawPlayer);
                    break;
                }
            }

            if (playerIsInactive) {
                SetPlayerActive(player, false);
            }
        }

        //For cube spawning...
        float endStreamSection = 0.75f;
        int numSections = 13;
        float offset = 1f / numSections;

        bool[] activeIndices = {
            false, false, false, false, false
        };

        foreach (RawPlayer player in rawPlayers) {
            string playerId = player.Id;
            
            float xPos = GetScreenPlayerX(player.X);
            //We are in front of first 3 screens...
            if (xPos < endStreamSection) {
                int index = xPos < (offset * 2) ? 0 :
                    xPos < (offset * 4) ? 1 :
                    xPos < (offset * 6) ? 2 :
                    xPos < (offset * 8) ? 3 : 4;

                activeIndices[index] = true;
            }   //else we are in front of the polling side panel...
        }

        for (int i = 0, count = activeIndices.Length; i < count; ++i) {
            if (activeIndices[i]) {
                CubeSpawner.Instance.SpawnStreamingCube(i);
            }
            else {
                CubeSpawner.Instance.ResetIndex(i);
            }
        }
    }

    IEnumerator UpdatePlayerInfo() {
        do {
            yield return new WaitForSeconds(m_playerUpdateTime);
            UpdatePlayerPositions();
        } while (true);
    }
}
