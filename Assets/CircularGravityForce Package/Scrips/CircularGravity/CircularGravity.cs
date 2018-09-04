/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-15-13 
* Last Updated: 06-15-15
* Description: Core logic for Circular Gravity Force.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace CircularGravityForce
{
    public class CircularGravity : MonoBehaviour
    {
        #region Enums

        //Force Types
        public enum ForceType
        {
            ForceAtPosition,
            Force,
            Torque,
            ExplosionForce,
            GravitationalAttraction
        }

        //Force Types
        public enum Shape
        {
            Sphere,
            Capsule,
            RayCast
        }

        //Effect Types
        public enum PhysicsEffect
        {
            None,
            NoGravity
        }
        #endregion

        #region Classes

        //Manages all force type properties
        [System.Serializable]
        public class ForceTypeProperties
        {
            //Explosion force upwards modifier
            [SerializeField]
            private float explosionForceUpwardsModifier = 0f;
            public float ExplosionForceUpwardsModifier
            {
                get { return explosionForceUpwardsModifier; }
                set { explosionForceUpwardsModifier = value; }
            }

            //Torque force max angular velocity
            [SerializeField]
            private float torqueMaxAngularVelocity = 10f;
            public float TorqueMaxAngularVelocity
            {
                get { return torqueMaxAngularVelocity; }
                set { torqueMaxAngularVelocity = value; }
            }
        }

        //Manages all constraint properties
        [System.Serializable]
        public class ConstraintProperties
        {
            //Enables objects to always angle towards the force point
            [SerializeField]
            private bool alignToForce = false;
            public bool AlignToForce
            {
                get { return alignToForce; }
                set { alignToForce = value; }
            }

            //Slerp Align speed
            [SerializeField]
            private float slerpSpeed = 8f;
            public float SlerpSpeed
            {
                get { return slerpSpeed; }
                set { slerpSpeed = value; }
            }

            //Gameobject filter options
            [SerializeField]
            private GameobjectFilter gameobjectFilter;
            public GameobjectFilter _gameobjectFilter
            {
                get { return gameobjectFilter; }
                set { gameobjectFilter = value; }
            }

            //Tag filter options
            [SerializeField]
            private TagFilter tagFilter;
            public TagFilter _tagFilter
            {
                get { return tagFilter; }
                set { tagFilter = value; }
            }

            //Layer filter options
            [SerializeField]
            private LayerFilter layerFilter;
            public LayerFilter _layerFilter
            {
                get { return layerFilter; }
                set { layerFilter = value; }
            }
        }

        //Manages all effected objects for when using special effect
        [System.Serializable]
        public class SpecialEffect
        {
            //Physics Effect
            [SerializeField]
            private PhysicsEffect physicsEffect = PhysicsEffect.None;
            public PhysicsEffect _physicsEffect
            {
                get { return physicsEffect; }
                set { physicsEffect = value; }
            }

            //Time the effect lasts
            [SerializeField]
            private float timeEffected = 0.0f;
            public float TimeEffected
            {
                get { return timeEffected; }
                set { timeEffected = value; }
            }

            //Attach GameObject to effected object.
            [SerializeField]
            private GameObject attachedGameObject;
            public GameObject AttachedGameObject
            {
                get { return attachedGameObject; }
                set { attachedGameObject = value; }
            }

            //Holds all the effected objects of the timeEffected
            [SerializeField]
            private EffectedObjects effectedObjects;
            public EffectedObjects _effectedObjects
            {
                get { return effectedObjects; }
                set { effectedObjects = value; }
            }

            //Constructor
            public SpecialEffect()
            {
                _effectedObjects = new EffectedObjects();
            }
        }

        //GameObject filtering options
        [System.Serializable]
        public class GameobjectFilter
        {
            //GameObject filter options
            public enum GameObjectFilterOptions
            {
                Disabled,
                OnlyEffectListedGameobjects,
                DontEffectListedGameobjects,
            }

            //GameObject Filter Options
            [SerializeField]
            private GameObjectFilterOptions gameObjectFilterOptions = GameObjectFilterOptions.Disabled;
            public GameObjectFilterOptions _gameobjectFilterOptions
            {
                get { return gameObjectFilterOptions; }
                set { gameObjectFilterOptions = value; }
            }

            //Tags used for the filter option
            [SerializeField]
            private List<GameObject> gameobjectList;
            public List<GameObject> GameobjectList
            {
                get { return gameobjectList; }
                set { gameobjectList = value; }
            }
        }

        //Tag filtering options
        [System.Serializable]
        public class TagFilter
        {
            //Tag filter options
            public enum TagFilterOptions
            {
                Disabled,
                OnlyEffectListedTags,
                DontEffectListedTags,
            }

            //Tag Filter Options
            [SerializeField]
            private TagFilterOptions tagFilterOptions = TagFilterOptions.Disabled;
            public TagFilterOptions _tagFilterOptions
            {
                get { return tagFilterOptions; }
                set { tagFilterOptions = value; }
            }

            //Tags used for the filter option
            [SerializeField]
            private List<string> tagsList;
            public List<string> TagsList
            {
                get { return tagsList; }
                set { tagsList = value; }
            }
        }

        //Trigger Area Filter
        [System.Serializable]
        public class TriggerAreaFilter
        {
            //Trigger Options
            public enum TriggerAreaFilterOptions
            {
                Disabled,
                OnlyEffectWithinTigger,
                DontEffectWithinTigger,
            }

            //Trigger area filter options
            [SerializeField]
            private TriggerAreaFilterOptions triggerAreaFilterOptions = TriggerAreaFilterOptions.Disabled;
            public TriggerAreaFilterOptions _triggerAreaFilterOptions
            {
                get { return triggerAreaFilterOptions; }
                set { triggerAreaFilterOptions = value; }
            }

            //Trigger Object
            [SerializeField]
            private Collider triggerArea;
            public Collider TriggerArea
            {
                get { return triggerArea; }
                set { triggerArea = value; }
            }
        }

        //Tag filtering options
        [System.Serializable]
        public class LayerFilter
        {
            //Tag filter options
            public enum LayerFilterOptions
            {
                Disabled,
                OnlyEffectListedLayers,
                DontEffectListedLayers,
            }

            //Layer filter options
            [SerializeField]
            private LayerFilterOptions layerFilterOptions = LayerFilterOptions.Disabled;
            public LayerFilterOptions _layerFilterOptions
            {
                get { return layerFilterOptions; }
                set { layerFilterOptions = value; }
            }

            //Tags used for the filter option
            [SerializeField]
            private List<string> layerList;
            public List<string> LayerList
            {
                get { return layerList; }
                set { layerList = value; }
            }
        }

        //Draw gravity properties
        [System.Serializable]
        public class DrawGravityProperties
        {
            //Thinkness of the line drawn
            [SerializeField]
            private float thickness = 0.2f;
            public float Thickness
            {
                get { return thickness; }
                set { thickness = value; }
            }

            [SerializeField]
            private Material gravityLineMaterial;
            public Material GravityLineMaterial
            {
                get { return gravityLineMaterial; }
                set { gravityLineMaterial = value; }
            }

            //Used to see gravity
            [SerializeField]
            private bool drawGravityForce = true;
            public bool DrawGravityForce
            {
                get { return drawGravityForce; }
                set { drawGravityForce = value; }
            }

            //Used to see gravity area from gizmos
            private bool drawGravityForceGizmos = true;
            public bool DrawGravityForceGizmos
            {
                get { return drawGravityForceGizmos; }
                set { drawGravityForceGizmos = value; }
            }
        }

        //Manages all effected objects for when using SpecialEffect
        public class EffectedObjects
        {
            //List of all EffectedObject
            private List<EffectedObject> effectedObjectList;
            public List<EffectedObject> EffectedObjectList
            {
                get { return effectedObjectList; }
                set { effectedObjectList = value; }
            }

            //Constructor
            public EffectedObjects()
            {
                EffectedObjectList = new List<EffectedObject>();
            }

            //Used to add to the effectedObjectList
            public void AddedEffectedObject(Rigidbody touchedObject, PhysicsEffect physicsEffect, GameObject attachedGameObject)
            {
                if (EffectedObjectList.Count == 0)
                {
                    EffectedObject effectedObject = new EffectedObject(Time.time, touchedObject, physicsEffect);
                    EffectedObjectList.Add(effectedObject);

                    CreateAttachedGameObject(touchedObject, attachedGameObject);
                }
                else if ((EffectedObjectList.Count > 0))
                {
                    bool checkIfExists = false;

                    foreach (EffectedObject item in EffectedObjectList)
                    {
                        if (item.TouchedObject == touchedObject)
                        {
                            item.EffectedTime = Time.time;
                            checkIfExists = true;
                            break;
                        }
                    }

                    if (!checkIfExists)
                    {
                        EffectedObject effectedObject = new EffectedObject(Time.time, touchedObject, physicsEffect);
                        EffectedObjectList.Add(effectedObject);

                        CreateAttachedGameObject(touchedObject, attachedGameObject);
                    }
                }
            }

            //Creates the attached gameobject for the effect
            private static void CreateAttachedGameObject(Rigidbody touchedObject, GameObject attachGameObject)
            {
                if (attachGameObject != null)
                {
                    if (touchedObject.transform.FindChild(SpecialEffectGameObject) == null)
                    {
                        GameObject newAttachGameObject = Instantiate(attachGameObject, touchedObject.gameObject.transform.position, attachGameObject.gameObject.transform.rotation) as GameObject;
                        newAttachGameObject.name = SpecialEffectGameObject;
                        newAttachGameObject.transform.parent = touchedObject.gameObject.transform;
                    }
                }
            }

            //Removes the attached gameobject for the effect
            private static void RemoveAttachedGameObject(Rigidbody touchedObject, GameObject attachGameObject)
            {
                if (touchedObject != null && attachGameObject != null)
                {
                    if (touchedObject.transform.FindChild(SpecialEffectGameObject) != null)
                    {
                        Destroy(touchedObject.transform.FindChild(SpecialEffectGameObject).gameObject);
                    }
                }
            }

            //Refreshs the EffectedObjects over a timer
            public void RefreshEffectedObjectListOverTime(float timer, GameObject attachedGameObject)
            {
                if (EffectedObjectList.Count != 0)
                {
                    List<EffectedObject> removeItems = new List<EffectedObject>();

                    foreach (EffectedObject item in EffectedObjectList)
                    {
                        if (item.EffectedTime + timer < Time.time)
                        {
                            switch (item.physicsEffect)
                            {
                                case PhysicsEffect.None:
                                    break;
                                case PhysicsEffect.NoGravity:
                                    if (item.TouchedObject != null)
                                    {
                                        item.TouchedObject.useGravity = true;
                                    }
                                    break;
                            }

                            RemoveAttachedGameObject(item.TouchedObject, attachedGameObject);
                            removeItems.Add(item);
                        }
                    }

                    //Clears effected items out of the list
                    foreach (var item in removeItems)
                    {
                        EffectedObjectList.Remove(item);
                    }
                }
            }
        }

        //Data structure for effected object
        public class EffectedObject
        {
            public EffectedObject()
            {
            }

            public EffectedObject(float effectedTime, Rigidbody touchedObject, PhysicsEffect physicsEffect)
            {
                this.EffectedTime = effectedTime;
                this.TouchedObject = touchedObject;
                this.physicsEffect = physicsEffect;
            }

            //Time Effected
            private float effectedTime;
            public float EffectedTime
            {
                get { return effectedTime; }
                set { effectedTime = value; }
            }

            //Rigidbody of touched object
            private Rigidbody touchedObject;
            public Rigidbody TouchedObject
            {
                get { return touchedObject; }
                set { touchedObject = value; }
            }

            //Type of effect
            public PhysicsEffect physicsEffect;
        }

        #endregion

        #region Properties/Constructor

        public CircularGravity()
        {
            _forceTypeProperties = new ForceTypeProperties();
            _specialEffect = new SpecialEffect();
            _gameobjectFilter = new GameobjectFilter();
            _tagFilter = new TagFilter();
            _layerFilter = new LayerFilter();
            _triggerAreaFilter = new TriggerAreaFilter();
            _constraintProperties = new ConstraintProperties();
            _constraintProperties._gameobjectFilter = new GameobjectFilter();
            _constraintProperties._tagFilter = new TagFilter();
            _constraintProperties._layerFilter = new LayerFilter();
            _drawGravityProperties = new DrawGravityProperties();
        }

        //Used for when wanting to see the cgf line
        static private string CirularGravityLineName = "CirularGravityForce_LineDisplay";

        //Used for when creating the GameObject on an effected item
        static private string SpecialEffectGameObject = "CirularGravityForce_SpecialEffect";

        //Enable/Disable CircularGravity
        [SerializeField]
        private bool enable = true;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        //Shape of the CirularGravity
        [SerializeField]
        private Shape shape = CircularGravity.Shape.Sphere;
        public Shape _shape
        {
            get { return shape; }
            set { shape = value; }
        }

        //Force Type
        [SerializeField]
        private ForceType forceType = ForceType.ForceAtPosition;
        public ForceType _forceType
        {
            get { return forceType; }
            set { forceType = value; }
        }

        //Force Mode
        [SerializeField]
        private ForceMode forceMode = ForceMode.Force;
        public ForceMode _forceMode
        {
            get { return forceMode; }
            set { forceMode = value; }
        }

        //Radius of the force
        [SerializeField]
        private float size = 5f;
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        //Capsule Radius, used only when using the capsule shape
        [SerializeField]
        private float capsuleRadius = 2f;
        public float CapsuleRadius
        {
            get { return capsuleRadius; }
            set { capsuleRadius = value; }
        }

        //Power for the force, can be negative or positive
        [SerializeField]
        private float forcePower = 10f;
        public float ForcePower
        {
            get { return forcePower; }
            set { forcePower = value; }
        }

        //Force Type Properties
        [SerializeField]
        private ForceTypeProperties forceTypeProperties;
        public ForceTypeProperties _forceTypeProperties
        {
            get { return forceTypeProperties; }
            set { forceTypeProperties = value; }
        }

        //Constraint properties
        [SerializeField]
        private ConstraintProperties constraintProperties;
        public ConstraintProperties _constraintProperties
        {
            get { return constraintProperties; }
            set { constraintProperties = value; }
        }

        //Tag filter options
        [SerializeField]
        private GameobjectFilter gameobjectFilter;
        public GameobjectFilter _gameobjectFilter
        {
            get { return gameobjectFilter; }
            set { gameobjectFilter = value; }
        }

        //Tag filter options
        [SerializeField]
        private TagFilter tagFilter;
        public TagFilter _tagFilter
        {
            get { return tagFilter; }
            set { tagFilter = value; }
        }

        //Layer filter options
        [SerializeField]
        private LayerFilter layerFilter;
        public LayerFilter _layerFilter
        {
            get { return layerFilter; }
            set { layerFilter = value; }
        }

        //Trigger area filter options
        [SerializeField]
        private TriggerAreaFilter triggerAreaFilter;
        public TriggerAreaFilter _triggerAreaFilter
        {
            get { return triggerAreaFilter; }
            set { triggerAreaFilter = value; }
        }

        //Special effect options
        [SerializeField]
        private SpecialEffect specialEffect;
        public SpecialEffect _specialEffect
        {
            get { return specialEffect; }
            set { specialEffect = value; }
        }

        //Draw gravity properties
        [SerializeField]
        private DrawGravityProperties drawGravityProperties;
        public DrawGravityProperties _drawGravityProperties
        {
            get { return drawGravityProperties; }
            set { drawGravityProperties = value; }
        }

        //Line Object
        private GameObject cirularGravityLine;

        //Effected Rigidbodys
        IEnumerable<Rigidbody> rigidbodyList;

        #endregion

        #region Gizmos

        //Used for draying icons
        void OnDrawGizmos()
        {

#if (UNITY_EDITOR)
            string icon = "CircularGravityForce Icons/";
            icon = SetupIcons(icon);

            Gizmos.DrawIcon(this.transform.position, icon, true);

            if (CheckGameObjects())
            {
                if (!EditorApplication.isPlaying)
                {
                    _drawGravityProperties.DrawGravityForceGizmos = false;
                }
                else
                {
                    _drawGravityProperties.DrawGravityForceGizmos = true;
                }
            }
            else
            {
                _drawGravityProperties.DrawGravityForceGizmos = true;
            }
#endif
            if (_drawGravityProperties.DrawGravityForceGizmos)
            {
                DrawGravityForceGizmos();
            }
        }

#if (UNITY_EDITOR)
        bool CheckGameObjects()
        {
            if (Selection.activeGameObject == this.gameObject)
                return true;

            foreach (var item in Selection.gameObjects)
            {
                if (item == this.gameObject)
                    return true;
            }

            return false;
        }

        string SetupIcons(string icon)
        {
            string cgfDir = string.Format("{0}/CircularGravityForce Package/Gizmos/CircularGravityForce Icons/", Application.dataPath);
            string dir = string.Format("{0}/Gizmos/CircularGravityForce Icons/", Application.dataPath);

            if (!Directory.Exists(dir))
            {
                if (Directory.Exists(cgfDir))
                {
                    CopyIcons(cgfDir, dir);

                    AssetDatabase.Refresh();
                }
            }

            switch (_shape)
            {
                case Shape.Sphere:
                    icon = icon + "cgf_s_icon";
                    break;
                case Shape.Capsule:
                    icon = icon + "cgf_c_icon";
                    break;
                case Shape.RayCast:
                    icon = icon + "cgf_r_icon";
                    break;
            }

            if (forcePower == 0 || enable == false)
            {
                icon = icon + "0.png";
            }
            else if (forcePower >= 0)
            {
                icon = icon + "1.png";
            }
            else if (forcePower < 0)
            {
                icon = icon + "2.png";
            }

            return icon;
        }

        //Copys all cgf icons
        void CopyIcons(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir).Where(s => s.EndsWith(".png")))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
            }
        }
