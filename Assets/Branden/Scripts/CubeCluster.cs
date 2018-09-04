using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeCluster : MonoBehaviour {
    [SerializeField]
    GameObject m_cubePrefab = null;

    List<Player> m_filteredPlayers = new List<Player>();
    public List<Player> FilteredPlayers {
        get { return m_filteredPlayers; }
    }

    List<Animator> m_playerCubes = new List<Animator>();

    [SerializeField]
    float m_explosiveRadius = 5f;

    [SerializeField]
    float m_explosivePower = 1000f;

    [SerializeField]
    float m_destroyAfterExplodeTime = 8f;

    Animator m_animator = null;

    GameObject m_cluster = null;

    Rigidbody[] m_rigidBodies = null;

    static bool m_hidePlayerCubes = false;
    public static bool HidePlayerCubes {
        get { return m_hidePlayerCubes; }
        set { m_hidePlayerCubes = value; }
    }

    void Update() {
        ShowPlayerCubes();
    }

    public void Find(Transform parent, bool searchDeep = true) {
        foreach (Transform child in parent) {
            GameObject childObj = child.gameObject;
            string childName = childObj.name;
            if (childName.Contains("Group - Animated")) {
                Find(child, false);
                return; //HACK: For now, let's not look further...
            }
            else if (childName.Contains("Cube - Player")) {
                m_playerCubes.Add(child.GetComponent<Animator>());
            }

            if (searchDeep) {
                Find(child, true);
            }
        }
    }

    public void CreateCluster() {
        m_cubePrefab.SetActive(false);

        if (m_cluster != null) {
            Debug.LogWarning("Must Destroy the existing" +
                " cluster before making a new one...");
            return;
        }

        m_cluster = (GameObject)Instantiate(m_cubePrefab,
            m_cubePrefab.transform.parent, false);

        m_animator = m_cluster.GetComponent<Animator>();
        Find(m_cluster.transform);

        SetPlayerCubeColors();
        m_cluster.SetActive(true);
        m_rigidBodies = m_cluster.GetComponentsInChildren<Rigidbody>();
    }

    public void DestroyCluster() {
        if (m_cluster != null) {
            StopAllCoroutines();    //In-case DestroyAfterExplode is running...

            Destroy(m_cluster);

            m_playerCubes.Clear();
            m_animator = null;
            m_cluster = null;
            m_rigidBodies = null;
            return;
        }

        Debug.LogWarning("Cannot Destroy a NULL Cube Cluster...");
    }

    public void Explode() {
        if (m_cluster == null) {
            Debug.LogWarning("Cannot Explode a NULL Cube Cluster...");
        }

        Vector3 origin = m_cluster.transform.position;
        foreach (Rigidbody rb in m_rigidBodies) {
            MeshRenderer meshRenderer = rb.GetComponent<MeshRenderer>();
            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            rb.GetComponent<MeshRenderer>().receiveShadows = false;
            rb.AddExplosionForce(m_explosivePower, origin, m_explosiveRadius);
        }
    }

    //Slides in cube cluster...
    public void ShowCluster(bool show) {
        if (m_animator == null) {
            Debug.LogWarning("Cannot Show a NULL Cube Cluster...");
            return;
        }

        m_animator.SetBool("isEnabled", show);
    }

    //Slides in cube cluster...
    public void InstantShowCluster() {
        if (m_animator == null) {
            Debug.LogWarning("Cannot Show a NULL Cube Cluster...");
            return;
        }

        m_animator.SetBool("isEnabled", true);
        m_animator.SetTrigger("instant");
    }

    public void SetPlayerCubeColors() {
        if (m_playerCubes.Count > 0) {
            var playerColorList = PlayerManager.Instance.PlayerColorList;
            for (int i = 0, count = m_playerCubes.Count; i < count; ++i) {
                MeshRenderer rend = m_playerCubes[i].GetComponent<MeshRenderer>();
                Color color = playerColorList[i].color;
                color.a = rend.material.color.a;
                rend.material.color = color;
            }
        }
    }

    public virtual void ShowPlayerCubes() {
        var activePlayers = PlayerManager.Instance.ActivePlayers;

        for (int i = 0, count = m_playerCubes.Count; i < count; ++i) {
            bool isVisible = false;

            foreach (Player player in activePlayers) {
                if (player.Id == i + 1) {
                    isVisible = !m_filteredPlayers.Contains(player);
                    break;
                }
            }

            isVisible = isVisible && !m_hidePlayerCubes;
            m_playerCubes[i].SetBool("isEnabled", isVisible);
        }
    }

    IEnumerator DestroyAfterExplode() {
        yield return new WaitForSeconds(m_destroyAfterExplodeTime);
        DestroyCluster();
    }
}
