/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf trigger, used for enabling or disabling based of if the raycast
*              is tripped in 2D.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
    public class CGF_EnableTrigger2D : MonoBehaviour
    {
        #region Properties

        //CircularGravity used for the enable trigger 
        [SerializeField]
        private CircularGravity2D cgf;
        public CircularGravity2D Cgf
        {
            get { return cgf; }
            set { cgf = value; }
        }

        //Value when tripped
        [SerializeField]
        private bool tripValue = true;
        public bool TripValue
        {
            get { return tripValue; }
            set { tripValue = value; }
        }

        //Trip distance
        [SerializeField]
        private float maxTripDistance = 10f;
        public float MaxTripDistance
        {
            get { return maxTripDistance; }
            set { maxTripDistance = value; }
        }

        private float gizmoSize = .25f;

        #endregion

        #region Gizmos

        void OnDrawGizmos()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.right);

            RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, fwd, maxTripDistance);

            if (cgf != null)
            {
                gizmoSize = (cgf.Size / 8f);
                if (gizmoSize > .25f)
                    gizmoSize = .25f;
                else if (gizmoSize < -.25f)
                    gizmoSize = -.25f;
            }

            Color activeColor = Color.cyan;
            Color nonActiveColor = Color.white;

            Gizmos.DrawSphere(this.transform.position, gizmoSize);

            if (hitInfo.transform == null)
            {
                Gizmos.color = nonActiveColor;
                Gizmos.DrawLine(this.transform.position, this.transform.position + (fwd * maxTripDistance));
                return;
            }

            if (Vector2.Distance(this.transform.position, hitInfo.point) > maxTripDistance) 
			{
                Gizmos.color = nonActiveColor;
				Gizmos.DrawLine (this.transform.position, hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
				return;
			} 
			else if (hitInfo.transform != null) 
			{
                Gizmos.color = activeColor;
				Gizmos.DrawLine (this.transform.position, hitInfo.point);
                Gizmos.DrawSphere(this.transform.position, gizmoSize);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
			}
		}

        #endregion

        #region Unity Functions

        void Start()
        {
            cgf.Enable = !TripValue;
        }

        void Update()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.right);

            RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, fwd, maxTripDistance);

            if (Vector2.Distance(this.transform.position, hitInfo.point) > maxTripDistance)
            {
                cgf.Enable = !TripValue;
                return;
            }

			if (hitInfo.transform != null) 
			{
                cgf.Enable = TripValue;
			}
            else
            {
                cgf.Enable = !TripValue;
            }
        }

        #endregion
    }
}
