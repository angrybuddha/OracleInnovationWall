/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf mod, sizes the cgf object based of the raycast hit point in 2D.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CircularGravityForce
{
    [RequireComponent(typeof(CircularGravity2D))]
    public class CGF_SizeByRaycast2D : MonoBehaviour
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
		private Vector2 hitPoint;
		public Vector2 HitPoint
		{
			get { return hitPoint; }
			set { hitPoint = value; }
		}

        private CircularGravity2D cgf;

        private float gizmoSize = .25f;

        #endregion

        #region Gizmos

        void OnDrawGizmos()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.right);

            RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, fwd);

            if (this.GetComponent<CircularGravity>() != null)
            {
                gizmoSize = (this.GetComponent<CircularGravity>().Size / 8f);
                if (gizmoSize > .25f)
                    gizmoSize = .25f;
                else if (gizmoSize < -.25f)
                    gizmoSize = -.25f;
            }

            Color activeColor = Color.cyan;
            Color nonActiveColor = Color.white;

            if (hitInfo.transform == null)
            {
                Gizmos.color = nonActiveColor;
                Gizmos.DrawLine(new Vector3(this.transform.position.x, this.transform.position.y, 0f), new Vector3(this.transform.position.x, this.transform.position.y, 0f) + (fwd * 1000f));
                return;
            }

            if (Vector2.Distance(this.transform.position, hitInfo.point) > maxCgfSize)
            {
                Gizmos.color = nonActiveColor;
                Gizmos.DrawLine(new Vector3(this.transform.position.x, this.transform.position.y, 0f), hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
                return;
            }
            else if(hitInfo.transform != null)
            {
                Gizmos.color = activeColor;
                Gizmos.DrawLine(new Vector3(this.transform.position.x, this.transform.position.y, 0f), hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, gizmoSize);
                Gizmos.DrawSphere((Vector3)hitInfo.point + (fwd * OffsetRaycast), gizmoSize);
            }

            
        }

        #endregion

        #region Unity Functions

        // Use this for initialization
        void Start()
        {
            cgf = this.GetComponent<CircularGravity2D>();

            cgf.Size = maxCgfSize;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 fwd = this.transform.TransformDirection(Vector3.right);

            RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, fwd);

            if (Vector2.Distance(this.transform.position, hitInfo.point) > maxCgfSize)
            {
                cgf.Size = maxCgfSize + OffsetRaycast;
				hitPoint = hitInfo.point;
                return;
            }

            if (Vector2.Distance(this.transform.position, hitInfo.point) == 0) 
			{
				cgf.Size = maxCgfSize + OffsetRaycast;
				hitPoint = Vector2.zero;
			} 
			else 
			{
                cgf.Size = Vector2.Distance(this.transform.position, hitInfo.point) + OffsetRaycast;
				hitPoint = hitInfo.point;
			}
        }

        #endregion
    }
}