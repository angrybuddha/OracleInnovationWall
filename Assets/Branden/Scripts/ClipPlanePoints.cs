using UnityEngine;

public class ClipPlanePoints : MonoBehaviour {

    [SerializeField, Header("If null, main camera is used...")]
    Camera m_camera = null;
    public Camera Camera {
        get { return m_camera; }
    }

    [SerializeField]
    float m_boundsDistOffset = 0f;
    public float BoundsDistOffset {
        get { return m_boundsDistOffset; }
        set { m_boundsDistOffset = value; }
    }

    [SerializeField]
    bool m_showDebugLines = true;

    Vector3 m_farUpperLeft;
    public Vector3 FarUpperLeft {
        get { return m_farUpperLeft; }
    }

    Vector3 m_farUpperRight;
    public Vector3 FarUpperRight {
        get { return m_farUpperRight; }
    }

    Vector3 m_farLowerLeft;
    public Vector3 FarLowerLeft {
        get { return m_farLowerLeft; }
    }

    Vector3 m_farLowerRight;
    public Vector3 FarLowerRight {
        get { return m_farLowerRight; }
    }

    Vector3 m_nearUpperLeft;
    public Vector3 NearUpperLeft {
        get { return m_nearUpperLeft; }
    }

    Vector3 m_nearUpperRight;
    public Vector3 NearUpperRight {
        get { return m_nearUpperRight; }
    }

    Vector3 m_nearLowerLeft;
    public Vector3 NearLowerLeft {
        get { return m_nearLowerLeft; }
    }

    Vector3 m_nearLowerRight;
    public Vector3 NearLowerRight {
        get { return m_nearLowerRight; }
    }

