using UnityEngine;

public class EqualCubeCluster : CubeCluster {
    static EqualCubeCluster m_instance = null;
    public static EqualCubeCluster Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<EqualCubeCluster>();
            }
            return m_instance;
        }
    }
}
