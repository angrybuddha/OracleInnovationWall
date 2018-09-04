/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for managing gun icons colors in the fps demo scene.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CircularGravityForce
{
	public class UI_GunIcon : MonoBehaviour
    {
        #region Properties

        [SerializeField]
		private Gun gun;
		public Gun _gun
		{
			get { return gun; }
			set { gun = value; }
		}

		[SerializeField]
		private KeyCode keyCodeButton;
		public KeyCode KeyCodeButton
		{
			get { return keyCodeButton; }
			set { keyCodeButton = value; }
		}

		[SerializeField]
		private CircularGravityForce.Gun.GunType gunType;
		public CircularGravityForce.Gun.GunType _gunType
		{
			get { return gunType; }
			set { gunType = value; }
		}

		[SerializeField]
		private Image icon;
		public Image Icon
		{
			get { return icon; }
			set { icon = value; }
		}

		[SerializeField]
		private Text text;
		public Text Text
		{
			get { return text; }
			set { text = value; }
		}

		private Color defaultColor = Color.white;
		private Color selectColor = Color.cyan;

        #endregion

        #region Unity Functions

        // Update is called once per frame
		void Update () 
		{
			if(Input.GetKeyDown(KeyCodeButton))
			{
				gun._gunType = gunType;
			}

			if(_gun._gunType == gunType)
			{
				Icon.color = selectColor;
				Text.color = selectColor;
			}
			else
			{
				Icon.color = defaultColor;
				Text.color = defaultColor;
			}
        }

        #endregion
    }
}