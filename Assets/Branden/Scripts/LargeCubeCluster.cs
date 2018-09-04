using UnityEngine;

public class LargeCubeCluster : CubeCluster {
    static LargeCubeCluster m_instance = null;
    public static LargeCubeCluster Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<LargeCubeCluster>();
            }
            return m_instance;
        }
    }
}
