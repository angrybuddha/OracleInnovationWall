using UnityEngine;
using System.Collections;

public class CenterMenu : MonoBehaviour {
    Animator m_animator = null;
    Animator m_pollAAnimator = null;
    Animator m_yesAnimator = null;          //MOves "yes" text up and down...
    Animator m_activeYesAnimator = null;    //Shows active "yes" text when activated...
    Animator m_inactiveYesAnimator = null;  //Shows inactive "yes" text when inactivated...
    Animator m_largeResultsAnimator = null; //Shows large cube cluster and results text...
    Animator m_smallResultsAnimator = null; //Shows small cube cluster and results text...
    Animator m_equalResultsAnimator = null; //Shows equal cube cluster and results text...
    Animator m_largePercentAnimator = null; //Shows equal cube cluster and results text...
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

    static CenterMenu m_instance = null;
    public static CenterMenu Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<CenterMenu>();
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
            else if (childName == "Panel - Yes") {
                m_yesAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - ActiveYes") {
                m_activeYesAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - InactiveYes") {
                m_inactiveYesAnimator = child.GetComponent<Animator>();
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
            else if (childName == "Text - ActiveYes") {
                m_activePollAAnswer = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Text - InactiveYes") {
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

    public void MoveDownYes(bool moveDown) {
        if (m_yesAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_yesAnimator.SetBool("isEnabled", moveDown);
    }

    public void ShowActiveYes(bool show) {
        if (m_activeYesAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_activeYesAnimator.SetBool("isEnabled", show);
    }

    public void HideInactiveYes(bool hide) {
        if (m_inactiveYesAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_inactiveYesAnimator.SetBool("isEnabled", hide);
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

    public void ResetYesAnimator() {
        if (m_yesAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_yesAnimator.SetBool("isEnabled", false);
        m_yesAnimator.SetTrigger("reset");
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
