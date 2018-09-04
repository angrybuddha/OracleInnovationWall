/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 12-20-13
* Last Updated: 06-15-15
* Description: Used for setting the scene time scale.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
    public class SceneTimeScale : MonoBehaviour
    {
        #region Properties

		[SerializeField, Range(0, 1f)]
        private float customeTime = 1.0f;
		public float CustomeTime
		{
			get { return customeTime; }
			set { customeTime = value; }
		}

        #endregion

        #region Unity Functions

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
			Time.timeScale = CustomeTime;
        }

        #endregion
    }
}