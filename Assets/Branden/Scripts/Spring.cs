using UnityEngine;

public class Spring : MonoBehaviour {

    [SerializeField, Header("Constant used in spring force")]
    float m_k = 2f;    //Constant...
    public float K {
        get { return m_k; }
        set { m_k = value; }
    }

    [SerializeField, Header("Constant used in damper force")]
    float m_b = 0.2f;    //Damper...
    public float B {
        get { return m_b; }
        set { m_b = value; }
    }

    [SerializeField]
    Transform m_restPoint = null;

    Vector3 m_velocity = Vector3.zero;
    public Vector3 Velocity {
        get { return m_velocity; }
        set { m_velocity = value; }
    }

    // Update is called once per frame
    void Update() {
        if (m_restPoint == null) {
            return;
        }

        Vector3 position = transform.position;
        AddForce(m_restPoint.position, ref position);
        transform.position = position;
    }

    public void AddForce(Vector3 destination, ref Vector3 position) {
        Vector3 changeInDistDir = (position - destination) * Time.deltaTime;
        m_velocity += changeInDistDir;

        //Fspring = -kΔx, Fdamp = bV, Force = Fspring - Fdamp
        Vector3 springForce = -m_k * changeInDistDir;
        Vector3 dampedForce = m_b * m_velocity;
        Vector3 force = springForce - dampedForce;

        position += force;
    }
}