    static ClipPlanePoints m_instance = null;
    public static ClipPlanePoints Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<ClipPlanePoints>();
            }
            return m_instance;
        }
    }

    //// Update is called once per frame TODO: keep around, but don't use now...
    //void Update() {
    //    UpdateCameraClipPlanePoints();

    //    if (IsOutsideBounds(trans.position)) {
    //        Debug.LogWarning("Colliding Left...");
    //    }
    //}

    public void UpdateCameraClipPlanePoints() {
        if (m_camera == null) {
            m_camera = Camera.main;
        }

        float rightAngle = m_camera.fieldOfView / 2f;
        float distance = m_camera.farClipPlane;

        float halfHeight = Mathf.Tan(Mathf.Deg2Rad * rightAngle) * distance;
        float halfWidth = halfHeight * m_camera.aspect;

        Transform camTrans = m_camera.transform;
        Vector3 origin = camTrans.position + (camTrans.forward * distance);

        //far...
        m_farUpperLeft = origin -
            (camTrans.right * halfWidth) + (camTrans.up * halfHeight);
        m_farUpperRight = origin +
            (camTrans.right * halfWidth) + (camTrans.up * halfHeight);
        m_farLowerLeft = origin -
            (camTrans.right * halfWidth) - (camTrans.up * halfHeight);
        m_farLowerRight = origin +
            (camTrans.right * halfWidth) - (camTrans.up * halfHeight);

        distance = m_camera.nearClipPlane;
        halfHeight = Mathf.Tan(Mathf.Deg2Rad * rightAngle) * distance;
        halfWidth = halfHeight * m_camera.aspect;
        origin = camTrans.position + (camTrans.forward * distance);

        //near...
        m_nearUpperLeft = origin -
            (camTrans.right * halfWidth) + (camTrans.up * halfHeight);
        m_nearUpperRight = origin +
            (camTrans.right * halfWidth) + (camTrans.up * halfHeight);
        m_nearLowerLeft = origin -
            (camTrans.right * halfWidth) - (camTrans.up * halfHeight);
        m_nearLowerRight = origin +
            (camTrans.right * halfWidth) - (camTrans.up * halfHeight);

        ShowDebug();
    }

    void ShowDebug() {
        if (m_showDebugLines) {
            //front..
            Debug.DrawLine(m_farUpperLeft, m_farUpperRight, Color.red);
            Debug.DrawLine(m_farUpperLeft, m_farLowerLeft, Color.red);
            Debug.DrawLine(m_farLowerLeft, m_farLowerRight, Color.red);
            Debug.DrawLine(m_farLowerRight, m_farUpperRight, Color.red);

            //left...
            Debug.DrawLine(m_farUpperLeft, m_nearUpperLeft, Color.red);
            Debug.DrawLine(m_nearUpperLeft, m_nearLowerLeft, Color.red);
            Debug.DrawLine(m_nearLowerLeft, m_farLowerLeft, Color.red);

            //right...
            Debug.DrawLine(m_farUpperRight, m_nearUpperRight, Color.red);
            Debug.DrawLine(m_nearUpperRight, m_nearLowerRight, Color.red);
            Debug.DrawLine(m_nearLowerRight, m_farLowerRight, Color.red);

            //back...
            Debug.DrawLine(m_nearUpperLeft, m_nearUpperRight, Color.red);
            Debug.DrawLine(m_nearLowerLeft, m_nearLowerRight, Color.red);
        }
    }

    //Is outside camera's view frustrum...
    public bool IsOutsideBounds(Vector3 point) {
        return IsOutsideBounds(point, m_boundsDistOffset);
    }

    public bool IsOutsideBounds(Vector3 point, float boundsOffset) {
        bool isOutsideBounds = CollidesFront(point, boundsOffset) ||
            CollidesBack(point, boundsOffset) || CollidesTop(point, boundsOffset) ||
            CollidesBottom(point, boundsOffset) || CollidesLeft(point, boundsOffset) ||
            CollidesRight(point, boundsOffset);

        return isOutsideBounds;
    }

    public bool CollidesFront(Vector3 point) {
        return CollidesFront(point, m_boundsDistOffset);
    }

    public bool CollidesBack(Vector3 point) {
        return CollidesBack(point, m_boundsDistOffset);
    }

    public bool CollidesTop(Vector3 point) {
        return CollidesTop(point, m_boundsDistOffset);
    }

    public bool CollidesBottom(Vector3 point) {
        return CollidesBottom(point, m_boundsDistOffset);
    }

    public bool CollidesLeft(Vector3 point) {
        return CollidesLeft(point, m_boundsDistOffset);
    }

    public bool CollidesRight(Vector3 point) {
        return CollidesRight(point, m_boundsDistOffset);
    }

    public bool CollidesFront(Vector3 point, float boundsOffset) {
        Vector3 up = m_farLowerLeft - m_farUpperLeft;
        Vector3 right = m_farLowerLeft - m_farLowerRight;
        Vector3 normal = Vector3.Cross(up, right).normalized;

        Vector3 origin = m_farLowerLeft - (normal * boundsOffset);

        Vector3 dir = (point - origin).normalized;
        float threshold = Vector3.Dot(normal, dir);

        //if true collides...
        return threshold < 0f;
    }

    public bool CollidesBack(Vector3 point, float boundsOffset) {
        Vector3 up = m_nearLowerLeft - m_nearUpperLeft;
        Vector3 right = m_nearLowerLeft - m_nearLowerRight;
        Vector3 normal = Vector3.Cross(right, up).normalized;

        Vector3 origin = m_nearLowerLeft - (normal * boundsOffset);

        Vector3 dir = (point - origin).normalized;
        float threshold = Vector3.Dot(normal, dir);

        //if true collides...
        return threshold < 0f;
    }

    public bool CollidesTop(Vector3 point, float boundsOffset) {
        Vector3 up = m_nearUpperLeft - m_farUpperLeft;
        Vector3 right = m_nearUpperLeft - m_nearUpperRight;
        Vector3 normal = Vector3.Cross(right, up).normalized;

        Vector3 origin = m_nearUpperLeft - (normal * boundsOffset);

        Vector3 dir = (point - origin).normalized;
        float threshold = Vector3.Dot(normal, dir);

        //if true collides...
        return threshold < 0f;
    }

    public bool CollidesBottom(Vector3 point, float boundsOffset) {
        Vector3 up = m_nearLowerLeft - m_farLowerLeft;
        Vector3 right = m_nearLowerLeft - m_nearLowerRight;
        Vector3 normal = Vector3.Cross(up, right).normalized;

        Vector3 origin = m_nearLowerLeft - (normal * boundsOffset);

        Vector3 dir = (point - origin).normalized;
        float threshold = Vector3.Dot(normal, dir);
        
        //if true collides...
        return threshold < 0f;
    }

    public bool CollidesLeft(Vector3 point, float boundsOffset) {
        Vector3 up = m_farLowerLeft - m_farUpperLeft;
        Vector3 right = m_farLowerLeft - m_nearLowerLeft;
        Vector3 normal = Vector3.Cross(right, up).normalized;

        Vector3 origin = m_farLowerLeft - (normal * boundsOffset);

        Vector3 dir = (point - origin).normalized;
        float threshold = Vector3.Dot(normal, dir);

        //if true collides...
        return threshold < 0f;
    }

    public bool CollidesRight(Vector3 point, float boundsOffset) {
        Vector3 up = m_farLowerRight - m_farUpperRight;
        Vector3 right = m_farLowerRight - m_nearLowerRight;
        Vector3 normal = Vector3.Cross(up, right).normalized;

        Vector3 origin = m_farLowerRight - (normal * boundsOffset);

        Vector3 dir = (point - origin).normalized;
        float threshold = Vector3.Dot(normal, dir);

        //if true collides...
        return threshold < 0f;
    }
}
