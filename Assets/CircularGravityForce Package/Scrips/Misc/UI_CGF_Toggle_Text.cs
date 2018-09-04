/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for setting the on/off text for the viewing cgf button.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CircularGravityForce
{
    public class UI_CGF_Toggle_Text : MonoBehaviour
    {
        #region Properties

        private Text text;

        #endregion

        #region Unity Functions

        // Use this for initialization
        void Start()
        {
            if (GameObject.Find("SceneSettings") != null)
            {
                text = this.GetComponent<Text>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (text != null && SceneSettings.Instance != null)
            {
                if (SceneSettings.Instance.ToggleCGF)
                {
                    text.text = "On";
                }
                else
                {
                    text.text = "Off";
                }
            }
        }

        #endregion
    }
}