using UnityEngine;
using System.Collections;

public class BarGraph : MonoBehaviour {
    Animator m_animControl = null;
    Animator[] m_cubeAnimators = null;

    Coroutine m_runningAnimateTable = null;

    Transform m_columns = null;
    GameObject m_shadow = null;

    static BarGraph m_instance = null;
    public static BarGraph Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<BarGraph>();
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

            if (childName == "Columns") {
                m_columns = child;
                m_cubeAnimators = child.GetComponentsInChildren<Animator>();
            }
            else if (childName == "AnimControl") {
                m_animControl = child.GetComponent<Animator>();
            }
            else if (childName == "Sprite - Shadow") {
                m_shadow = childObj;
                m_shadow.SetActive(false);
            }

            Find(child);
        }
    }

    public void SetNumberRows(int columnIndex, int numRows) {
        Transform column = m_columns.GetChild(columnIndex);
        for (int i = 0, count = column.childCount; i < count; ++i) {
            GameObject obj = column.GetChild(i).gameObject;
            obj.GetComponent<MeshRenderer>().material.color = Color.white;
            obj.SetActive(i < numRows);
        }
    }

    //Note: Leaves cube in active state it was...
    public GameObject AddCubeToRow(int columnIndex) {
        Transform column = m_columns.GetChild(columnIndex);
        int rowIndex = column.childCount - 1;

        for (int i = 0, count = column.childCount; i < count; ++i) {
            if (!column.GetChild(i).gameObject.activeSelf) {
                rowIndex = i;
                break;
            }
        }

        GameObject cube = column.GetChild(rowIndex).gameObject;
        return cube;
    }

    public void ShowCube(GameObject cube, Color cubeColor) {
        Material cubeMaterial = cube.GetComponent<MeshRenderer>().material;
        Animator cubeAnimator = cube.GetComponent<Animator>();

        cubeMaterial.color = cubeColor;
        cube.SetActive(true);

        //cubeAnimator.SetTrigger("resetOther");
        cubeAnimator.SetBool("isEnabled", true);
    }

    //TODO: Test Code... Remove later.
    [ContextMenu("Simulate Show Table")]
    public void ShowTable() {
        SetNumberRows(0, 9);
        SetNumberRows(1, 9);
        SetNumberRows(2, 10);
        SetNumberRows(3, 10);
        SetNumberRows(4, 6);
        SetNumberRows(5, 3);
        SetNumberRows(6, 3);
        SetNumberRows(7, 3);
        SetNumberRows(8, 4);
        SetNumberRows(9, 4);
        SetNumberRows(10, 6);
        SetNumberRows(11, 2);
        SetNumberRows(12, 2);
        SetNumberRows(13, 2);
        SetNumberRows(14, 4);
        SetNumberRows(15, 2);
        SetNumberRows(16, 2);
        SetNumberRows(17, 2);
        SetNumberRows(18, 4);
        SetNumberRows(19, 4);
        SetNumberRows(20, 4);
        SetNumberRows(21, 5);
        SetNumberRows(22, 3);
        SetNumberRows(23, 3);
        SetNumberRows(24, 3);
        SetNumberRows(25, 3);
        SetNumberRows(26, 4);
        SetNumberRows(27, 7);
        SetNumberRows(28, 7);
        SetNumberRows(29, 4);
        SetNumberRows(30, 4);
        SetNumberRows(31, 4);
        SetNumberRows(32, 5);
        SetNumberRows(33, 8);
        SetNumberRows(34, 8);
        SetNumberRows(35, 10);
        SetNumberRows(36, 9);
        SetNumberRows(37, 7);
        SetNumberRows(38, 6);
        SetNumberRows(39, 6);

        ShowTable(true);

        StartCoroutine(TestSpawnRedCube());
    }

    //TODO: Test Code... Remove later.
    IEnumerator TestSpawnRedCube() {
        yield return new WaitForSeconds(3f);

        GameObject cube = AddCubeToRow(13);
        ShowCube(cube, Color.red);
    }


    [ContextMenu("Simulate Hide Table")]
    public void HideTable() {
        ShowTable(false);
    }

    public void ShowTable(bool show) {
        if (m_animControl == null) {
            Debug.LogWarning("Animator has not been assigned yet!");
            return;
        }

        if (m_runningAnimateTable != null) {
            StopCoroutine(m_runningAnimateTable);
        }
        
        m_runningAnimateTable = StartCoroutine(AnimateTable(show));
    }

    IEnumerator AnimateTable(bool show) {
        //Each animation is 1 second each. 2 should give enough time...
        float totalAnimTime = 2f;
        m_animControl.SetBool("isEnabled", show);

        if (show) {
            m_shadow.SetActive(true);
        }

        while (totalAnimTime > 0f) {
            Transform animCntrlTrans = m_animControl.transform;

            Vector3 planePos = animCntrlTrans.position;
            Vector3 planeNormal = animCntrlTrans.forward;
            Vector3 point = Vector3.zero;
            Vector3 planeToPoint = Vector3.zero;

            foreach (Animator animator in m_cubeAnimators) {
                if (animator.gameObject.activeInHierarchy) {
                    point = animator.transform.position;
                    planeToPoint = planePos - point;
                    float threshold = Vector3.Dot(planeNormal, planeToPoint);

                    bool showCube = threshold > 0f;
                    animator.SetBool("isEnabled", showCube);
                }
            }

            //TODO: Commented out, but keep around...
            ////Renders plane...
            //Vector3 topLeft = planePos - (animCntrlTrans.right * 20f) +
            //    (animCntrlTrans.up * 1f);
            //Vector3 topRight = planePos + (animCntrlTrans.right * 20f) +
            //    (animCntrlTrans.up * 1f);
            //Vector3 bottomLeft = planePos - (animCntrlTrans.right * 20f) -
            //    (animCntrlTrans.up * 1f);
            //Vector3 bottomRight = planePos + (animCntrlTrans.right * 20f) -
            //    (animCntrlTrans.up * 1f);
            //
            //Color lineColor = Color.red;
            //Debug.DrawLine(topLeft, topRight, lineColor);
            //Debug.DrawLine(topRight, bottomRight, lineColor);
            //Debug.DrawLine(bottomRight, bottomLeft, lineColor);
            //Debug.DrawLine(bottomLeft, topLeft, lineColor);

            totalAnimTime -= Time.deltaTime;
            yield return null;
        }

        if (!show) {
            m_shadow.SetActive(false);
        }
    }
}
