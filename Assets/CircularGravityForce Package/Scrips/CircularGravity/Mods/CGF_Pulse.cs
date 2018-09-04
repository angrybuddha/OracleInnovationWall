/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for cgf mod, creates a pulse effect using the cgf.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace CircularGravityForce
{
    [RequireComponent(typeof(CircularGravity))]
    public class CGF_Pulse : MonoBehaviour
    {
        #region Classis

        //Pulse properties
        [System.Serializable]
        public class PulseProperties
        {
            //Enable a Pulse
            [SerializeField]
            private bool pulse = true;
            public bool Pulse
            {
                get { return pulse; }
                set { pulse = value; }
            }

            //Pulsing speed if pulse if enabled
            [SerializeField]
            private float speed = 10f;
            public float Speed
            {
                get { return speed; }
                set { speed = value; }
            }

            //Minimum pulse size
            [SerializeField]
            private float minSize = 1f;
            public float MinSize
            {
                get { return minSize; }
                set { MinSize = value; }
            }

            //Maximum pulse size
            [SerializeField]
            private float maxSize = 5f;
            public float MaxSize
            {
                get { return maxSize; }
                set { maxSize = value; }
            }
        }

        #endregion

        #region Properties/Constructor

        [SerializeField]
        private PulseProperties pulseProperties;
        public PulseProperties _pulseProperties
        {
            get { return pulseProperties; }
            set { pulseProperties = value; }
        }

        private CircularGravity cgf;

        //Used to tell whether to add or subtract to pulse
        private bool pulse_Positive;

        public CGF_Pulse()
        {
            _pulseProperties = new PulseProperties();
        }

        #endregion

        #region Unity Functions

        void Start()
        {
            cgf = this.GetComponent<CircularGravity>();

            //Sets up pulse
            if (_pulseProperties.Pulse)
            {
                cgf.Size = _pulseProperties.MinSize;
                pulse_Positive = true;
            }
        }

        void Update()
        {
            if (cgf.Enable)
            {
                if (_pulseProperties.Pulse)
                {
                    CalculatePulse();
                }
            }
        }

        #endregion

        #region Functions

        //Calculatie the given pulse
        private void CalculatePulse()
        {
            if (_pulseProperties.Pulse)
            {
                if (pulse_Positive)
                {
                    if (cgf.Size <= _pulseProperties.MaxSize)
                        cgf.Size = cgf.Size + (_pulseProperties.Speed * Time.deltaTime);
                    else
                        pulse_Positive = false;
                }
                else
                {
                    if (cgf.Size >= _pulseProperties.MinSize)
                        cgf.Size = cgf.Size - (_pulseProperties.Speed * Time.deltaTime);
                    else
                        pulse_Positive = true;
                }
            }
        }

        #endregion
    }
}
