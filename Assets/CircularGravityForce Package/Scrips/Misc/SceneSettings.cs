/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Used for managing the demo scenes.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CircularGravityForce
{
    public class SceneSettings : MonoBehaviour
    {
        //Singleton Logic
        private static SceneSettings _instance;
        public static SceneSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<SceneSettings>();

                return _instance;
            }
        }

        #region Properties

        [SerializeField, Header("Scene Options:")]
        private bool toggleMainMenu = false;
        public bool ToggleMainMenu
        {
            get { return toggleMainMenu; }
            set { toggleMainMenu = value; }
        }
        [SerializeField]
        private bool lockMouse = false;
        public bool LockMouse
        {
            get { return lockMouse; }
            set { lockMouse = value; }
        }

        [SerializeField, Header("Canvas Objects:")]
        private GameObject guiCanvas;
        public GameObject GUICanvas
        {
            get { return guiCanvas; }
            set { guiCanvas = value; }
        }
        [SerializeField]
        private Text helpUIText;
        public Text HelpUIText
        {
            get { return helpUIText; }
            set { helpUIText = value; }
        }
        [SerializeField]
        private GameObject panel_MainMenu;
        public GameObject Panel_MainMenu
        {
            get { return panel_MainMenu; }
            set { panel_MainMenu = value; }
        }

        [SerializeField, Header("FPS Scene Objects:")]
        private Gun fps_GunScript;
        public Gun Fps_GunScript
        {
            get { return fps_GunScript; }
            set { fps_GunScript = value; }
        }
        [SerializeField]
        private UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController;
        public UnityStandardAssets.Characters.FirstPerson.FirstPersonController FirstPersonController
        {
            get { return firstPersonController; }
            set { firstPersonController = value; }
        }

		private bool toggleCGF = false;
        public bool ToggleCGF
        {
            get { return toggleCGF; }
            set { toggleCGF = value; }
        }

		private bool toggleGUI = true;
        public bool ToggleGUI
        {
            get { return toggleGUI; }
            set { toggleGUI = value; }
        }

        #endregion

        #region Unity Functions

        void Update()
        {
            //Main Key
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                ToggleMainMenu = !ToggleMainMenu;
            }
            //Reset Scene Key
            if (Input.GetKeyUp(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
            //Toggle CGF Lines
            if (Input.GetKeyUp(KeyCode.T))
            {
                ToggleCGFLines();
            }

			//Toggles GUI
			if(Input.GetKeyUp(KeyCode.Home))
			{
                ToggleGUILayout();
			}
		
            //Help ESC text
            if (HelpUIText != null)
            {
                if (ToggleMainMenu)
                    HelpUIText.text = "Press <b>ESC</b> to go back.";
                else
                    HelpUIText.text = "Press <b>ESC</b> for menu.";
            }

            if (Panel_MainMenu != null)
                Panel_MainMenu.SetActive(ToggleMainMenu);

            if (Fps_GunScript != null)
                Fps_GunScript.enabled = (!ToggleMainMenu);

            if (FirstPersonController != null)
                FirstPersonController.enabled = (!ToggleMainMenu);

            #if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9)
                if((!ToggleMainMenu && LockMouse))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            #else
                Screen.lockCursor = (!ToggleMainMenu && LockMouse); ;
            #endif
        }

        #endregion

        #region Functions

        //Toggle CGF Lines function
		public void ToggleCGFLines()
		{
            ToggleCGF = !ToggleCGF;

			//2D
			CircularGravity[] cgfs = GameObject.FindObjectsOfType<CircularGravity>();
			foreach(var cgf in cgfs)
			{
                cgf._drawGravityProperties.DrawGravityForce = ToggleCGF;
			}

			//3D
			CircularGravity2D[] cgfs2D = GameObject.FindObjectsOfType<CircularGravity2D>();
			foreach(var cgf in cgfs2D)
			{
                cgf._drawGravityProperties.DrawGravityForce = ToggleCGF;
			}
		}
	
		public void ToggleGUILayout()
		{
            ToggleGUI = !ToggleGUI;

            GUICanvas.SetActive(ToggleGUI);
		}
	
        //Load scene function
        public void LoadScene(string sceneName)
        {
            Application.LoadLevel(sceneName);
        }

        #endregion
    }
}