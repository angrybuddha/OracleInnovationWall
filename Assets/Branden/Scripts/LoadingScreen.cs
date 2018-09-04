using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
    [SerializeField]
    bool m_debug = false;

    float m_loadWaitTime = 6f;

    static LoadingScreen m_instance = null;
    public static LoadingScreen Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<LoadingScreen>();
            }
            return m_instance;
        }
    }

    TMPro.TextMeshProUGUI m_textPro = null;
    CanvasGroup m_canvasGroup = null;

    string m_debugMessage = null;

    // Use this for initialization
    void Awake () {
        Init();
    }

    public void Init() {
        //Already Initialized...
        if (m_textPro != null) {
            return;
        }

        m_textPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_canvasGroup.alpha = 1f;
    }

    public void FinishLoading() {
        PrintMessage("Loading...");
        StartCoroutine(RunFinishLoading());
    }

    public void PrintMessage(string message) {
        if (m_textPro) {
            m_textPro.text = message;
        }
    }

    public void PrintDebugMessage(string message) {
        if (m_textPro) {
            m_debugMessage = message;
        }
    }

    void Update() {
        if (StartupSettings.Instance.ShowDebugLogging) {
            PrintDebugMessage("STATE: " + AppManager.State);
        }
    }

    void OnGUI() {
        if (m_debug) {
            GUIStyle style = new GUIStyle();
            style.fontSize = 48;
            style.normal.textColor = Color.black;

            GUI.Label(new Rect(20, 20, Screen.width, 100),
                m_debugMessage, style);
        }
    }

    public void Show(bool show = true) {
        m_canvasGroup.alpha = show ? 1f : 0f;
    }

    IEnumerator RunFinishLoading() {
        if (Startup.ContentUpdating) {
            //Wait for at least a minute so update doesn't happen again, in startup.cs...
            yield return new WaitForSeconds(60f);
            Startup.ContentUpdating = false;
        }
        else {
            TimeManager.Instance.TimeScale = 100f;  //100 should be enough time for now...
            yield return new WaitForSeconds(100f * m_loadWaitTime);
            TimeManager.Instance.TimeScale = 1f;
            yield return new WaitForSeconds(.5f);
            PlayerManager.Instance.Init();
        }

        Show(false);
    }
}
