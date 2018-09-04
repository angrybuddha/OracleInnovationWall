using UnityEngine;
using UnityEngine.UI;

public class Bezel : MonoBehaviour {
    LayoutElement m_layoutElement = null;
    Image m_image = null;

    static int m_numBezels = 3;
    public static int NumBezels {
        get { return m_numBezels; }
        set { m_numBezels = value; }
    }

    static bool m_show = true;
    public static bool Show {
        get { return m_show; }
        set { m_show = value; }
    }

    //In editor, bezel width should be 24...
    static float m_width = 0f;
    public static float Width {
        get { return m_width; }
        set { m_width = value; }
    }

    void Awake() {
        m_layoutElement = GetComponent<LayoutElement>();
        m_image = GetComponentInChildren<Image>();
    }

    // Use this for initialization
    void Start() {
        if (m_image != null) {
            m_image.enabled = m_show;
        }

        m_layoutElement.preferredWidth = m_width;
    }
}