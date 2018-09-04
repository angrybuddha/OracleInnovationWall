using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class RenderFog : MonoBehaviour {
    [SerializeField]
    bool m_render = true;

    bool m_revertFogState = false;

    void OnPreRender() {
        m_revertFogState = RenderSettings.fog;
        RenderSettings.fog = m_render;
    }

    void OnPostRender() {
        RenderSettings.fog = m_revertFogState;
    }
}
