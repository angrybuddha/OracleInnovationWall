/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for locking the current Transform.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
	public class LockTo : MonoBehaviour
    {
        #region Properties

        [SerializeField]
	    private bool lockTransform = true;
	    public bool LockTransform
	    {
	        get { return lockTransform; }
	        set { lockTransform = value; }
	    }

	    private Quaternion initialRotation;

        #endregion

        #region Unity Functions

        // Use this for initialization
		void Start () 
	    {
	        initialRotation = transform.rotation;
		}
		
		// Update is called once per frame
		void Update () 
	    {
	        if (LockTransform)
	        {
	            transform.rotation = initialRotation;
	        }
        }

        #endregion
    }
}