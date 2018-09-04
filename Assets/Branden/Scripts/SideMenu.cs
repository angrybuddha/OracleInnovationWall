using UnityEngine;

public class SideMenu : MonoBehaviour {
    Animator m_animator = null;
    Animator m_cameraViewAnimator = null;
    Animator m_joinConversationAnimator = null;
    Animator m_getStartedAnimator = null;
    Animator m_takePollAnimator = null;

    static SideMenu m_instance = null;
    public static SideMenu Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<SideMenu>();
            }
            return m_instance;
        }
    }

    void Awake() {
        Find(transform);
    }

    void Find(Transform t) {
        foreach (Transform child in t) {
            GameObject childObj = child.gameObject;
            string childName = childObj.name;

            if (childName == "RawImage - SidePanel") {
                m_cameraViewAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - JoinConversation") {
                m_joinConversationAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - GetStarted") {
                m_getStartedAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Text - TakePoll") {
                m_takePollAnimator = child.GetComponent<Animator>();
            }

            Find(child);
        }
    }

    public void ShowBackground(bool show) {
        if (m_animator == null) {
            m_animator = GetComponent<Animator>();
        }

        m_animator.SetBool("isEnabled", show);
    }

    public void ShowCameraView(bool show) {
        if (m_cameraViewAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_cameraViewAnimator.SetBool("isEnabled", show);
    }

    public void ShowJoinConversation(bool show) {
        if (m_joinConversationAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_joinConversationAnimator.SetBool("isEnabled", show);
    }

    public void ShowTakePoll(bool show) {
        if (m_takePollAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_takePollAnimator.SetBool("isEnabled", show);
    }

    public void ShowGetStarted(bool show) {
        if (m_getStartedAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_getStartedAnimator.SetBool("isEnabled", show);
    }
}
