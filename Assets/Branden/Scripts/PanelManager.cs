using UnityEngine;
using System;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour {

    public class Panel {
        public event Action<bool> OnActive = null;
        List<Player> m_presentPlayers = new List<Player>();

        bool m_isActive = false;
        public bool IsActive {
            get { return m_isActive; }
        }

        public void Add(Player player) {
            if(m_presentPlayers.Count == 0) {
                m_presentPlayers.Add(player);
                m_isActive = true;
                if (OnActive != null) {
                    OnActive(true);
                }
            }
            else if(!m_presentPlayers.Contains(player)) {  //Don't fall through because of
                    //  order when added, in condition above...
                m_presentPlayers.Add(player);
            }
        }

        public void Remove(Player player) {
            if (m_presentPlayers.Contains(player)) {
                m_presentPlayers.Remove(player);

                if (m_presentPlayers.Count == 0) {
                    m_isActive = false;
                    if (OnActive != null) {
                        OnActive(false);
                    }
                }
            }
        }

        public void Clear() {
            m_presentPlayers.Clear();
            m_isActive = false;
        }

        public bool ContainsPlayer(Player player) {
            bool containsPlayer = m_presentPlayers.Contains(player);
            return containsPlayer;
        }
    }

    Panel[] m_panels = new Panel[] {
        new Panel(), new Panel(), new Panel(), new Panel()
    };

    static PanelManager m_instance = null;
    public static PanelManager Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<PanelManager>();
            }
            return m_instance;
        }
    }

    public void SubscribeToActivePanel(int panelIndex, Action<bool> OnActive) {
        m_panels[panelIndex].OnActive += OnActive;
    }

    public bool IsPanelActive(int panelIndex) {
        bool isActive = m_panels[panelIndex].IsActive;
        return isActive;
    }

    public void AddPlayerToPanel(int panelIndex, Player player) {
        m_panels[panelIndex].Add(player);
    }

    public void RemovePlayerFromPanel(int panelIndex, Player player) {
        m_panels[panelIndex].Remove(player);
    }

    public void RemovePlayerFromAnyPanel(Player player) {
        foreach(Panel panel in m_panels) {
            panel.Remove(player);
        }
    }

    public void AddPlayerToPanel(Player player) {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.transform.position);
        int index = (int)(Mathf.Clamp(m_panels.Length * viewportPos.x, 0, m_panels.Length - 1f));
        
        Panel panel = m_panels[index];
        if (!panel.ContainsPlayer(player)) {
            panel.Add(player);
            for(int i = 0, count = m_panels.Length; i < count ; ++i) {
                if(i != index) {
                    m_panels[i].Remove(player);
                }
            }
        }

        AddPlayerToPanel(index, player);
    }

    public bool ContainsPlayer(Player player, int panelIndex) {
        return m_panels[panelIndex].ContainsPlayer(player);
    }

    public void ClearAllPanels() {
        for (int i = 0, count = m_panels.Length; i < count; ++i) {
            ClearPanel(i);
        }
    }

    //Clears all players from panel, and resets active flag...
    public void ClearPanel(int panelIndex) {
        m_panels[panelIndex].Clear();
    }
}
