/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf trigger, used creating a hover effect using the cgf object.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CircularGravityForce
{
	public class CGF_HoverTrigger : MonoBehaviour
    {
        #region Properties

        //CircularGravity used for the hover
        [SerializeField]
        private CircularGravity cgf;
        public CircularGravity Cgf
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

			if (Physics.Raycast(this.transform.position, fwd, out hitInfo, maxDistance, layerMask))
			{
                if (hitInfo.distance < maxDistance)
                {
                    Gizmos.color = activeColor;
                }
                else
                {
                    Gizmos.color = nonActiveColor;
                }

                Gizmos.DrawLine(this.transform.position, hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
			}
			else
			{
				Gizmos.color = Color.white;
				Gizmos.DrawLine(this.transform.position, this.transform.position + (fwd * MaxDistance));
			}

            Gizmos.DrawSphere(this.transform.position, gizmoSize);
		}
		
        #endregion

        #region Unity Functions

        void Update()
		{
			layerMask = ~(1 << LayerMask.NameToLayer (IgnoreLayer));

			Vector3 fwd = this.transform.TransformDirection(Vector3.forward);
			
			RaycastHit hitInfo;
			
			if (Physics.Raycast(this.transform.position, fwd, out hitInfo, maxDistance, layerMask))
			{
                if (hitInfo.distance < maxDistance)
                {
                    float proportionalHeight = (HoverDistance - hitInfo.distance) / HoverDistance;
                    cgf.ForcePower = proportionalHeight * ForcePower;
                }
                else
                {
                    cgf.ForcePower = 0f;
                }
			}
			else
			{
				cgf.ForcePower = 0f;
			}
        }

        #endregion
    }
}
