/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for making the current Transform look at a target transform.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
	public class LookTo : MonoBehaviour
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
        private bool rotationRigidbody = false;
        public bool RotationRigidbody
        {
            get { return rotationRigidbody; }
            set { rotationRigidbody = value; }
        }

		[SerializeField]
		private float slerpSpeed = 8f;
		public float SlerpSpeed
		{
			get { return slerpSpeed; }
			set { slerpSpeed = value; }
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

        #endregion

        #region Unity Functions

        // Update is called once per frame
	    void FixedUpdate()
	    {
			if(Target != null)
			{
				var flatVectorToTarget = transform.position - Target.position;

				if(LockX)
				{
					flatVectorToTarget.x = 0;
				}
				if(LockY)
				{
					flatVectorToTarget.y = 0;
				}
				if(LockZ)
				{
					flatVectorToTarget.z = 0;
				}

				var newRotation = Quaternion.LookRotation(flatVectorToTarget);
				var rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * SlerpSpeed);

                if (RotationRigidbody)
                    transform.GetComponent<Rigidbody>().MoveRotation(rotation);
                else
                    transform.rotation = rotation;
			}
        }

        #endregion
    }
}