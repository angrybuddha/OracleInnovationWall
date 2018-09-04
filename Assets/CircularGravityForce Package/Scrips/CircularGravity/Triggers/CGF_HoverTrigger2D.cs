/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf trigger, used creating a hover effect using the cgf object in 2D.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
    public class CGF_HoverTrigger2D : MonoBehaviour
    {
        #region Properties

        //CircularGravity used for the hover 
        [SerializeField]
        private CircularGravity2D cgf;
        public CircularGravity2D Cgf
        {
            get { return cgf; }
            set { cgf = value; }
        }

        //Hover Force Power
        [SerializeField]
        private float forcePower = 30f;
        public float ForcePower
        {
            get { return forcePower; }
            set { forcePower = value; }
        }

        //Hover Distance
        [SerializeField]
        private float hoverDistance = 3f;
        public float HoverDistance
        {
            get { return hoverDistance; }
            set { hoverDistance = value; }
        }

        //Max Distance
        [SerializeField]
        private float maxDistance = 10f;
        public float MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }

        //Used for if you want to ignore a layer
		[SerializeField]
		private string ignoreLayer;
		public string IgnoreLayer
		{
			get { return ignoreLayer; }
			set { ignoreLayer = value; }
		}

		private LayerMask layerMask;

        private float gizmoSize = .25f;

        #endregion

        #region Gizmos

        void OnDrawGizmos()
        {
			layerMask = ~(1 << LayerMask.NameToLayer (IgnoreLayer));

            Vector3 fwd = this.transform.TransformDirection(Vector3.right);

			RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, fwd, MaxDistance, layerMask);

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
                Gizmos.DrawLine(this.transform.position, this.transform.position + (fwd * MaxDistance));
                return;
            }

            if (Vector2.Distance(this.transform.position, hitInfo.point) > maxDistance)
            {
                Gizmos.color = nonActiveColor;
                Gizmos.DrawLine(this.transform.position, hitInfo.point);

                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
                return;
            }
            else if (hitInfo.transform != null)
            {
                Gizmos.color = activeColor;
                Gizmos.DrawLine(this.transform.position, hitInfo.point);
                Gizmos.DrawSphere(this.transform.position, gizmoSize);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
            }
        }

        #endregion

        #region Unity Functions

        void Update()
        {
			layerMask = ~(1 << LayerMask.NameToLayer (IgnoreLayer));

            Vector3 fwd = this.transform.TransformDirection(Vector3.right);

			RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, fwd, MaxDistance, layerMask);

            if (Vector2.Distance(this.transform.position, hitInfo.point) > maxDistance)
            {
                cgf.ForcePower = 0f;
            }

            if (hitInfo.transform != null)
            {
                float proportionalHeight = (HoverDistance - Vector2.Distance(this.transform.position, hitInfo.point)) / HoverDistance;
                cgf.ForcePower = proportionalHeight * ForcePower;
            }
            else
            {
                cgf.ForcePower = 0f;
            }
        }

        #endregion
    }
}