using UnityEngine;
using System.Collections.Generic;

public class CenterScreen : MonoBehaviour {

    Animator m_animator = null; //Center Screen fades in/out...
    Animator m_headerFadeAnimator = null;
    Animator m_headerMoveAnimator = null;
    Animator m_pollBAnimator = null;
    Animator m_barGraphAnimator = null;
    Animator m_rangeBarAnimator = null;

    RectTransform m_rangeBar = null;
    public RectTransform RangeBar {
        get { return m_rangeBar; }
    }

    //For poll B...
    TMPro.TextMeshProUGUI m_firstLabel = null;
    public TMPro.TextMeshProUGUI FirstLabel {
        get { return m_firstLabel; }
    }

    //For poll B...
    TMPro.TextMeshProUGUI m_lastLabel = null;
    public TMPro.TextMeshProUGUI LastLabel {
        get { return m_lastLabel; }
    }

    //For poll B...
    List<TMPro.TextMeshProUGUI> m_centerLabels = new
        List<TMPro.TextMeshProUGUI>();
    public List<TMPro.TextMeshProUGUI> CenterLabels {
        get { return m_centerLabels; }
    }

    TMPro.TextMeshProUGUI m_headerText = null;
    public TMPro.TextMeshProUGUI HeaderText {
        get { return m_headerText; }
    }

    static CenterScreen m_instance = null;
    public static CenterScreen Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<CenterScreen>();
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

            if (childName == "Panel - Header") {
                m_headerFadeAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - MovingHeader") {
                m_headerMoveAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Text - PollQuestion") {
                m_headerText = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Panel - PollB") {
                m_pollBAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "RawImage - BarGraph") {
                m_barGraphAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - RangeBar") {
                m_rangeBarAnimator = child.GetComponent<Animator>();
                m_rangeBar = (RectTransform)child;
            }
            else if (childName == "Label - First") {
                m_firstLabel = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Label - Last") {
                m_lastLabel = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Label - Center") {
                var textPro = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                m_centerLabels.Add(textPro);
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

    public void ShowPollB(bool show) {
        if (m_pollBAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_pollBAnimator.SetBool("isEnabled", show);
    }

    public void ShowBarGraph(bool show) {
        if (m_barGraphAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_barGraphAnimator.SetBool("isEnabled", show);
    }

    public void HideRangeBar(bool show) {
        if (m_rangeBarAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_rangeBarAnimator.SetBool("isEnabled", show);
    }

    public void ShowHeader(bool show) {
        if (m_headerFadeAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_headerFadeAnimator.SetBool("isEnabled", show);
    }

    public void MoveUpHeader(bool moveUp) {
        if (m_headerMoveAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_headerMoveAnimator.SetBool("isEnabled", moveUp);
    }

    public void ResetHeader() {
        if (m_headerMoveAnimator == null ||
            m_headerMoveAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_headerFadeAnimator.SetBool("isEnabled", false);
        m_headerMoveAnimator.SetBool("isEnabled", false);
        m_headerFadeAnimator.SetTrigger("reset");
        m_headerMoveAnimator.SetTrigger("reset");
    }

    public bool InRangeOfRangeBar(Vector3 worldPos) {
        float range = 0f;   //Ignores range for this function...
        return InRangeOfRangeBar(worldPos, ref range);
    }

    public bool InRangeOfRangeBar(Vector3 worldPos, ref float range) {
        float rangeBarWidth = m_rangeBar.sizeDelta.x;
        float minXPos = (Screen.width - rangeBarWidth)/2f;
        float maxXPos = Screen.width - minXPos;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        float screenPosX = viewportPos.x * Screen.width;

        if(screenPosX < minXPos || screenPosX > maxXPos) {
            return false;   //Not in range...
        }

        range = (screenPosX - minXPos)/rangeBarWidth;

        return true;
    }
}
