/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04/15/13
* Last Updated: 06-15-15
* Description: Allows you to rotate around a given transform.
*******************************************************************************************/
using System.Collections;
using UnityEngine;

namespace CircularGravityForce
{
	public class RotateAround : MonoBehaviour
    {
        #region Properties
        
        //Rotates around this GameObject 
	    [SerializeField]
	    private Transform rotateAroundObject;
	    public Transform RotateAroundObject
	    {
	        get { return rotateAroundObject; }
	        set { rotateAroundObject = value; }
	    }

	    //Speed of rotation
	    [SerializeField]
	    private float speed = 10f;
	    public float Speed
	    {
	        get { return speed; }
	        set { speed = value; }
	    }

	    //Axis of the rotation
	    public Vector3 axis = Vector3.up;
	    public Vector3 Axis
	    {
	        get { return axis; }
	        set { axis = value; }
	    }

        #endregion

        #region Unity Functions

        //Update is called once per frame
	    void Update()
	    {
	        this.transform.RotateAround(RotateAroundObject.position, Axis, Speed * Time.deltaTime);
        }

        #endregion
    }
}