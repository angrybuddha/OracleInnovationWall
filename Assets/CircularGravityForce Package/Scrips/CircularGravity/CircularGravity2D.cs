/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 04-29-15
* Last Updated: 06-15-15
* Description: Core logic for Circular Gravity Force for 2D.
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
    public class CircularGravity2D : MonoBehaviour
    {
        #region Enums

        //Force Types
        public enum ForceType2D
        {
            ForceAtPosition,
            Force,
            Torque,
            GravitationalAttraction
        }

        //Force Types
        public enum Shape2D
        {
            Sphere,
            RayCast,
        }

        #endregion

        #region Classes

        //Manages all effected objects for when using special effect
        public class EffectedObjects2D
        {
            //List of all EffectedObject
            private List<EffectedObject2D> effectedObjectList;
            public List<EffectedObject2D> EffectedObjectList
            {
                get { return effectedObjectList; }
                set { effectedObjectList = value; }
            }

            //Constructor
            public EffectedObjects2D()
            {
                EffectedObjectList = new List<EffectedObject2D>();
            }

            //Used to add to the effectedObjectList
            public void AddedEffectedObject2D(Rigidbody2D touchedObject, CircularGravity.PhysicsEffect physicsEffect, GameObject attachedGameObject)
            {
                if (EffectedObjectList.Count == 0)
                {
                    EffectedObject2D effectedObject = new EffectedObject2D(Time.time, touchedObject, physicsEffect);
                    EffectedObjectList.Add(effectedObject);

                    CreateAttachedGameObject2D(touchedObject, attachedGameObject);
                }
                else if ((EffectedObjectList.Count > 0))
                {
                    bool checkIfExists = false;

                    foreach (EffectedObject2D item in EffectedObjectList)
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
                        EffectedObject2D effectedObject = new EffectedObject2D(Time.time, touchedObject, physicsEffect);
                        EffectedObjectList.Add(effectedObject);

                        CreateAttachedGameObject2D(touchedObject, attachedGameObject);
                    }
                }
            }

            //Creates the attached gameobject for the effect
            private static void CreateAttachedGameObject2D(Rigidbody2D touchedObject, GameObject attachGameObject)
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
            private static void RemoveAttachedGameObject2D(Rigidbody2D touchedObject, GameObject attachGameObject)
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
                    List<EffectedObject2D> removeItems = new List<EffectedObject2D>();

                    foreach (EffectedObject2D item in EffectedObjectList)
                    {
                        if (item.EffectedTime + timer < Time.time)
                        {
                            switch (item.PhysicsEffect)
                            {
                                case CircularGravity.PhysicsEffect.None:
                                    break;
                                case CircularGravity.PhysicsEffect.NoGravity:
                                    if (item.TouchedObject != null)
                                    {
                                        item.TouchedObject.gravityScale = 1f;
                                    }
                                    break;
                            }

                            RemoveAttachedGameObject2D(item.TouchedObject, attachedGameObject);
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

        //Manages all effected objects for when using special effect in 2D
        [System.Serializable]
        public class SpecialEffect2D
        {
            //Physics Effect
            [SerializeField]
            private CircularGravity.PhysicsEffect physicsEffect = CircularGravity.PhysicsEffect.None;
            public CircularGravity.PhysicsEffect _physicsEffect
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
            private EffectedObjects2D effectedObjects;
            public EffectedObjects2D _effectedObjects
            {
                get { return effectedObjects; }
                set { effectedObjects = value; }
            }

            //Constructor
            public SpecialEffect2D()
            {
                _effectedObjects = new EffectedObjects2D();
            }
        }

        //Trigger Area Filter
        [System.Serializable]
        public class TriggerAreaFilter2D
        {
            //Trigger Options
            public enum TriggerAreaFilterOptions2D
            {
                Disabled,
                OnlyEffectWithinTigger,
                DontEffectWithinTigger,
            }

            //Trigger area filter options
            [SerializeField]
            private TriggerAreaFilterOptions2D triggerAreaFilterOptions = TriggerAreaFilterOptions2D.Disabled;
            public TriggerAreaFilterOptions2D _triggerAreaFilterOptions
            {
                get { return triggerAreaFilterOptions; }
                set { triggerAreaFilterOptions = value; }
            }

            //Trigger Object
            [SerializeField]
            private Collider2D triggerArea;
            public Collider2D TriggerArea
            {
                get { return triggerArea; }
                set { triggerArea = value; }
            }
        }

        //Data structure for effected object
        public class EffectedObject2D
        {
            public EffectedObject2D()
            {
            }

            public EffectedObject2D(float effectedTime, Rigidbody2D touchedObject, CircularGravity.PhysicsEffect physicsEffect)
            {
                this.EffectedTime = effectedTime;
                this.TouchedObject = touchedObject;
                this.PhysicsEffect = physicsEffect;
            }

            //Time Effected
            private float effectedTime;
            public float EffectedTime
            {
                get { return effectedTime; }
                set { effectedTime = value; }
            }

            //Rigidbody of touched object
            private Rigidbody2D touchedObject;
            public Rigidbody2D TouchedObject
            {
                get { return touchedObject; }
                set { touchedObject = value; }
            }

            //Type of effect
            private CircularGravity.PhysicsEffect physicsEffect;
            public CircularGravity.PhysicsEffect PhysicsEffect
            {
                get { return physicsEffect; }
                set { physicsEffect = value; }
            }
        }

        #endregion

        #region Properties/Constructor

        public CircularGravity2D()
        {
            _specialEffect = new SpecialEffect2D();
            _gameobjectFilter = new CircularGravity.GameobjectFilter();
            _tagFilter = new CircularGravity.TagFilter();
            _layerFilter = new CircularGravity.LayerFilter();
            _triggerAreaFilter = new TriggerAreaFilter2D();
            _constraintProperties = new CircularGravity.ConstraintProperties();
            _constraintProperties._gameobjectFilter = new CircularGravity.GameobjectFilter();
            _constraintProperties._tagFilter = new CircularGravity.TagFilter();
            _constraintProperties._layerFilter = new CircularGravity.LayerFilter();
            _drawGravityProperties = new CircularGravity.DrawGravityProperties();
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
        private Shape2D shape2D = Shape2D.Sphere;
        public Shape2D _shape2D
        {
            get { return shape2D; }
            set { shape2D = value; }
        }

        //Force Type
        [SerializeField]
        private ForceType2D forceType2D = ForceType2D.ForceAtPosition;
        public ForceType2D _forceType2D
        {
            get { return forceType2D; }
            set { forceType2D = value; }
        }

        //Force Mode
        [SerializeField]
        private ForceMode2D forceMode2D = ForceMode2D.Force;
        public ForceMode2D _forceMode2D
        {
            get { return forceMode2D; }
            set { forceMode2D = value; }
        }

        //Radius of the force
        [SerializeField]
        private float size = 10f;
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        //Power for the force, can be negative or positive
        [SerializeField]
        private float forcePower = 10f;
        public float ForcePower
        {
            get { return forcePower; }
            set { forcePower = value; }
        }

        //Constraint properties
        [SerializeField]
        private CircularGravity.ConstraintProperties constraintProperties;
        public CircularGravity.ConstraintProperties _constraintProperties
        {
            get { return constraintProperties; }
            set { constraintProperties = value; }
        }

        //Tag filter options
        [SerializeField]
        private CircularGravity.GameobjectFilter gameobjectFilter;
        public CircularGravity.GameobjectFilter _gameobjectFilter
        {
            get { return gameobjectFilter; }
            set { gameobjectFilter = value; }
        }

        //Tag filter options
        [SerializeField]
        private CircularGravity.TagFilter tagFilter;
        public CircularGravity.TagFilter _tagFilter
        {
            get { return tagFilter; }
            set { tagFilter = value; }
        }

        //Layer filter options
        [SerializeField]
        private CircularGravity.LayerFilter layerFilter;
        public CircularGravity.LayerFilter _layerFilter
        {
            get { return layerFilter; }
            set { layerFilter = value; }
        }

        //Trigger area filter options
        [SerializeField]
        private TriggerAreaFilter2D triggerAreaFilter;
        public TriggerAreaFilter2D _triggerAreaFilter
        {
            get { return triggerAreaFilter; }
            set { triggerAreaFilter = value; }
        }

        //Special effect options
        [SerializeField]
        private SpecialEffect2D specialEffect;
        public SpecialEffect2D _specialEffect
        {
            get { return specialEffect; }
            set { specialEffect = value; }
        }

        //Draw gravity properties
        [SerializeField]
        private CircularGravity.DrawGravityProperties drawGravityProperties;
        public CircularGravity.DrawGravityProperties _drawGravityProperties
        {
            get { return drawGravityProperties; }
            set { drawGravityProperties = value; }
        }

        //Line Object
        private GameObject cirularGravityLine;

        //Effected Rigidbodys
        IEnumerable<Rigidbody2D> rigidbodyList;

        #endregion

        #region Gizmos

        //Used for draying icons
        void OnDrawGizmos()
        {
#if (UNITY_EDITOR)
            string icon = "CircularGravityForce Icons/";
            icon = SetupIcons(icon);

            Gizmos.DrawIcon(this.transform.position, icon, true);

            if (!EditorApplication.isPlaying)
            {
                if (CheckGameObjects())
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

            switch (_shape2D)
            {
                case Shape2D.Sphere:
                    icon = icon + "cgf_s_icon";
                    break;
                case Shape2D.RayCast:
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
            else if (ForcePower < 0)
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
            if (Enable && ForcePower != 0)
            {
                CalculateAndEstimateForce();
            }

            _specialEffect._effectedObjects.RefreshEffectedObjectListOverTime(_specialEffect.TimeEffected, _specialEffect.AttachedGameObject);
        }

        #endregion

        #region Functions

        //Applys the force function
        public void ApplyForce(Rigidbody2D rigid, Transform trans)
        {
            switch (_forceType2D)
            {
                case ForceType2D.ForceAtPosition:
                    rigid.AddForceAtPosition((rigid.gameObject.transform.position - trans.position) * ForcePower, trans.position, _forceMode2D);
                    break;
                case ForceType2D.Force:
                    rigid.AddForce(trans.right * ForcePower, _forceMode2D);
                    break;
                case ForceType2D.Torque:
                    rigid.AddTorque(-ForcePower, _forceMode2D);
                    break;
                case ForceType2D.GravitationalAttraction:
                    rigid.AddForce((rigid.gameObject.transform.position - trans.position).normalized * rigid.mass * ForcePower / (rigid.gameObject.transform.position - trans.position).sqrMagnitude, _forceMode2D);
                    break;
            }
        }

        //Calculate and Estimate the force
        private void CalculateAndEstimateForce()
        {
            if (_shape2D == Shape2D.Sphere)
            {
                #region Sphere

                rigidbodyList = Physics2D.OverlapCircleAll(this.transform.position, Size)
                    .Where(c => c.attachedRigidbody != null)
                    .Select(c => c.attachedRigidbody);

                if (rigidbodyList.Count() > 0)
                {
                    //Used for GameObject filtering
                    switch (_gameobjectFilter._gameobjectFilterOptions)
                    {
                        case CircularGravity.GameobjectFilter.GameObjectFilterOptions.Disabled:
                            break;
                        case CircularGravity.GameobjectFilter.GameObjectFilterOptions.OnlyEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => _gameobjectFilter.GameobjectList.Contains<GameObject>(r.gameObject));
                            break;
                        case CircularGravity.GameobjectFilter.GameObjectFilterOptions.DontEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => !_gameobjectFilter.GameobjectList.Contains<GameObject>(r.gameObject));
                            break;
                    }

                    //Used for Tag filtering
                    switch (_tagFilter._tagFilterOptions)
                    {
                        case CircularGravity.TagFilter.TagFilterOptions.Disabled:
                            break;
                        case CircularGravity.TagFilter.TagFilterOptions.OnlyEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => _tagFilter.TagsList.Contains<string>(r.tag));
                            break;
                        case CircularGravity.TagFilter.TagFilterOptions.DontEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => !_tagFilter.TagsList.Contains<string>(r.tag));
                            break;
                    }

                    //Used for Layer filtering
                    switch (_layerFilter._layerFilterOptions)
                    {
                        case CircularGravity.LayerFilter.LayerFilterOptions.Disabled:
                            break;
                        case CircularGravity.LayerFilter.LayerFilterOptions.OnlyEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => _layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.gameObject.layer)));
                            break;
                        case CircularGravity.LayerFilter.LayerFilterOptions.DontEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => !_layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.gameObject.layer)));
                            break;
                    }

                    //Used for Trigger Area Filtering
                    switch (_triggerAreaFilter._triggerAreaFilterOptions)
                    {
                        case TriggerAreaFilter2D.TriggerAreaFilterOptions2D.Disabled:
                            break;
                        case TriggerAreaFilter2D.TriggerAreaFilterOptions2D.OnlyEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => _triggerAreaFilter.TriggerArea.bounds.Contains((Vector2)r.transform.position));
                            break;
                        case TriggerAreaFilter2D.TriggerAreaFilterOptions2D.DontEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => !_triggerAreaFilter.TriggerArea.bounds.Contains((Vector2)r.transform.position));
                            break;
                        default:
                            break;
                    }

                    if (_constraintProperties.AlignToForce)
                    {
                        ApplyAlignment2D(AlignToForceFilter2D(_constraintProperties, rigidbodyList));
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

                if (_shape2D == Shape2D.RayCast)
                {
                    rigidbodyList = Physics2D.RaycastAll(cgfTran.position, cgfTran.rotation * Vector3.right, Size)
                        .Where(r => r.collider.attachedRigidbody != null)
                        .Select(r => r.collider.attachedRigidbody);
                }

                if (rigidbodyList.Count() > 0)
                {
                    //Used for GameObject filtering
                    switch (_gameobjectFilter._gameobjectFilterOptions)
                    {
                        case CircularGravity.GameobjectFilter.GameObjectFilterOptions.Disabled:
                            break;
                        case CircularGravity.GameobjectFilter.GameObjectFilterOptions.OnlyEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => _gameobjectFilter.GameobjectList.Contains<GameObject>(r.transform.gameObject));
                            break;
                        case CircularGravity.GameobjectFilter.GameObjectFilterOptions.DontEffectListedGameobjects:
                            rigidbodyList = rigidbodyList.Where(r => !_gameobjectFilter.GameobjectList.Contains<GameObject>(r.transform.gameObject));
                            break;
                    }

                    //Used for Tag filtering
                    switch (_tagFilter._tagFilterOptions)
                    {
                        case CircularGravity.TagFilter.TagFilterOptions.Disabled:
                            break;
                        case CircularGravity.TagFilter.TagFilterOptions.OnlyEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => _tagFilter.TagsList.Contains<string>(r.transform.gameObject.tag));
                            break;
                        case CircularGravity.TagFilter.TagFilterOptions.DontEffectListedTags:
                            rigidbodyList = rigidbodyList.Where(r => !_tagFilter.TagsList.Contains<string>(r.transform.gameObject.tag));
                            break;
                    }

                    //Used for Layer filtering
                    switch (_layerFilter._layerFilterOptions)
                    {
                        case CircularGravity.LayerFilter.LayerFilterOptions.Disabled:
                            break;
                        case CircularGravity.LayerFilter.LayerFilterOptions.OnlyEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => _layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.transform.gameObject.layer)));
                            break;
                        case CircularGravity.LayerFilter.LayerFilterOptions.DontEffectListedLayers:
                            rigidbodyList = rigidbodyList.Where(r => !_layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(r.transform.gameObject.layer)));
                            break;
                    }

                    //Used for Trigger Area Filtering
                    switch (_triggerAreaFilter._triggerAreaFilterOptions)
                    {
                        case TriggerAreaFilter2D.TriggerAreaFilterOptions2D.Disabled:
                            break;
                        case TriggerAreaFilter2D.TriggerAreaFilterOptions2D.OnlyEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => _triggerAreaFilter.TriggerArea.bounds.Contains(r.transform.position));
                            break;
                        case TriggerAreaFilter2D.TriggerAreaFilterOptions2D.DontEffectWithinTigger:
                            rigidbodyList = rigidbodyList.Where(r => !_triggerAreaFilter.TriggerArea.bounds.Contains(r.transform.position));
                            break;
                        default:
                            break;
                    }

                    if (_constraintProperties.AlignToForce)
                    {
                        ApplyAlignment2D(AlignToForceFilter2D(_constraintProperties, rigidbodyList));
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
        public IEnumerable<Rigidbody2D> AlignToForceFilter2D(CircularGravityForce.CircularGravity.ConstraintProperties _constraintProperties, IEnumerable<Rigidbody2D> list)
        {
            switch (_constraintProperties._gameobjectFilter._gameobjectFilterOptions)
            {
                case CircularGravityForce.CircularGravity.GameobjectFilter.GameObjectFilterOptions.Disabled:
                    break;
                case CircularGravityForce.CircularGravity.GameobjectFilter.GameObjectFilterOptions.OnlyEffectListedGameobjects:
                    list = list.Where(g => _constraintProperties._gameobjectFilter.GameobjectList.Contains<GameObject>(g.transform.gameObject));
                    break;
                case CircularGravityForce.CircularGravity.GameobjectFilter.GameObjectFilterOptions.DontEffectListedGameobjects:
                    list = list.Where(g => !_constraintProperties._gameobjectFilter.GameobjectList.Contains<GameObject>(g.transform.gameObject));
                    break;
                default:
                    break;
            }

            switch (_constraintProperties._tagFilter._tagFilterOptions)
            {
                case CircularGravityForce.CircularGravity.TagFilter.TagFilterOptions.Disabled:
                    break;
                case CircularGravityForce.CircularGravity.TagFilter.TagFilterOptions.OnlyEffectListedTags:
                    list = list.Where(g => _constraintProperties._tagFilter.TagsList.Contains<string>(g.transform.gameObject.tag));
                    break;
                case CircularGravityForce.CircularGravity.TagFilter.TagFilterOptions.DontEffectListedTags:
                    list = list.Where(g => !_constraintProperties._tagFilter.TagsList.Contains<string>(g.transform.gameObject.tag));
                    break;
                default:
                    break;
            }

            switch (_constraintProperties._layerFilter._layerFilterOptions)
            {
                case CircularGravityForce.CircularGravity.LayerFilter.LayerFilterOptions.Disabled:
                    break;
                case CircularGravityForce.CircularGravity.LayerFilter.LayerFilterOptions.OnlyEffectListedLayers:
                    list = list.Where(g => _constraintProperties._layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(g.transform.gameObject.layer)));
                    break;
                case CircularGravityForce.CircularGravity.LayerFilter.LayerFilterOptions.DontEffectListedLayers:
                    list = list.Where(g => !_constraintProperties._layerFilter.LayerList.Contains<string>(LayerMask.LayerToName(g.transform.gameObject.layer)));
                    break;
                default:
                    break;
            }

            return list;
        }

        //Applys the alignment of the listed game objects
        void ApplyAlignment2D(IEnumerable<Rigidbody2D> list)
        {
            foreach (var item in list)
            {
                if ((item.transform.position - this.transform.position) != Vector3.zero)
                {
                    var newRotation = Quaternion.LookRotation((item.transform.position - this.transform.position).normalized, Vector3.back);

                    newRotation.x = 0.0f;
                    newRotation.y = 0.0f;

                    item.transform.rotation = Quaternion.Slerp(item.transform.rotation, newRotation, Time.deltaTime * _constraintProperties.SlerpSpeed);
                }
            }
        }

        //Sets up the special effects
        private void SetupSpecialEffect(Rigidbody2D rigidbody)
        {
            switch (_specialEffect._physicsEffect)
            {
                case CircularGravity.PhysicsEffect.None:
                    break;
                case CircularGravity.PhysicsEffect.NoGravity:
                    rigidbody.gravityScale = 0f;
                    break;
                default:
                    break;
            }

            if (_specialEffect.TimeEffected > 0)
            {
                _specialEffect._effectedObjects.AddedEffectedObject2D(rigidbody, _specialEffect._physicsEffect, _specialEffect.AttachedGameObject);
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
                if (ForcePower == 0)
                    DebugGravityLineColor = Color.white;
                else if (ForcePower > 0)
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
            switch (_shape2D)
            {
                case Shape2D.Sphere:

                    //Models line
                    lineRenderer.SetVertexCount(8);

                    lineRenderer.SetPosition(0, cgfTran.position + ((cgfTran.rotation * Vector3.up) * Size));
                    lineRenderer.SetPosition(1, cgfTran.position);
                    lineRenderer.SetPosition(2, cgfTran.position + ((cgfTran.rotation * Vector3.down) * Size));
                    lineRenderer.SetPosition(3, cgfTran.position);
                    lineRenderer.SetPosition(4, cgfTran.position + ((cgfTran.rotation * Vector3.left) * Size));
                    lineRenderer.SetPosition(5, cgfTran.position);
                    lineRenderer.SetPosition(6, cgfTran.position + ((cgfTran.rotation * Vector3.right) * Size));
                    lineRenderer.SetPosition(7, cgfTran.position);

                    break;

                case Shape2D.RayCast:

                    //Model Line
                    lineRenderer.SetVertexCount(2);

                    lineRenderer.SetPosition(0, cgfTran.position);
                    lineRenderer.SetPosition(1, cgfTran.position + ((cgfTran.rotation * Vector3.right) * Size));

                    break;
            }
        }

        //Draws effected area by forces with debug draw line, so you can see it in Gizmos
        private void DrawGravityForceGizmos()
        {
            //Circular Gravity Force Transform
            Transform cgfTran = this.transform;

            Color DebugGravityLineColor;

            if (Enable)
            {
                if (ForcePower == 0)
                    DebugGravityLineColor = Color.white;
                else if (ForcePower > 0)
                    DebugGravityLineColor = Color.green;
                else
                    DebugGravityLineColor = Color.red;
            }
            else
            {
                DebugGravityLineColor = Color.white;
            }

            Gizmos.color = DebugGravityLineColor;

            //Renders type outline
            switch (_shape2D)
            {
                case Shape2D.Sphere:

                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.up) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.down) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.left) * Size), cgfTran.position);
                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.right) * Size), cgfTran.position);

                    break;

                case Shape2D.RayCast:

                    Gizmos.DrawLine(cgfTran.position + ((cgfTran.rotation * Vector3.right) * Size), cgfTran.position);

                    break;
            }
        }

        #endregion
    }
}