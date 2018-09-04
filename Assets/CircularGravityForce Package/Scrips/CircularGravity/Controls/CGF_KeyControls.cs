/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for when wanting to setup key controls for the cgf gameobject.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CircularGravityForce
{
    [RequireComponent(typeof(CircularGravity))]
    public class CGF_KeyControls : MonoBehaviour
    {
        #region Enums

        public enum CGFControls
		{
			Enable,
		}

        #endregion

        #region Classis

        [System.Serializable]
        public class EnableControls
        {
            [SerializeField]
            private KeyCode keyCode;
            public KeyCode _keyCode
            {
                get { return keyCode; }
                set { keyCode = value; }
            }
            [SerializeField]
            private bool pressValue;
            public bool PressValue
            {
                get { return pressValue; }
                set { pressValue = value; }
            }
        }

        [System.Serializable]
        public class SizeControls
        {
            [SerializeField]
            private KeyCode keyCode;
            public KeyCode _keyCode
            {
                get { return keyCode; }
                set { keyCode = value; }
            }
            [SerializeField]
            private float pressValue;
            public float PressValue
            {
                get { return pressValue; }
                set { pressValue = value; }
            }
        }

        [System.Serializable]
        public class ForcePowerControls
        {
            [SerializeField]
            private KeyCode keyCode;
            public KeyCode _keyCode
            {
                get { return keyCode; }
                set { keyCode = value; }
            }
            [SerializeField]
            private float pressValue;
            public float PressValue
            {
                get { return pressValue; }
                set { pressValue = value; }
            }
        }

        [System.Serializable]
        public class EnableControler
        {
            [SerializeField]
            private bool idleValue = false;
            public bool IdleValue
            {
                get { return idleValue; }
                set { idleValue = value; }
            }
            [SerializeField]
            private List<EnableControls> enableControls;
            public List<EnableControls> _enableControls
            {
                get { return enableControls; }
                set { enableControls = value; }
            }
        }
        [System.Serializable]
        public class ForceControler
        {
            [SerializeField]
            private float idleValue = 0f;
            public float IdleValue
            {
                get { return idleValue; }
                set { idleValue = value; }
            }
            [SerializeField]
            private List<ForcePowerControls> forcePowerControls;
            public List<ForcePowerControls> _forcePowerControls
            {
                get { return forcePowerControls; }
                set { forcePowerControls = value; }
            }
        }
        [System.Serializable]
        public class SizeControler
        {
            [SerializeField]
            private float idleValue = 0f;
            public float IdleValue
            {
                get { return idleValue; }
                set { idleValue = value; }
            }
            [SerializeField]
            private List<SizeControls> sizeControls;
            public List<SizeControls> _sizeControls
            {
                get { return sizeControls; }
                set { sizeControls = value; }
            }
        }

        #endregion

        #region Properties/Constructor

        public CGF_KeyControls()
        {
            _enableControler = new EnableControler();
            _enableControler._enableControls = new List<EnableControls>();
            _forceControler = new ForceControler();
            _forceControler._forcePowerControls = new List<ForcePowerControls>();
            _sizeControler = new SizeControler();
            _sizeControler._sizeControls = new List<SizeControls>();
        }

        [SerializeField]
        private EnableControler enableControler;
        public EnableControler _enableControler
        {
            get { return enableControler; }
            set { enableControler = value; }
        }
        
        [SerializeField]
        private ForceControler forceControler;
        public ForceControler _forceControler
        {
            get { return forceControler; }
            set { forceControler = value; }
        }
        [SerializeField]
        private SizeControler sizeControler;
        public SizeControler _sizeControler
        {
            get { return sizeControler; }
            set { sizeControler = value; }
        }

        private CircularGravity cgf;

		bool flagEnableControl = false;
		bool flagSizeControl = false;
		bool flagPowerControl = false;

        #endregion

        #region Unity Functions

        void Awake()
        {
            cgf = this.GetComponent<CircularGravity>();

			SetIdleValues();
        }

        void Update()
        {
			flagEnableControl = false;
			foreach (var enableControl in _enableControler._enableControls) 
			{
				if (enableControl._keyCode != KeyCode.None) 
				{
					if (Input.GetKey (enableControl._keyCode)) 
					{
						cgf.Enable = enableControl.PressValue;
						flagEnableControl = true;
					}
				}
			}

			flagSizeControl = false;
			foreach (var sizeControl in _sizeControler._sizeControls) 
			{
				if (sizeControl._keyCode != KeyCode.None) 
				{
					if (Input.GetKey (sizeControl._keyCode)) 
					{
						cgf.Size = sizeControl.PressValue;
						flagSizeControl = true;
					}
				}
			}

			flagPowerControl = false;
			foreach (var forcePowerControl in _forceControler._forcePowerControls) 
			{
				if (forcePowerControl._keyCode != KeyCode.None) 
				{
					if (Input.GetKey (forcePowerControl._keyCode)) 
					{
						cgf.ForcePower = forcePowerControl.PressValue;
						flagPowerControl = true;
					}
				}
			}

			SetIdleValues();
        }

        #endregion

        #region Functions

        void SetIdleValues()
		{
			if (!flagEnableControl && _enableControler._enableControls.Count != 0)
			{
				cgf.Enable = _enableControler.IdleValue;
			}
			
			if (!flagSizeControl && _sizeControler._sizeControls.Count != 0)
			{
				cgf.Size = _sizeControler.IdleValue;
			}
			
			if (!flagPowerControl && _forceControler._forcePowerControls.Count != 0)
			{
				cgf.ForcePower = _forceControler.IdleValue;
			}
        }

        #endregion
    }
}