#endif

        #endregion

        #region Unity Events

        //Update is called once per frame
        void Update()
        {
            if (Enable)
            {
                //Sets up the line that gets rendered showing the area of forces
                if (_drawGravityProperties.DrawGravityForce)
                {
                    if (cirularGravityLine == null)
                    {
                        //Creates line for showing the force
                        cirularGravityLine = new GameObject(CirularGravityLineName);
                        cirularGravityLine.transform.SetParent(this.gameObject.transform, false);
                        cirularGravityLine.AddComponent<LineRenderer>();
                    }
                }
                else
                {
                    if (cirularGravityLine != null)
                    {
                        //Destroys line when not using
                        Destroy(cirularGravityLine);
                    }
                }
            }
            else
            {
                if (cirularGravityLine != null)
                {
                    //Destroys line when not using
                    Destroy(cirularGravityLine);
                }
            }
        }

        //Used for when drawing the cgf line with no lag
        void LateUpdate()
        {
            if (Enable)
            {
                //Sets up the line that gets rendered showing the area of forces
                if (_drawGravityProperties.DrawGravityForce)
                {
                    if (cirularGravityLine != null)
                    {
                        DrawGravityForceLineRenderer();
                    }
                }
            }
        }

        //This function is called every fixed frame
        void FixedUpdate()
        {
            if (Enable && forcePower != 0)
            {
                CalculateAndEstimateForce();
            }

            _specialEffect._effectedObjects.RefreshEffectedObjectListOverTime(_specialEffect.TimeEffected, _specialEffect.AttachedGameObject);
        }

        #endregion

        #region Functions

        //Applys the force function
        public void ApplyForce(Rigidbody rigid, Transform trans)
        {
            switch (_forceType)
            {
                case ForceType.ForceAtPosition:
                    rigid.AddForceAtPosition((rigid.gameObject.transform.position - trans.position) * ForcePower, trans.position, _forceMode);
                    break;
                case ForceType.Force:
                    rigid.AddForce(trans.forward * ForcePower, _forceMode);
                    break;
                case ForceType.Torque:
                    rigid.maxAngularVelocity = _forceTypeProperties.TorqueMaxAngularVelocity;
                    rigid.AddTorque(trans.right * ForcePower, _forceMode);
                    break;
                case ForceType.ExplosionForce:
                    rigid.AddExplosionForce(ForcePower, trans.position, Size, _forceTypeProperties.ExplosionForceUpwardsModifier, _forceMode);
                    break;
                case ForceType.GravitationalAttraction:
                    rigid.AddForce((rigid.gameObject.transform.position - trans.position).normalized * rigid.mass * ForcePower / (rigid.gameObject.transform.position - trans.position).sqrMagnitude, _forceMode);
                    break;
            }
        }

        //Calculate and Estimate the force
        private void CalculateAndEstimateForce()
        {
            if (_shape == Shape.Sphere)
            {
                #region Sphere

                rigidbodyList = Physics.OverlapSphere(this.transform.position, Size)
                    .Where(c => c.attachedRigidbody != null)
                    .Select(c => c.attachedRigidbody);

                if (rigidbodyList.Count() > 0)
                {
                    //Used for GameObject filtering
                    switch (_gameobjectFilter._gameobjectFilterOptions)
                    {
                        case GameobjectFilter.GameObjectFilterOptions.Disabled:
                            break;
                        case GameobjectFilter.GameObjectFilterOptions.OnlyEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => _gameobjectFilter.GameobjectList.Contains<GameObject>(r.gameObject));
                            break;
                        case GameobjectFilter.GameObjectFilterOptions.DontEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => !_gameobjectFilter.GameobjectList.Contains<GameObject>(r.gameObject));
                            break;
                    }

                    //Used for Tag filtering
                    switch (_tagFilter._tagFilterOptions)
                    {
                        case TagFilter.TagFilterOptions.Disabled:
                            break;
                        case TagFilter.TagFilterOptions.OnlyEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => _tagFilter.TagsList.Contains<string>(r.tag));
                            break;
                        case TagFilter.TagFilterOptions.DontEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => !_tagFilter.TagsList.Contains<string>(r.tag));
                            break;
                    }

                    //Used for Layer filtering
                    switch (_layerFilter._layerFilterOptions)
                    {
                        case LayerFilter.LayerFilterOptions.Disabled:
                            break;
                        case LayerFilter.LayerFilterOptions.OnlyEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => _layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.gameObject.layer)));
                            break;
                        case LayerFilter.LayerFilterOptions.DontEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => !_layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.gameObject.layer)));
                            break;
                    }

                    //Used for Trigger Area Filtering
                    switch (_triggerAreaFilter._triggerAreaFilterOptions)
                    {
                        case TriggerAreaFilter.TriggerAreaFilterOptions.Disabled:
                            break;
                        case TriggerAreaFilter.TriggerAreaFilterOptions.OnlyEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => _triggerAreaFilter.TriggerArea.bounds.Contains(r.transform.position));
                            break;
                        case TriggerAreaFilter.TriggerAreaFilterOptions.DontEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => !_triggerAreaFilter.TriggerArea.bounds.Contains(r.transform.position));
                            break;
                        default:
                            break;
                    }

                    if (_constraintProperties.AlignToForce)
                    {
                        ApplyAlignment(AlignToForceFilter(_constraintProperties, rigidbodyList));
                    }

                    foreach (var rigid in rigidbodyList)
                    {
                        SetupSpecialEffect(rigid);

                        ApplyForce(rigid, this.transform);
                    }
                }
                #endregion
            }
            else
            {
                #region RayCast

                //Circular Gravity Force Transform
                Transform cgfTran = this.transform;

                if (_shape == Shape.RayCast)
                {
                    rigidbodyList = Physics.RaycastAll(cgfTran.position, cgfTran.rotation * Vector3.forward, Size)
                        .Where(r => r.collider.attachedRigidbody != null)
                        .Select(r => r.collider.attachedRigidbody);
                }
                else if (_shape == Shape.Capsule)
                {
                    rigidbodyList = Physics.CapsuleCastAll
                    (cgfTran.position, cgfTran.position + ((cgfTran.rotation * Vector3.back)), capsuleRadius, cgfTran.position - ((cgfTran.position + (cgfTran.rotation * (Vector3.back)))), Size)
                        .Where(r => r.collider.attachedRigidbody != null)
                        .Select(r => r.collider.attachedRigidbody);
                }

                if (rigidbodyList.Count() > 0)
                {
                    //Used for GameObject filtering
                    switch (_gameobjectFilter._gameobjectFilterOptions)
                    {
                        case GameobjectFilter.GameObjectFilterOptions.Disabled:
                            break;
                        case GameobjectFilter.GameObjectFilterOptions.OnlyEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => _gameobjectFilter.GameobjectList.Contains<GameObject>(r.transform.gameObject));
                            break;
                        case GameobjectFilter.GameObjectFilterOptions.DontEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => !_gameobjectFilter.GameobjectList.Contains<GameObject>(r.transform.gameObject));
                            break;
                    }

                    //Used for Tag filtering
                    switch (_tagFilter._tagFilterOptions)
                    {
                        case TagFilter.TagFilterOptions.Disabled:
                            break;
                        case TagFilter.TagFilterOptions.OnlyEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => _tagFilter.TagsList.Contains<string>(r.transform.gameObject.tag));
                            break;
                        case TagFilter.TagFilterOptions.DontEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => !_tagFilter.TagsList.Contains<string>(r.transform.gameObject.tag));
                            break;
                    }

                    //Used for Layer filtering
                    switch (_layerFilter._layerFilterOptions)
                    {
                        case LayerFilter.LayerFilterOptions.Disabled:
                            break;
                        case LayerFilter.LayerFilterOptions.OnlyEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => _layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.transform.gameObject.layer)));
                            break;
                        case LayerFilter.LayerFilterOptions.DontEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => !_layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.transform.gameObject.layer)));
                            break;
                    }

                    //Used for Trigger Area Filtering
                    switch (_triggerAreaFilter._triggerAreaFilterOptions)
                    {
                        case TriggerAreaFilter.TriggerAreaFilterOptions.Disabled:
                            break;
                        case TriggerAreaFilter.TriggerAreaFilterOptions.OnlyEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => _triggerAreaFilter.TriggerArea.bounds.Contains(r.transform.position));
                            break;
                        case TriggerAreaFilter.TriggerAreaFilterOptions.DontEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => !_triggerAreaFilter.TriggerArea.bounds.Contains(r.transform.position));
                            break;
                        default:
                            break;
                    }

                    if (_constraintProperties.AlignToForce)
                    {
                        ApplyAlignment(AlignToForceFilter(_constraintProperties, rigidbodyList));
                    }

                    foreach (var rigid in rigidbodyList)
                    {
                        SetupSpecialEffect(rigid);

                        ApplyForce(rigid, cgfTran);
                    }
                }

                #endregion
            }
        }

        //Applies where filters for alignment
        public IEnumerable<Rigidbody> AlignToForceFilter(ConstraintProperties _constraintProperties, IEnumerable<Rigidbody> list)
        {
            switch (_constraintProperties._gameobjectFilter._gameobjectFilterOptions)
            {
                case GameobjectFilter.GameObjectFilterOptions.Disabled:
                    break;
                case GameobjectFilter.GameObjectFilterOptions.OnlyEffectListedGameobjects:
                    list = list.Where(r => _constraintProperties._gameobjectFilter.GameobjectList.Contains<GameObject>(r.transform.gameObject));
                    break;
                case GameobjectFilter.GameObjectFilterOptions.DontEffectListedGameobjects:
                    list = list.Where(r => !_constraintProperties._gameobjectFilter.GameobjectList.Contains<GameObject>(r.transform.gameObject));
                    break;
                default:
                    break;
            }

            switch (_constraintProperties._tagFilter._tagFilterOptions)
            {
                case TagFilter.TagFilterOptions.Disabled:
                    break;
                case TagFilter.TagFilterOptions.OnlyEffectListedTags:
                    list = list.Where(r => _constraintProperties._tagFilter.TagsList.Contains<string>(r.transform.gameObject.tag));
                    break;
                case TagFilter.TagFilterOptions.DontEffectListedTags:
                    list = list.Where(r => !_constraintProperties._tagFilter.TagsList.Contains<string>(r.transform.gameObject.tag));
                    break;
                default:
                    break;
            }

            switch (_constraintProperties._layerFilter._layerFilterOptions)
            {
                case LayerFilter.LayerFilterOptions.Disabled:
                    break;
                case LayerFilter.LayerFilterOptions.OnlyEffectListedLayers:
                    list = list.Where(r => _constraintProperties._layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.transform.gameObject.layer)));
                    break;
                case LayerFilter.LayerFilterOptions.DontEffectListedLayers:
                    list = list.Where(r => !_constraintProperties._layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.transform.gameObject.layer)));
                    break;
                default:
                    break;
            }

            return list;
        }

        //Applys the alignment of the listed game objects
        void ApplyAlignment(IEnumerable<Rigidbody> list)
        {
            foreach (var item in list)
            {
                Vector3 up = (item.transform.position - this.transform.position).normalized;
                Vector3 localUp = item.transform.up;

                Quaternion targetRotation = Quaternion.FromToRotation(localUp, up) * item.rotation;
                item.MoveRotation(Quaternion.Slerp(item.rotation, targetRotation, Time.deltaTime * _constraintProperties.SlerpSpeed));
            }
        }

        //Sets up the special effects
        private void SetupSpecialEffect(Rigidbody rigidbody)
        {
            switch (_specialEffect._physicsEffect)
            {
                case PhysicsEffect.None:
                    break;
                case PhysicsEffect.NoGravity:
                    rigidbody.useGravity = false;
                    break;
                default:
                    break;
            }

            if (_specialEffect.TimeEffected > 0)
            {
                _specialEffect._effectedObjects.AddedEffectedObject(rigidbody, _specialEffect._physicsEffect, _specialEffect.AttachedGameObject);
            }
        }

        #endregion

        #region Draw

        //Draws effected area by forces line renderer
        private void DrawGravityForceLineRenderer()
        {
            //Circular Gravity Force Transform
            Transform cgfTran = this.transform;

            Color DebugGravityLineColor;

            if (Enable)
            {
                if (forcePower == 0)
                    DebugGravityLineColor = Color.white;
                else if (forcePower > 0)
                    DebugGravityLineColor = Color.green;
                else
                    DebugGravityLineColor = Color.red;
            }
            else
            {
                DebugGravityLineColor = Color.white;
            }

            //Line setup
            LineRenderer lineRenderer = cirularGravityLine.GetComponent<LineRenderer>();
            lineRenderer.SetWidth(_drawGravityProperties.Thickness, _drawGravityProperties.Thickness);
            lineRenderer.material = _drawGravityProperties.GravityLineMaterial;
            lineRenderer.SetColors(DebugGravityLineColor, DebugGravityLineColor);

            //Renders type outline
            switch (_shape)
            {
                case Shape.Sphere:

                    //Models line
                    lineRenderer.SetVertexCount(12);

                    lineRenderer.SetPosition(0, cgfTran.position + ((cgfTran.rotation * Vector3.up) * Size));
                    lineRenderer.SetPosition(1, cgfTran.position);
                    lineRenderer.SetPosition(2, cgfTran.position + ((cgfTran.rotation * Vector3.down) * Size));
                    lineRenderer.SetPosition(3, cgfTran.position);
                    lineRenderer.SetPosition(4, cgfTran.position + ((cgfTran.rotation * Vector3.left) * Size));
                    lineRenderer.SetPosition(5, cgfTran.position);
                    lineRenderer.SetPosition(6, cgfTran.position + ((cgfTran.rotation * Vector3.right) * Size));
                    lineRenderer.SetPosition(7, cgfTran.position);
                    lineRenderer.SetPosition(8, cgfTran.position + ((cgfTran.rotation * Vector3.forward) * Size));
                    lineRenderer.SetPosition(9, cgfTran.position);
                    lineRenderer.SetPosition(10, cgfTran.position + ((cgfTran.rotation * Vector3.back) * Size));
                    lineRenderer.SetPosition(11, cgfTran.position);

                    break;

                case Shape.Capsule:

                    //Model Capsule
                    lineRenderer.SetVertexCount(17);

                    //Starting Point
                    lineRenderer.SetPosition(0, cgfTran.position + ((cgfTran.rotation * Vector3.up) * capsuleRadius));
                    lineRenderer.SetPosition(1, cgfTran.position);
                    lineRenderer.SetPosition(2, cgfTran.position + ((cgfTran.rotation * Vector3.down) * capsuleRadius));
                    lineRenderer.SetPosition(3, cgfTran.position);
                    lineRenderer.SetPosition(4, cgfTran.position + ((cgfTran.rotation * Vector3.left) * capsuleRadius));
                    lineRenderer.SetPosition(5, cgfTran.position);
                    lineRenderer.SetPosition(6, cgfTran.position + ((cgfTran.rotation * Vector3.right) * capsuleRadius));
                    lineRenderer.SetPosition(7, cgfTran.position);

                    //Middle Line
                    Vector3 endPointLoc = cgfTran.position + ((cgfTran.rotation * Vector3.forward) * Size);
                    lineRenderer.SetPosition(8, endPointLoc);

                    //End Point
                    lineRenderer.SetPosition(9, endPointLoc + ((cgfTran.rotation * Vector3.up) * capsuleRadius));
                    lineRenderer.SetPosition(10, endPointLoc);
                    lineRenderer.SetPosition(11, endPointLoc + ((cgfTran.rotation * Vector3.down) * capsuleRadius));
                    lineRenderer.SetPosition(12, endPointLoc);
                    lineRenderer.SetPosition(13, endPointLoc + ((cgfTran.rotation * Vector3.left) * capsuleRadius));
                    lineRenderer.SetPosition(14, endPointLoc);
                    lineRenderer.SetPosition(15, endPointLoc + ((cgfTran.rotation * Vector3.right) * capsuleRadius));
                    lineRenderer.SetPosition(16, endPointLoc);

                    break;

                case Shape.RayCast:

                    //Model Line
                    lineRenderer.SetVertexCount(2);

                    lineRenderer.SetPosition(0, cgfTran.position);
                    lineRenderer.SetPosition(1, cgfTran.position + ((cgfTran.rotation * Vector3.forward) * Size));

                    break;
            }
        }

        //Draws effected area by forces with debug draw line, so you can see it in Gizmos
        private void DrawGravityForceGizmos()
        {
            //Circular Gravity Force Transform
            Transform cgfTran = this.transform;

            Color DebugGravityLineColorA;
            Color DebugGravityLineColorB;

            if (Enable)
            {
                if (forcePower == 0)
                {
                    DebugGravityLineColorA = Color.white;
                    DebugGravityLineColorB = Color.white;
                }
                else if (forcePower > 0)
                {
                    DebugGravityLineColorA = Color.green;
                    DebugGravityLineColorB = Color.green;
                }
                else
                {
                    DebugGravityLineColorA = Color.red;
                    DebugGravityLineColorB = Color.red;
                }
            }
            else
            {
                DebugGravityLineColorA = Color.white;
                DebugGravityLineColorB = Color.white;
            }

            DebugGravityLineColorA.a = .5f;
            DebugGravityLineColorB.a = .1f;

            //Renders type outline
            switch (_shape)
            {
                case CircularGravity.Shape.Sphere:

                    Gizmos.color = DebugGravityLineColorB;
                    Gizmos.DrawSphere(cgfTran.position, Size);

                    Gizmos.color = DebugGravityLineColorA;
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.up) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.down) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.left) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.right) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.forward) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.back) * Size), cgfTran.position);

                    break;

                case CircularGravity.Shape.Capsule:

                    Gizmos.color = DebugGravityLineColorA;
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.up) * capsuleRadius), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.down) * capsuleRadius), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.left) * capsuleRadius), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.right) * capsuleRadius), cgfTran.position);

                    Vector3 endPointLoc = cgfTran.position + ((cgfTran.rotation * Vector3.forward) * Size);

                    Gizmos.DrawLine(cgfTran.position, endPointLoc);

                    Gizmos.DrawLine(endPointLoc + ((cgfTran.rotation * Vector3.up) * capsuleRadius), endPointLoc);
                    Gizmos.DrawLine(endPointLoc + ((cgfTran.rotation * Vector3.down) * capsuleRadius), endPointLoc);
                    Gizmos.DrawLine(endPointLoc + ((cgfTran.rotation * Vector3.left) * capsuleRadius), endPointLoc);
                    Gizmos.DrawLine(endPointLoc + ((cgfTran.rotation * Vector3.right) * capsuleRadius), endPointLoc);

                    break;

                case CircularGravity.Shape.RayCast:

                    Gizmos.color = DebugGravityLineColorA;
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.forward) * Size), cgfTran.position);

                    break;
            }
        }

        #endregion
    }
}