using UnityEngine;

public class SmallCubeCluster : CubeCluster {
    static SmallCubeCluster m_instance = null;
    public static SmallCubeCluster Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<SmallCubeCluster>();
            }
            return m_instance;
        }
    }
}
