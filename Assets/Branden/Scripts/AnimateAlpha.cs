using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimateAlpha : MonoBehaviour {

    [SerializeField]
    bool m_addSelf = true;

    [SerializeField]
    bool m_addChildren = true;

    [SerializeField]
    List<MeshRenderer> m_renderers = null;

    Animator m_animator = null;

    bool m_init = false;

    void Awake() {
        Init();
    }

    void Init() {
        if (m_init) {
            return;
        }

        var self = GetComponent<MeshRenderer>();
        m_animator = GetComponent<Animator>();

        if (m_addSelf && m_addChildren) {
            m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>(true));
        }
        else if (m_addChildren) {
            m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>(true));
            if(self != null) {
                m_renderers.Remove(self);
            }
        }
        else if(m_addSelf) {
            m_renderers.Add(self);
        }

        m_init = true;
    }

    public void ChangeAlpha_Event(float alpha) {
        Init();
        StopCoroutine("ChangeAlpha");
        foreach (MeshRenderer renderer in m_renderers) {
            Color materialColor = renderer.material.color;
            materialColor.a = alpha;
            renderer.material.color = materialColor;
        }
    }

    public void ChangeAlphaOverTime_Event(float alpha) {
        Init();
        StopCoroutine("ChangeAlpha");
        StartCoroutine(ChangeAlpha(alpha));
    }

    IEnumerator ChangeAlpha(float alpha) {
        float animTime = m_animator.GetCurrentAnimatorStateInfo(0).speed;
        float timer = animTime;
        List<float> startAlphas = new List<float>();
        foreach (MeshRenderer renderer in m_renderers) {
            startAlphas.Add(renderer.material.color.a);
        }

        do {
            timer = Mathf.Max(0f, timer - Time.deltaTime);
            float time = timer / animTime;

            int alphaIndex = 0;
            foreach (MeshRenderer renderer in m_renderers) {
                Color materialColor = renderer.material.color;
                materialColor.a = Mathf.Lerp(alpha, startAlphas[
                    alphaIndex++], time);
                renderer.material.color = materialColor;
            }
            yield return null;
        } while (timer > 0f);
    }
}
