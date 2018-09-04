/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for player cubes.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using CircularGravityForce;


public class Cube : MonoBehaviour
{
    #region Properties

    //Force pivot 
    [SerializeField, Header("Forces: ")]
    private Transform forcePivot;
    public Transform ForcePivot
    {
        get { return forcePivot; }
        set { forcePivot = value; }
    }

    //Torque pivot
    [SerializeField]
    private Transform torquePivot;
    public Transform TorquePivot
    {
        get { return torquePivot; }
        set { torquePivot = value; }
    }

    //Physics used for force 
    [SerializeField]
    private CircularGravity cgfForce;
    public CircularGravity CgfForce
    {
        get { return cgfForce; }
        set { cgfForce = value; }
    }

    //Physics used for torque 
    [SerializeField]
    private CircularGravity cgfTorque;
    public CircularGravity CgfTorque
    {
        get { return cgfTorque; }
        set { cgfTorque = value; }
    }

    //Physics used for hovering 
    [SerializeField]
    private CircularGravity cgfHover;
    public CircularGravity CgfHover
    {
        get { return cgfHover; }
        set { cgfHover = value; }
    }

    //Rotation slerp speed
    [SerializeField]
    private float rotationSlerpSpeed = 4f;
    public float RotationSlerpSpeed
    {
        get { return rotationSlerpSpeed; }
        set { rotationSlerpSpeed = value; }
    }

    //Turns off force when gets to a min distance
    [SerializeField, Header("Cube Targets:")]
    private float minTargetDistance = .5f;
    public float MinTargetDistance
    {
        get { return minTargetDistance; }
        set { minTargetDistance = value; }
    }

    //Player cubes target
    [SerializeField]
    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    #endregion

    #region Unity Functions

    //Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            
            ManageForces();

            PointForceTowardsTarget();

            if (Core.Instance._questionManager._voteState == QuestionManager.VoteState.Voting)
            {
                this.GetComponent<Rigidbody>().freezeRotation = true;
                
                LookAt();
            }
            else
            {
                this.GetComponent<Rigidbody>().freezeRotation = false;

                CgfTorque.Enable = true;
            }
        }
        else
        {
            this.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }

    //Manages the physics forces 
    void ManageForces()
    {
        CgfTorque.ForcePower = .5f;
        CgfTorque._forceTypeProperties.TorqueMaxAngularVelocity = 1f;

        if (MinTargetDistance < Vector3.Distance(this.transform.position, Target.position))
            CgfForce.Enable = true;
        else
            CgfForce.Enable = false;
    }

    //Points force towards target 
    void PointForceTowardsTarget()
    {
        var flatVectorToTarget = Vector3.zero;

        flatVectorToTarget = ForcePivot.position - Target.position;

        var newRotation = Quaternion.LookRotation(flatVectorToTarget);
        var rotation = Quaternion.Slerp(ForcePivot.rotation, newRotation, Time.deltaTime * RotationSlerpSpeed);

        ForcePivot.rotation = rotation;
    }

    //Looks at the target
    void LookAt()
    {
        if (Target != null)
        {
            var flatVectorToTarget = transform.position - Target.gameObject.GetComponent<Hand>().Target.transform.position;

            var newRotation = Quaternion.LookRotation(flatVectorToTarget);
            var rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 1f);

            transform.rotation = rotation;
        }
    }

    #endregion
}
