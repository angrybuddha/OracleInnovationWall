using UnityEngine;
using System;
using System.Collections;

using MQTTRawPlayer = MQTTListener.RawPlayer;

public class Player : MonoBehaviour {

    [SerializeField]
    int m_id = 0;
    public int Id {
        get { return m_id; }
    }

    [SerializeField]
    float m_moveSpeed = 5f;

    MQTTRawPlayer m_rawPlayer = new MQTTRawPlayer();
    public MQTTRawPlayer RawPlayer {
        get { return m_rawPlayer; }
        set { m_rawPlayer = value; }
    }

    Color m_activeColor = Color.red;
    public Color ActiveColor {
        get { return m_activeColor; }
    }

    Animator m_activeAnimator = null;
    Animator m_inactiveAnimator = null;
    Animator m_countdownAnimator = null;
    Animator m_moveImgAnimator = null;
    Animator m_moveCubeAnimator = null;
    Animator m_rangeArrowAnimator = null;
    Animator m_hashTagAnimator = null;
    Animator m_takePollAnimator = null;
    Animator m_pointToPollAnimator = null;

    Spring m_spring = null;
    public Vector3 Velocity {
        get { return m_spring.Velocity; }
        set { m_spring.Velocity = value; }
    }

    Material m_activeMaterial = null;

    Vector3 m_transitionPos = Vector3.zero;
    TMPro.TextMeshProUGUI m_countdownTextPro = null;

    TMPro.TextMeshProUGUI m_hashtagTextPro = null;

    Coroutine m_startCountdown = null;

    bool m_initialized = false;

    static int m_countdownValue = 0;
    public static int CountdownValue {
        get { return m_countdownValue; }
        set { m_countdownValue = value; }
    }

    static string m_hashtagStr = null;
    public static string HashtagStr {
        get { return m_hashtagStr; }
        set { m_hashtagStr = value; }
    }

    static bool m_showColor = false;
    public static bool ShowColor {
        get { return m_showColor; }
        set { m_showColor = value; }
    }

    static bool m_showInactiveColor = true;
    public static bool ShowInactiveColor {
        get { return m_showInactiveColor; }
        set { m_showInactiveColor = value; }
    }

    static bool m_showMoveIcon = true;
    public static bool ShowMoveIcon {
        get { return m_showMoveIcon; }
        set { m_showMoveIcon = value; }
    }

    static bool m_showTakePollText = false;
    public static bool ShowTakePollText {
        get { return m_showTakePollText; }
        set { m_showTakePollText = value; }
    }

    static bool m_showPointPoll = false;
    public static bool ShowPointPoll {
        get { return m_showPointPoll; }
        set { m_showPointPoll = value; }
    }

    static bool m_showMoveCubeText = false;
    public static bool ShowMoveCubeText {
        get { return m_showMoveCubeText; }
        set { m_showMoveCubeText = value; }
    }

    static bool m_showHashtag = false;
    public static bool ShowHashtag {
        get { return m_showHashtag; }
        set { m_showHashtag = value; }
    }

    static bool m_checkRange = false;
    public static bool CheckRange {
        get { return m_checkRange; }
        set { m_checkRange = value; }
    }

    static bool m_showCountdownValue = false;
    public static bool ShowCountdownValue {
        get { return m_showCountdownValue; }
        set { m_showCountdownValue = value; }
    }

    static int m_idCount = 0;

    public void Awake() {
        if (m_initialized) {
            return;
        }

        m_transitionPos = transform.position;

        m_id = ++m_idCount;
        m_rawPlayer.Id = m_id.ToString();

        m_spring = GetComponent<Spring>();
        Find(transform);

        m_initialized = true;
    }

    public void OnEnable() {
        m_activeAnimator.SendMessage("ChangeAlpha_Event", 0f);
    }

