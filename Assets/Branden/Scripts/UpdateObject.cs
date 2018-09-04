using UnityEngine;

[ExecuteInEditMode]
public class UpdateObject : MonoBehaviour {
    [SerializeField]
    bool m_swapMesh = false;

    [SerializeField]
    bool m_swapMaterial = false;

    [SerializeField]
    bool m_addAnimateAlphaComp = false;

    [SerializeField]
    Material m_material = null;

    [SerializeField]
    bool m_addRigidBody = false;

    [SerializeField]
    bool m_parent = false;

    [SerializeField]
    Mesh m_mesh = null;

    // Update is called once per frame
    void Update() {
        if (m_swapMesh) {
            m_swapMesh = false;

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter filter in meshFilters) {
                filter.mesh = m_mesh;
            }
        }

        if (m_addRigidBody) {
            m_addRigidBody = false;

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders) {
                collider.isTrigger = true;
                Rigidbody rigidBody = collider.gameObject.AddComponent<Rigidbody>();
                rigidBody.useGravity = false;
            }
        }

        if (m_parent) {
            m_parent = false;

            Rigidbody[] rigidBody = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidBody) {
                rb.transform.SetParent(transform, true);
                rb.gameObject.name = "Cube";
            }
        }

        if (m_swapMaterial) {
            m_swapMaterial = false;
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer render in renderers) {
                render.sharedMaterial = m_material;
            }
        }

        if (m_addAnimateAlphaComp) {
            m_addAnimateAlphaComp = false;
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer render in renderers) {
                render.gameObject.AddComponent<AnimateAlpha>();
            }
        }
    }
}
