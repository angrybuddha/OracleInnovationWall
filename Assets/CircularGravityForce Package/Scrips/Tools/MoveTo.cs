/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 05-20-15
* Last Updated: 06-15-15
* Description: Used for making the current Transform move to a target transform.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour
{
    #region Properties

    [SerializeField]
    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [SerializeField]
    private bool moveRigidbody = false;
    public bool MoveRigidbody
    {
        get { return moveRigidbody; }
        set { moveRigidbody = value; }
    }

    [SerializeField]
    private float lerpSpeed = 8f;
    public float LerpSpeed
    {
        get { return lerpSpeed; }
        set { lerpSpeed = value; }
    }

    [SerializeField]
    private bool lockX = false;
    public bool LockX
    {
        get { return lockX; }
        set { lockX = value; }
    }
    [SerializeField]
    private bool lockY = false;
    public bool LockY
    {
        get { return lockY; }
        set { lockY = value; }
    }
    [SerializeField]
    private bool lockZ = false;
    public bool LockZ
    {
        get { return lockZ; }
        set { lockZ = value; }
    }

    [SerializeField]
    private Vector3 offset = Vector3.zero;
    public Vector3 Offset
    {
        get { return offset; }
        set { offset = value; }
    }

    #endregion

    #region Unity Functions

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector3 newLocation = new Vector3(Target.position.x + Offset.x, Target.position.y + Offset.y, Target.position.z + Offset.z);

            if (LockX)
            {
                newLocation.x = transform.position.x;
            }
            if (LockY)
            {
                newLocation.y = transform.position.y;
            }
            if (LockZ)
            {
                newLocation.z = transform.position.z;
            }

            Vector3 moveTo = Vector3.Lerp(transform.position, newLocation, Time.deltaTime * LerpSpeed);

            if (Target != null)
            {
                if (MoveRigidbody)
                    transform.GetComponent<Rigidbody>().MovePosition(moveTo);
                else
                    transform.position = moveTo;
            }
        }
    }

    #endregion
}
