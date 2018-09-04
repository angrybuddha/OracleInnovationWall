using UnityEngine;

public class LeftMenu : MonoBehaviour {
    Animator m_animator = null;

    static LeftMenu m_instance = null;
    public static LeftMenu Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<LeftMenu>();
            }
            return m_instance;
        }
    }

    public void ShowBackground(bool show) {
        if (m_animator == null) {
            m_animator = GetComponent<Animator>();
        }

        m_animator.SetBool("isEnabled", show);
    }
}
