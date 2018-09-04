using UnityEngine;

public class GlobalSettingText : MonoBehaviour {
    enum TextMessage {
        JOIN_CONV,
        JOIN_CONV_SUB,
        GET_STARTED,
        GET_STARTED_SUB,
        MOVE,
        FLAG
    };

    [SerializeField]
    TextMessage m_textMessage = TextMessage.JOIN_CONV;

    static string[] m_messages = new string[] {
        "Join the conversation",
        "Take a poll and see where you stand\n" +
            "among those who visit Oracle.",
        "Let's get started",
        "The colored cube represents your vote.\n" +
            "Move your body to submit your answer.",
        "Move to the right panel",
        "Join the conversation"
    };

    public static string[] Messages {
        get { return m_messages; }
        set { m_messages = value; }
    }

    TMPro.TextMeshProUGUI m_textPro = null;

    // Use this for initialization
    void Start () {
        m_textPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        m_textPro.text = m_messages[(int)m_textMessage];
    }
}
