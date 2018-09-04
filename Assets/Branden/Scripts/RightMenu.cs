using UnityEngine;
using System.Collections;

public class RightMenu : MonoBehaviour {
    Animator m_animator = null;
    Animator m_pollAAnimator = null;
    Animator m_noAnimator = null;           //Shows "no" text when question asked...
    Animator m_activeNoAnimator = null;    //Shows active "no" text when activated...
    Animator m_inactiveNoAnimator = null;  //Shows inactive "no" text when inactivated...
    Animator m_largeResultsAnimator = null; //Shows large cube cluster and results text...
    Animator m_smallResultsAnimator = null; //Shows small cube cluster and results text...
    Animator m_equalResultsAnimator = null; //Shows equal cube cluster and results text...
    Animator m_largePercentAnimator = null;
    Animator m_smallPercentAnimator = null;

    TMPro.TextMeshProUGUI m_pollALargePercent = null;
    TMPro.TextMeshProUGUI m_pollASmallPercent = null;
    TMPro.TextMeshProUGUI m_pollAEqualPercent = null;

    TMPro.TextMeshProUGUI m_activePollAAnswer = null;
    public TMPro.TextMeshProUGUI ActivePollAAnswer {
        get { return m_activePollAAnswer; }
    }

    TMPro.TextMeshProUGUI m_inactivePollAAnswer = null;
    public TMPro.TextMeshProUGUI InactivePollAAnswer {
        get { return m_inactivePollAAnswer; }
    }

    static RightMenu m_instance = null;
    public static RightMenu Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<RightMenu>();
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

            if (childName == "Panel - PollA") {
                m_pollAAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - No") {
                m_noAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - ActiveNo") {
                m_activeNoAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - InactiveNo") {
                m_inactiveNoAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - LargeResults") {
                m_largeResultsAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - SmallResults") {
                m_smallResultsAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - EqualResults") {
                m_equalResultsAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Text - LargePercent") {
                m_largePercentAnimator = child.GetComponent<Animator>();
                m_pollALargePercent = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Text - SmallPercent") {
                m_smallPercentAnimator = child.GetComponent<Animator>();
                m_pollASmallPercent = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Text - EqualPercent") {
                m_pollAEqualPercent = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Text - ActiveNo") {
                m_activePollAAnswer = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Text - InactiveNo") {
                m_inactivePollAAnswer = child.GetComponent<TMPro.TextMeshProUGUI>();
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

    public void ShowPollA(bool show) {
        if (m_pollAAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_pollAAnimator.SetBool("isEnabled", show);
    }

    public void MoveDownNo(bool moveDown) {
        if (m_noAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_noAnimator.SetBool("isEnabled", moveDown);
    }

    public void ShowActiveNo(bool show) {
        if (m_activeNoAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_activeNoAnimator.SetBool("isEnabled", show);
    }

    public void HideInactiveNo(bool hide) {
        if (m_inactiveNoAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_inactiveNoAnimator.SetBool("isEnabled", hide);
    }

    public void ShowLargeResults(bool show) {
        if (m_largeResultsAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_largeResultsAnimator.SetBool("isEnabled", show);
    }

    public void ShowSmallResults(bool show) {
        if (m_smallResultsAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_smallResultsAnimator.SetBool("isEnabled", show);
    }

    public void ShowEqualResults(bool show) {
        if (m_equalResultsAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_equalResultsAnimator.SetBool("isEnabled", show);
    }

    public void HideLargePercent(bool hide) {
        if (m_largePercentAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_largePercentAnimator.SetBool("isEnabled", hide);
    }

    public void HideSmallPercent(bool hide) {
        if (m_smallPercentAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_smallPercentAnimator.SetBool("isEnabled", hide);
    }

    public void ResetNoAnimator() {
        if (m_noAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_noAnimator.SetBool("isEnabled", false);
        m_noAnimator.SetTrigger("reset");
    }

    //NOTE: finalPercent must be between 0 and 100...
    public void StartPollAPercentCountdown(float countdownTime, int finalPercent) {
        StartCoroutine(RunPollAPercentCountdown(countdownTime, finalPercent));
    }

    //NOTE: finalPercent must be between 0 and 100...
    IEnumerator RunPollAPercentCountdown(float countdownTime, int finalPercent) {
        int timeCount = 0;
        float waitTime = countdownTime / finalPercent;

        while (timeCount < finalPercent) {
            m_pollALargePercent.text = timeCount.ToString() + "%";
            m_pollASmallPercent.text = timeCount.ToString() + "%";
            m_pollAEqualPercent.text = timeCount.ToString() + "%";
            yield return new WaitForSeconds(waitTime);
            ++timeCount;
        }
    }
}