    void Find(Transform t) {
        foreach (Transform child in t) {
            GameObject childObj = child.gameObject;
            string childName = childObj.name;

            if (childName == "Cube - Active") {
                m_activeAnimator = child.GetComponent<Animator>();
                m_activeMaterial = child.GetComponent<MeshRenderer>().material;
            }
            else if (childName == "Cube - Inactive") {
                m_inactiveAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Image - Move") {
                m_moveImgAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - MoveCube") {
                m_moveCubeAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Image - RangeArrow") {
                m_rangeArrowAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - Hashtag") {
                m_hashTagAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Text - Countdown") {
                m_countdownTextPro = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                m_countdownAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Text - Hashtag") {
                m_hashtagTextPro = child.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else if (childName == "Panel - TakePoll") {
                m_takePollAnimator = child.GetComponent<Animator>();
            }
            else if (childName == "Panel - PointToPoll") {
                m_pointToPollAnimator = child.GetComponent<Animator>();
            }

            Find(child);
        }
    }

    void Update() {
        PanelManager panelManager = PanelManager.Instance;
        UpdatePosition();
        UpdatePlayerState();

        if (m_hashtagTextPro != null) {
            m_hashtagTextPro.text = m_hashtagStr;
        }

        if (panelManager != null) {
            panelManager.AddPlayerToPanel(this);
        }
    }

    void UpdatePosition() {
        SetWorldPosition(ref m_transitionPos);

        Vector3 position = transform.position;
        m_spring.AddForce(m_transitionPos, ref position);
        transform.position = position;
    }

    void UpdatePlayerState() {
        ShowActive(m_showColor);
        ShowInactive(ShowInactiveColor);
        HideMoveIcon(!m_showMoveIcon);
        ShowMoveCube(m_showMoveCubeText);
        ShowHashtagImg(m_showHashtag);
        ShowTakePoll(m_showTakePollText);
        ShowPointToPoll(m_showPointPoll && !m_showHashtag);
        ShowCountdown(m_showCountdownValue);
        CheckSetRange();
    }

    public void SetWorldPosition(ref Vector3 pos) {
        if (m_rawPlayer == null) {
            return;
        }

        Camera camera = Camera.main;
        Vector3 viewportPos = camera.WorldToViewportPoint(transform.position);
        viewportPos.x = PlayerManager.Instance.GetScreenPlayerX(m_rawPlayer.X);
        pos = camera.ViewportToWorldPoint(viewportPos);
    }

    public void ShowActive(bool show) {
        if (m_activeAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_activeAnimator.SetBool("isEnabled", show);
    }

    public void ShowInactive(bool show) {
        if (m_inactiveAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_inactiveAnimator.SetBool("isEnabled", show);
    }

    public void HideMoveIcon(bool hide) {
        if (m_moveImgAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_moveImgAnimator.SetBool("isEnabled", hide);
    }

    public void ShowMoveCube(bool show) {
        if (m_moveCubeAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_moveCubeAnimator.SetBool("isEnabled", show);
    }

    public void ShowRangeArrow(bool show) {
        if (m_rangeArrowAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_rangeArrowAnimator.SetBool("isEnabled", show);
    }

    public void ShowHashtagImg(bool show) {
        if (m_rangeArrowAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_hashTagAnimator.SetBool("isEnabled", show);
    }

    public void ShowTakePoll(bool show) {
        if (m_takePollAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_takePollAnimator.SetBool("isEnabled", show);
    }

    public void ShowPointToPoll(bool show) {
        if (m_pointToPollAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_pointToPollAnimator.SetBool("isEnabled", show);
    }

    public void ShowCountdown(bool show) {
        if (m_countdownAnimator == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        m_countdownTextPro.text = m_countdownValue.ToString();
        m_countdownAnimator.SetBool("isEnabled", show);
    }

    //Does not set alpha...
    public void SetActiveColor(Color color) {
        color.a = m_activeMaterial.color.a;
        m_activeMaterial.color = m_activeColor = color;
    }

    public static IEnumerator ShowHashtagOverTime(float time) {
        m_showHashtag = true;
        yield return new WaitForSeconds(time);
        m_showHashtag = false;
    }

    void CheckSetRange() {
        if (m_checkRange) {
            CenterScreen centerScreen = CenterScreen.Instance;
            ShowRangeArrow(centerScreen.InRangeOfRangeBar(
                transform.position));
        }
        else {
            ShowRangeArrow(false);
        }
    }

    public static IEnumerator RunCountdown(Action OnCountdownEnd) {
        int count = m_countdownValue;

        do {
            m_countdownValue = Mathf.Max(0, count);
            yield return new WaitForSeconds(1f);
        } while (count-- > 0);

        if (OnCountdownEnd != null) {
            OnCountdownEnd();
        }
    }
}
