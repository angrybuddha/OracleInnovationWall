using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AppRefresh : MonoBehaviour {

    [SerializeField, Header("In seconds...")]
    float m_refreshTime = 60f;

    [SerializeField]
    int m_sceneIndexToLoad = 0;

    [SerializeField]
    string m_messageText = "Please wait while the app refreshes\n" +
        "and pulls in new content";

    TMPro.TextMeshProUGUI m_message = null;

	// Use this for initialization
	void Awake () {
        m_message = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    void Start() {
        StartCoroutine(RunRefresh());
    }

    IEnumerator RunRefresh() {
        float timer = m_refreshTime;
        int dotCount = 0;

        while (timer > 0f) {
            dotCount = (dotCount + 1) % 3;

            m_message.text = m_messageText;
            for (int i = 0, count = dotCount + 1; i < count; ++i) {
                m_message.text += ".";
            }

            timer -= 1f;
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene(m_sceneIndexToLoad);
    }
}
