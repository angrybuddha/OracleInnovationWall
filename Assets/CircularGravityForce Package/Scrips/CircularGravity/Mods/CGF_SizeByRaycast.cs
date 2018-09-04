/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf mod, sizes the cgf object based of the raycast hit point.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CircularGravityForce
{
    [RequireComponent(typeof(CircularGravity))]
    public class CGF_SizeByRaycast : MonoBehaviour
    {
        #region Properties

        //Offset of the Raycast, if needed
        [SerializeField]
        private float offsetRaycast = 1f;
        public float OffsetRaycast
        {
            get { return offsetRaycast; }
            set { offsetRaycast = value; }
        }

        //Max distance
        [SerializeField]
        private float maxCgfSize = 10f;
        public float MaxCgfSize
        {
            get { return maxCgfSize; }
            set { maxCgfSize = value; }
        }

		//Hitpoint
		[SerializeField]
		private Vector3 hitPoint;
		public Vector3 HitPoint
		{
			get { return hitPoint; }
			set { hitPoint = value; }
		}

        private CircularGravity cgf;

        private float gizmoSize = .25f;

        #endregion

        #region Gizmos

        void OnDrawGizmos()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.forward);

            RaycastHit hitInfo;

            if(this.GetComponent<CircularGravity>() != null)
            {
                gizmoSize = (this.GetComponent<CircularGravity>().Size / 8f);
                if (gizmoSize > .25f)
                    gizmoSize = .25f;
                else if (gizmoSize < -.25f)
                    gizmoSize = -.25f;
            }

            Color activeColor = Color.cyan;
            Color nonActiveColor = Color.white;

            if (Physics.Raycast(this.transform.position, fwd, out hitInfo))
            {
                if (hitInfo.distance > maxCgfSize)
                {
                    Gizmos.color = nonActiveColor;
                    Gizmos.DrawLine(this.transform.position, hitInfo.point);
                    Gizmos.DrawSphere(hitInfo.point, gizmoSize);
                    return;
                }

                Gizmos.color = activeColor;
                Gizmos.DrawLine(this.transform.position, hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
                Gizmos.DrawSphere(hitInfo.point + (fwd * OffsetRaycast), gizmoSize);
            }
            else
            {
                Gizmos.color = nonActiveColor;
                Gizmos.DrawLine(this.transform.position, this.transform.position + (fwd * 1000f));
            }
        }

        #endregion

        #region Unity Functions

        // Use this for initialization
        void Start()
        {
            cgf = this.GetComponent<CircularGravity>();

            cgf.Size = maxCgfSize;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.forward);

            RaycastHit hitInfo;

            if (Physics.Raycast(this.transform.position, fwd, out hitInfo))
            {
                if (hitInfo.distance > maxCgfSize)
                {
                    cgf.Size = maxCgfSize + OffsetRaycast;
                    return;
                }

                cgf.Size = hitInfo.distance + OffsetRaycast;

				hitPoint = hitInfo.point;
            }

			if(hitInfo.distance == 0)
			{
				hitPoint = Vector3.zero;
				cgf.Size = maxCgfSize + OffsetRaycast;
			}
        }

        #endregion
    }
}