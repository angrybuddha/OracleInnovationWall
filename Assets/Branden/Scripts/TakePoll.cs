using UnityEngine;
using System;

public class TakePoll : MonoBehaviour {
    Animator m_animator = null;

    public void Show(bool show) {
        if (m_animator == null) {
            m_animator = GetComponent<Animator>();
        }

        m_animator.SetBool("isEnabled", show);
    }

    public void Reset() {
        if (m_animator == null) {
            m_animator = GetComponent<Animator>();
        }

        m_animator.SetBool("isEnabled", true);
        m_animator.SetTrigger("reset");
    }
}
