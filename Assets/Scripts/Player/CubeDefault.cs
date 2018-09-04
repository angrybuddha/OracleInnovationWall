/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for cube votes.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

using CircularGravityForce;

public class CubeDefault : MonoBehaviour
{
    //Cube state
    [SerializeField, Header("Cube State: ")]
    private QuestionManager.QuestionState questionState = QuestionManager.QuestionState.None;
    public QuestionManager.QuestionState QuestionState
    {
        get { return questionState; }
        set { questionState = value; }
    }

    //Stops at target
    [SerializeField]
    private bool stopAtTarget = false;
    public bool StopAtTarget
    {
        get { return stopAtTarget; }
        set { stopAtTarget = value; }
    }

    //Physics force pivot
    [SerializeField, Header("Forces: ")]
    private Transform forcePivot;
    public Transform ForcePivot
    {
        get { return forcePivot; }
        set { forcePivot = value; }
    }

    //Physics force
    [SerializeField]
    private CircularGravity cgfForce;
    public CircularGravity CgfForce
    {
        get { return cgfForce; }
        set { cgfForce = value; }
    }

    //Rotation slerp speed
    [SerializeField]
    private float rotationSlerpSpeed = 4f;
    public float RotationSlerpSpeed
    {
        get { return rotationSlerpSpeed; }
        set { rotationSlerpSpeed = value; }
    }

    //Cube speed
    [SerializeField, Header("Cube Targets Settings:")]
    private float speed = 60f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    //Slowdown speed
    [SerializeField]
    private float slowdownSpeed = 5f;
    public float SlowdownSpeed
    {
        get { return slowdownSpeed; }
        set { slowdownSpeed = value; }
    }

    //Uses slowdown speed when reachs the slowdown target distance
    [SerializeField]
    private float slowdownTargetDistance = 10f;
    public float SlowdownTargetDistance
    {
        get { return slowdownTargetDistance; }
        set { slowdownTargetDistance = value; }
    }

    //The stop target distance
    [SerializeField]
    private float stopTargetDistance = 2f;
    public float StopTargetDistance
    {
        get { return stopTargetDistance; }
        set { stopTargetDistance = value; }
    }

    //Min target distance
    [SerializeField]
    private float minTargetDistance = .5f;
    public float MinTargetDistance
    {
        get { return minTargetDistance; }
        set { minTargetDistance = value; }
    }
    
    //Target
    [SerializeField]
    private Vector3 target;
    public Vector3 Target
    {
        get { return target; }
        set { target = value; }
    }

    private Rigidbody rigid;
    private bool stop = false;

    //If a scatter plot, draw a scatter plot icon in the editor
    void OnDrawGizmos()
    {
        if (QuestionState == QuestionManager.QuestionState.ScatterPlot)
        {
            Gizmos.DrawIcon(Target, "Player - ScatterPoint.png");
        }
    }

    // Use this for initialization
    void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (questionState)
        {
            case QuestionManager.QuestionState.None:

                CgfForce.Enable = false;

                RigidbodyReset();
                
                break;

            case QuestionManager.QuestionState.Poll:
                
                CgfForce.Enable = false;
                
                break;
            case QuestionManager.QuestionState.MultipleChoice:
                
                CgfForce.Enable = false;
                
                break;
            case QuestionManager.QuestionState.ScatterPlot:

                if (!stop)
                {
                    rigid.useGravity = false;

                    if (StopAtTarget == true)
                    {
                        if (StopTargetDistance > Vector3.Distance(this.transform.position, Target))
                        {
                            RigidbodyScatterPlotStop();
                            
                            stop = true;
                        }
                    }

                    if (MinTargetDistance < Vector3.Distance(this.transform.position, Target))
                    {
                        if (SlowdownTargetDistance < Vector3.Distance(this.transform.position, Target))
                        {
                            CgfForce.ForcePower = Speed;
                        }
                        else
                        {
                            CgfForce.ForcePower = SlowdownSpeed;
                        }

                        CgfForce.Enable = true;
                    }
                    else
                    {
                        CgfForce.Enable = false;
                    }

                    PointForceTowardsTarget();
                }

                break;
        }
    }

    //If gameobject disabled
    void OnDisable()
    {
        stop = false;

        RigidbodyReset();

        QuestionState = QuestionManager.QuestionState.None;
    }

    //Point physics force towards target
    void PointForceTowardsTarget()
    {
        var flatVectorToTarget = Vector3.zero;

        flatVectorToTarget = ForcePivot.position - Target;

        var newRotation = Quaternion.LookRotation(flatVectorToTarget);
        var rotation = Quaternion.Slerp(ForcePivot.rotation, newRotation, Time.deltaTime * RotationSlerpSpeed);

        ForcePivot.rotation = rotation;
    }

    //Resets rigidbody settings
    void RigidbodyReset()
    {
        rigid.useGravity = true;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.mass = 1.0f;
        rigid.drag = 2.0f;
        rigid.angularDrag = .5f;
    }

    //Rigidbody Scatter Plot Stop
    void RigidbodyScatterPlotStop()
    {
        rigid.useGravity = true;
        rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rigid.drag = 0f;
        rigid.angularDrag = 0.05f;
        rigid.velocity = new Vector3(0, 0, 0);
        rigid.angularVelocity = Vector3.zero;
        Physics.IgnoreCollision(this.GetComponent<Collider>(), Core.Instance.Floor.GetComponent<Collider>(), false);
    }
}