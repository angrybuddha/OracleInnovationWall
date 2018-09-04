/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-21-14
* Last Updated: 06-15-15
*******************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CircularGravityForce
{
    public class CircularGravity_Tool : EditorWindow
    {
        #region Enumes

        enum CGFOptions
        {
            _3D,
            _2D
        }

        //Constructor
        public CircularGravity_Tool()
        {
        }

        #endregion

        #region ToolBar

        // Add menu named "Tools -> CGF 3 -> Wizard" to Toolbar
        [MenuItem("Tools/CGF 3/Wizard #%w", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = EditorWindow.GetWindow(typeof(CircularGravity_Tool));

            #if (UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9)
                editorWindow.titleContent = new GUIContent("CGF 3 Wizard");
            #else
                editorWindow.title = "CGF 3 Wizard";
            #endif
        }

        [MenuItem("Tools/CGF 3/Triggers/Create 'Enable'", false, 1)]
        public static void Trigger_CreateEnable()
        {
            bool isCreated = false;

            if(Selection.activeGameObject != null)
            {
                var selectedObject = Selection.activeGameObject;

                if(selectedObject.GetComponent<CircularGravity>() != null)
                {
                    var cgf = selectedObject;
                    var circularGravity = selectedObject.GetComponent<CircularGravity>();

                    CreateEnableTrigger(cgf, circularGravity);

                    isCreated = true;
                }
            }
            if (!isCreated)
            {
                CreateEnableTrigger();

                Debug.LogWarning("Warning: Dont forget to set the Enable Triggers CGF propertie");
            }
        }

        [MenuItem("Tools/CGF 3/Triggers/Create 'Hover'", false, 2)]
        public static void Trigger_CreateHover()
        {
            bool isCreated = false;

            if (Selection.activeGameObject != null)
            {
                var selectedObject = Selection.activeGameObject;

                if (selectedObject.GetComponent<CircularGravity>() != null)
                {
                    var cgf = selectedObject;
                    var circularGravity = selectedObject.GetComponent<CircularGravity>();

                    CreateHoverTrigger(cgf, circularGravity);

                    isCreated = true;
                }
            }
            if (!isCreated)
            {
                CreateHoverTrigger();

                Debug.LogWarning("Warning: Dont forget to set the Hover Triggers CGF propertie");
            }
        }

        [MenuItem("Tools/CGF 3/Triggers/Create 2D 'Enable'", false, 3)]
        public static void Trigger2D_CreateEnable()
        {
            bool isCreated = false;

            if (Selection.activeGameObject != null)
            {
                var selectedObject = Selection.activeGameObject;

                if (selectedObject.GetComponent<CircularGravity2D>() != null)
                {
                    var cgf = selectedObject;
                    var circularGravity = selectedObject.GetComponent<CircularGravity2D>();

                    CreateEnableTrigger2D(cgf, circularGravity);

                    isCreated = true;
                }
            }
            if (!isCreated)
            {
                CreateEnableTrigger2D();

                Debug.LogWarning("Warning: Dont forget to set the Enable Triggers CGF propertie");
            }
        }

        [MenuItem("Tools/CGF 3/Triggers/Create 2D 'Hover'", false, 4)]
        public static void Trigger2D_CreateHover()
        {
            bool isCreated = false;

            if (Selection.activeGameObject != null)
            {
                var selectedObject = Selection.activeGameObject;

                if (selectedObject.GetComponent<CircularGravity2D>() != null)
                {
                    var cgf = selectedObject;
                    var circularGravity = selectedObject.GetComponent<CircularGravity2D>();

                    CreateHoverTrigger2D(cgf, circularGravity);

                    isCreated = true;
                }
            }
            if (!isCreated)
            {
                CreateHoverTrigger2D();

                Debug.LogWarning("Warning: Dont forget to set the Hover Trigger CGF propertie");
            }
        }
        
        [MenuItem("Tools/CGF 3/Tutorial Videos/Basic Overview", false, 5)]
        public static void SupportTutVidBasicOverview()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=4Y4vxsAgVWg");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Advanced Overview", false, 6)]
        public static void SupportTutVidAdvanceOverview()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=qEi8GQRxQAY");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Simple Car", false, 7)]
        public static void SupportTutVidCar()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=cc5Wlsesgbo");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Simple Rocket", false, 8)]
        public static void SupportTutVidRocket()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=edrS9BTXx1E");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Simple Ball", false, 9)]
        public static void SupportTutVidBall()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=Ba9Quxq7x08");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Simple Hovercraft", false, 10)]
        public static void SupportTutVidHovercraft()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=bdh0ekHca7U");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Simple Planets", false, 11)]
        public static void SupportTutVidPlanets()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=LAYr6b7NQ20");
        }
        [MenuItem("Tools/CGF 3/Tutorial Videos/Filtering", false, 12)]
        public static void SupportTutVidFiltering()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=oiXBFhMO4cM");
        }
        [MenuItem("Tools/CGF 3/Support/Unity Form", false, 13)]
        public static void SupportUnityForm()
        {
            Application.OpenURL("http://forum.unity3d.com/threads/circular-gravity-force.217100/");
        }
        [MenuItem("Tools/CGF 3/Support/Asset Store", false, 14)]
        public static void SupportAssetStore()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/8181");
        }
        [MenuItem("Tools/CGF 3/Support/Website", false, 15)]
        public static void SupportWebsite()
        {
            Application.OpenURL("http://resurgamstudios.com/cgf.html");
        }
		
        #endregion

        #region Properties

        //GUI Properties
        private Vector2 scrollPos;

        private CGFOptions cgfOptions = CGFOptions._3D;

        //3D
		private CircularGravity.Shape cgfShape = CircularGravity.Shape.Sphere;
		private CircularGravity.ForceType cgfForceType = CircularGravity.ForceType.ForceAtPosition;
		private ForceMode cgfForceMode = ForceMode.Force;
		private float cfgExplosionForceUpwardsModifier = 0f;
		private float cfgTorqueMaxAngularVelocity = 10f;
		private float cgfSize = 5;
        private float cgfCapsuleRadius = 2;
		private float cgfForcePower = 10;
        private bool cgfAlignToForce = false;
        private float cgfslerpSpeed = 8f;
        private bool cgfDrawGravityForce = false;
        private float cgfDrawGravityForceThickness = 0.05f;
        private bool modAxisControls = false;
		private bool modKeyControls = false;
        private bool modPulse = false;
        private bool modSize = false;
        private bool triggerEnable = false;
        private bool triggerHover = false;
		private bool buttonCreate3D = false;
        

        //2D
        private CircularGravity2D.Shape2D cgfShape2D = CircularGravity2D.Shape2D.Sphere;
        private CircularGravity2D.ForceType2D cgfForceType2D = CircularGravity2D.ForceType2D.ForceAtPosition;
        private ForceMode2D cgfForceMode2D = ForceMode2D.Force;
        private float cgfSize2D = 5;
        private float cgfForcePower2D = 10;
        private bool cgfAlignToForce2D = false;
        private float cgfslerpSpeed2D = 8f;
        private bool cgfDrawGravityForce2D = false;
        private float cgfDrawGravityForceThickness2D = 0.05f;
        private bool modAxisControls2D = false;
		private bool modKeyControls2D = false;
        private bool modPulse2D = false;
        private bool modSize2D = false;
        private bool triggerEnable2D = false;
        private bool triggerHover2D = false;
        private bool buttonCreate2D = false;

        #endregion

        #region Unity Functions

        void OnGUI()
        {
			GUILayout.BeginHorizontal();
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Circular Gravity Force:", EditorStyles.boldLabel);
            cgfOptions = (CGFOptions)EditorGUILayout.EnumPopup("   Type:", cgfOptions);

            switch (cgfOptions)
            {
                case CGFOptions._3D:
                    DrawCGFGUI();
                    break;
                case CGFOptions._2D:
                    DrawCGF2DGUI();
                    break;
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        void Update()
        {
            switch (cgfOptions)
            {
                case CGFOptions._3D:
                    UpdateCGFGUI();
                    break;
                case CGFOptions._2D:
                    UpdateCGF2DGUI();
                    break;
            }
        }

        private void DrawCGFGUI()
        {
            cgfSize = EditorGUILayout.FloatField("   Size:", cgfSize);

            cgfForcePower = EditorGUILayout.FloatField("   Force Power:", cgfForcePower);

            cgfShape = (CircularGravity.Shape)EditorGUILayout.EnumPopup("   Shape:", cgfShape);

            if (cgfShape == CircularGravity.Shape.Capsule)
            {
                cgfCapsuleRadius = EditorGUILayout.FloatField("   Capsule Radius:", cgfCapsuleRadius);
            }

            cgfForceType = (CircularGravity.ForceType)EditorGUILayout.EnumPopup("   Force Type:", cgfForceType);

            if (cgfForceType == CircularGravity.ForceType.ExplosionForce)
            {
                cfgExplosionForceUpwardsModifier = EditorGUILayout.FloatField("   Upwards Modifier:", cfgExplosionForceUpwardsModifier);
            }
            else if (cgfForceType == CircularGravity.ForceType.Torque)
            {
                cfgTorqueMaxAngularVelocity = EditorGUILayout.FloatField("   Max Angular Velocity:", cfgTorqueMaxAngularVelocity);
            }

            cgfForceMode = (ForceMode)EditorGUILayout.EnumPopup("   Force Mode:", cgfForceMode);

            if (cgfForceType == CircularGravity.ForceType.ExplosionForce)
            {
                cgfCapsuleRadius = EditorGUILayout.FloatField("   Capsule Radius:", cgfCapsuleRadius);
            }

            cgfAlignToForce = EditorGUILayout.Toggle("   Align to Force:", cgfAlignToForce);
            if (cgfAlignToForce)
            {
                cgfslerpSpeed = EditorGUILayout.FloatField("   Slerp Speed:", cgfslerpSpeed);
            }

            cgfDrawGravityForce = EditorGUILayout.Toggle("   Draw Gravity Force:", cgfDrawGravityForce);
            if (cgfDrawGravityForce)
            {
                cgfDrawGravityForceThickness = EditorGUILayout.FloatField("   Thickness:", cgfDrawGravityForceThickness);
            }

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Controls:", EditorStyles.boldLabel);
			modAxisControls = EditorGUILayout.Toggle("   Add 'Axis Controls':", modAxisControls);
			modKeyControls = EditorGUILayout.Toggle("   Add 'Key Controls':", modKeyControls);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mods:", EditorStyles.boldLabel);
            modPulse = EditorGUILayout.Toggle("   Add 'Pulse':", modPulse);
            modSize = EditorGUILayout.Toggle("   Add 'Size By Raycast':", modSize);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tiggers:", EditorStyles.boldLabel);
            triggerEnable = EditorGUILayout.Toggle("   Create 'Enable':", triggerEnable);
            triggerHover = EditorGUILayout.Toggle("   Create 'Hover':", triggerHover);

            EditorGUILayout.Space();

            buttonCreate3D = GUILayout.Button("Create CGF");
        }

        private void DrawCGF2DGUI()
        {
            cgfSize2D = EditorGUILayout.FloatField("   Size:", cgfSize2D);
            cgfForcePower2D = EditorGUILayout.FloatField("   Force Power:", cgfForcePower2D);

            cgfShape2D = (CircularGravity2D.Shape2D)EditorGUILayout.EnumPopup("   Shape:", cgfShape2D);

            cgfForceType2D = (CircularGravity2D.ForceType2D)EditorGUILayout.EnumPopup("   Force Type:", cgfForceType2D);

            cgfForceMode2D = (ForceMode2D)EditorGUILayout.EnumPopup("   Force Mode:", cgfForceMode2D);

            cgfAlignToForce2D = EditorGUILayout.Toggle("   Align to Force:", cgfAlignToForce2D);
            if (cgfAlignToForce2D)
            {
                cgfslerpSpeed2D = EditorGUILayout.FloatField("   Slerp Speed:", cgfslerpSpeed2D);
            }

            cgfDrawGravityForce2D = EditorGUILayout.Toggle("   Draw Gravity Force:", cgfDrawGravityForce2D);
            if (cgfDrawGravityForce2D)
            {
                cgfDrawGravityForceThickness2D = EditorGUILayout.FloatField("   Thickness:", cgfDrawGravityForceThickness2D);
            }

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Controls:", EditorStyles.boldLabel);
			modAxisControls2D = EditorGUILayout.Toggle("   Add 'Axis Controls':", modAxisControls2D);
			modKeyControls2D = EditorGUILayout.Toggle("   Add 'Key Controls':", modKeyControls2D);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mods:", EditorStyles.boldLabel);
            modPulse2D = EditorGUILayout.Toggle("   Add 'Pulse':", modPulse2D);
            modSize2D = EditorGUILayout.Toggle("   Add 'Size By Raycast':", modSize2D);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tiggers:", EditorStyles.boldLabel);
            triggerEnable2D = EditorGUILayout.Toggle("   Create 'Enable':", triggerEnable2D);
            triggerHover2D = EditorGUILayout.Toggle("   Create 'Hover':", triggerHover2D);

            EditorGUILayout.Space();

            buttonCreate2D = GUILayout.Button("Create CGF");
        }

        private void UpdateCGFGUI()
        {
            if (buttonCreate3D)
            {
                //Creates empty gameobject.
                GameObject cgf = new GameObject();

                //Creates Circular Gravity Force component
                CircularGravity circularGravity = cgf.AddComponent<CircularGravity>();

				//Adds Controls
                if(modAxisControls)
                {
                    cgf.AddComponent<CGF_AxisControls>();
                }

				if(modKeyControls)
				{
					cgf.AddComponent<CGF_KeyControls>();
				}

				//Adds Mods
                if(modPulse)
                {
                    cgf.AddComponent<CGF_Pulse>();
                }
                if(modSize)
                {
                    cgf.AddComponent<CGF_SizeByRaycast>();
                }

                //Sets up properties
                circularGravity._shape = cgfShape;
                circularGravity._forceType = cgfForceType;
                circularGravity.ForcePower = cgfForcePower;
                circularGravity._forceMode = cgfForceMode;
                circularGravity._forceTypeProperties.ExplosionForceUpwardsModifier = cfgExplosionForceUpwardsModifier;
                circularGravity._forceTypeProperties.TorqueMaxAngularVelocity = cfgTorqueMaxAngularVelocity;
                circularGravity.Size = cgfSize;
                circularGravity.CapsuleRadius = cgfCapsuleRadius;
                circularGravity._constraintProperties.AlignToForce = cgfAlignToForce;
                circularGravity._constraintProperties.SlerpSpeed = cgfslerpSpeed;

                circularGravity._drawGravityProperties.DrawGravityForce = cgfDrawGravityForce;
                circularGravity._drawGravityProperties.GravityLineMaterial = new Material(Shader.Find("UI/Default"));
                circularGravity._drawGravityProperties.Thickness = cgfDrawGravityForceThickness;

                //Sets gameojbect Name
                cgf.name = "CGF";

                FocusGameObject(cgf);

                if(triggerEnable)
                {
                    CreateEnableTrigger(cgf, circularGravity);
                }
                if (triggerHover)
                {
                    CreateHoverTrigger(cgf, circularGravity);
                }

                //Disables the buttonCreateNewCGF
                buttonCreate3D = false;
            }
        }

        private void UpdateCGF2DGUI()
        {
            if (buttonCreate2D)
            {
                //Creates empty gameobject.
                GameObject cgf = new GameObject();

                //Creates Circular Gravity Force component
                CircularGravity2D circularGravity = cgf.AddComponent<CircularGravity2D>();

				//Adds Controls
				if(modAxisControls2D)
				{
					cgf.AddComponent<CGF_AxisControls2D>();
				}
				
				if(modKeyControls2D)
				{
					cgf.AddComponent<CGF_KeyControls2D>();
				}
				
				//Adds Mods
				if(modPulse2D)
				{
					cgf.AddComponent<CGF_Pulse2D>();
				}
				if(modSize2D)
				{
					cgf.AddComponent<CGF_SizeByRaycast2D>();
				}

                //Sets up properties
                circularGravity._shape2D = cgfShape2D;
                circularGravity._forceType2D = cgfForceType2D;
                circularGravity.ForcePower = cgfForcePower2D;
                circularGravity._forceMode2D = cgfForceMode2D;
                circularGravity.Size = cgfSize2D;
                circularGravity._constraintProperties.AlignToForce = cgfAlignToForce2D;
                circularGravity._constraintProperties.SlerpSpeed = cgfslerpSpeed2D;

                circularGravity._drawGravityProperties.DrawGravityForce = cgfDrawGravityForce2D;
                circularGravity._drawGravityProperties.GravityLineMaterial = new Material(Shader.Find("UI/Default"));
                circularGravity._drawGravityProperties.Thickness = cgfDrawGravityForceThickness2D;

                //Sets gameojbect Name
                cgf.name = "CGF 2D";

                FocusGameObject(cgf, true);

                if (triggerEnable2D)
                {
                    CreateEnableTrigger2D(cgf, circularGravity);
                }
                if (triggerHover2D)
                {
                    CreateHoverTrigger2D(cgf, circularGravity);
                }

                //Disables the buttonCreateNewCGF
                buttonCreate2D = false;
            }
        }

        #endregion

        #region Events

        private static void CreateEnableTrigger(GameObject cgf = null, CircularGravity circularGravity = null)
        {
            GameObject triggerEnableObj = new GameObject();
            triggerEnableObj.name = "Trigger Enable";
            if (circularGravity != null)
            {
                triggerEnableObj.AddComponent<CGF_EnableTrigger>().Cgf = circularGravity;
            }
            else
            {
                triggerEnableObj.AddComponent<CGF_EnableTrigger>();
            }
            if (cgf != null)
            {
                triggerEnableObj.transform.SetParent(cgf.transform, false);
            }
            triggerEnableObj.transform.position = triggerEnableObj.transform.position + Vector3.right * 6f;
            triggerEnableObj.transform.rotation = Quaternion.Euler(0, 90, 0);

            if(cgf == null)
            {
                FocusGameObject(triggerEnableObj);
            }
        }

        private static void CreateHoverTrigger(GameObject cgf = null, CircularGravity circularGravity = null)
        {
            GameObject triggerEnableObj = new GameObject();
            triggerEnableObj.name = "Trigger Hover";
            if (circularGravity != null)
            {
                triggerEnableObj.AddComponent<CGF_HoverTrigger>().Cgf = circularGravity;
            }
            else
            {
                triggerEnableObj.AddComponent<CGF_HoverTrigger>();
            }
            if (cgf != null)
            {
                triggerEnableObj.transform.SetParent(cgf.transform, false);
            }
            triggerEnableObj.transform.position = triggerEnableObj.transform.position + Vector3.left * 6f;
            triggerEnableObj.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (cgf == null)
            {
                FocusGameObject(triggerEnableObj);
            }
        }

        private static void CreateEnableTrigger2D(GameObject cgf = null, CircularGravity2D circularGravity = null)
        {
            GameObject triggerEnableObj = new GameObject();
            triggerEnableObj.name = "Trigger Enable";
            if (circularGravity != null)
            {
                triggerEnableObj.AddComponent<CGF_EnableTrigger2D>().Cgf = circularGravity;
            }
            else
            {
                triggerEnableObj.AddComponent<CGF_EnableTrigger2D>();
            }
            if (cgf != null)
            {
                triggerEnableObj.transform.SetParent(cgf.transform, false);
            }
            triggerEnableObj.transform.position = triggerEnableObj.transform.position + new Vector3(0, 6f);

            if (cgf == null)
            {
                FocusGameObject(triggerEnableObj, true);
            }
        }

        private static void CreateHoverTrigger2D(GameObject cgf = null, CircularGravity2D circularGravity = null)
        {
            GameObject triggerEnableObj = new GameObject();
            triggerEnableObj.name = "Trigger Hover";
            if (circularGravity != null)
            {
                triggerEnableObj.AddComponent<CGF_HoverTrigger2D>().Cgf = circularGravity;
            }
            else
            {
                triggerEnableObj.AddComponent<CGF_HoverTrigger2D>();
            }
            if (cgf != null)
            {
                triggerEnableObj.transform.SetParent(cgf.transform, false);
            }
            triggerEnableObj.transform.position = triggerEnableObj.transform.position + new Vector3(0, -6f);
            triggerEnableObj.transform.rotation = Quaternion.Euler(0, 0, 180f);

            if (cgf == null)
            {
                FocusGameObject(triggerEnableObj, true);
            }
        }

        private static void FocusGameObject(GameObject focusGameObject, bool in2D = false)
        {
            //Sets the create location for the Circular Gravity Force gameobject
            if (SceneView.lastActiveSceneView != null)
            {
                if (in2D)
                    focusGameObject.transform.position = new Vector3(SceneView.lastActiveSceneView.pivot.x, SceneView.lastActiveSceneView.pivot.y, 0f);
                else
                    focusGameObject.transform.position = SceneView.lastActiveSceneView.pivot;

                //Sets the Circular Gravity Force gameobject selected in the hierarchy
                Selection.activeGameObject = focusGameObject;

                //focus the editor camera on the Circular Gravity Force gameobject
                SceneView.lastActiveSceneView.FrameSelected();
            }
            else
            {
                focusGameObject.transform.position = Vector3.zero;
            }
        }

        #endregion
    }
}