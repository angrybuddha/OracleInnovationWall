/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf trigger, used for enabling or disabling based of if the raycast
*              is tripped.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
    public class CGF_EnableTrigger : MonoBehaviour
    {
        #region Properties

        //CircularGravity used for the enable trigger 
        [SerializeField]
        private CircularGravity cgf;
        public CircularGravity Cgf
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
            Vector3 fwd = this.transform.TransformDirection(Vector3.forward);

            RaycastHit hitInfo;

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

            if (Physics.Raycast(this.transform.position, fwd, out hitInfo, maxTripDistance))
            {
                if (hitInfo.distance > maxTripDistance)
                {
                    Gizmos.color = nonActiveColor;
                    Gizmos.DrawLine(this.transform.position, hitInfo.point);
                    Gizmos.DrawSphere(this.transform.position, gizmoSize);
                    Gizmos.DrawSphere(hitInfo.point, gizmoSize);
                    return;
                }

                Gizmos.color = activeColor;
                Gizmos.DrawLine(this.transform.position, hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
            }
            else
            {
                Gizmos.color = nonActiveColor;
                Gizmos.DrawLine(this.transform.position, this.transform.position + (fwd * MaxTripDistance));
            }

            Gizmos.DrawSphere(this.transform.position, gizmoSize);
        }

        #endregion

        #region Unity Functions

        void Start()
        {
            cgf.Enable = !TripValue;
        }

        void Update()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.forward);

            RaycastHit hitInfo;

            if (Physics.Raycast(this.transform.position, fwd, out hitInfo, maxTripDistance))
            {
                if (hitInfo.distance > maxTripDistance)
                {
                    cgf.Enable = !TripValue;
					return;
                }

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
