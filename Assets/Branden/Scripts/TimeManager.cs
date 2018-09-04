using UnityEngine;

public class TimeManager : MonoBehaviour {
    [SerializeField]
    float m_timeScale = 1f;
    public float TimeScale {
        get { return m_timeScale; }
        set { m_timeScale = value; }
    }

    static TimeManager m_instance = null;
    public static TimeManager Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<TimeManager>();
            }
            return m_instance;
        }
    }

    // Update is called once per frame
    void Update () {
        Time.timeScale = m_timeScale;
	}
}
