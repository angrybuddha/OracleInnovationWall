/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for animating/managing the main cube.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Linq;

using CircularGravityForce;
using ParticlePlayground;
using TMPro;
using System.Collections.Generic;

public class Cubie : MonoBehaviour
{
    #region Properties

    //Slogan text
    public const string slogan = "Approach the screen to share your collaborative thoughts";

    //Enable Scatter plot flag
    [SerializeField, Header("Settings: ")]
    private bool enableScatter = false;
    public bool EnableScatter
    {
        get { return enableScatter; }
        set { enableScatter = value; }
    }

    //Cubies speacj text
    [SerializeField]
    private TextMeshProUGUI speachText;
    public TextMeshProUGUI SpeachText
    {
        get { return speachText; }
        set { speachText = value; }
    }

    //Cubie particles
    [SerializeField, Header("Particles: ")]
    private PlaygroundParticlesC ppc;
    public PlaygroundParticlesC Ppc
    {
        get { return ppc; }
        set { ppc = value; }
    }

    //Core cubie particles
    [SerializeField]
    private PlaygroundParticlesC ppc_Core;
    public PlaygroundParticlesC Ppc_Core
    {
        get { return ppc_Core; }
        set { ppc_Core = value; }
    }

    //Explotion cubie particles
    [SerializeField]
    private PlaygroundParticlesC ppc_Explode;
    public PlaygroundParticlesC Ppc_Explode
    {
        get { return ppc_Explode; }
        set { ppc_Explode = value; }
    }

    //Force picot point
    [SerializeField, Header("Forces: ")]
    private Transform forcePivot;
    public Transform ForcePivot
    {
        get { return forcePivot; }
        set { forcePivot = value; }
    }

    //Torque Pivot
    [SerializeField]
    private Transform torquePivot;
    public Transform TorquePivot
    {
        get { return torquePivot; }
        set { torquePivot = value; }
    }

    //Controls physics force 
    [SerializeField]
    private CircularGravity cgfForce;
    public CircularGravity CgfForce
    {
        get { return cgfForce; }
        set { cgfForce = value; }
    }

    //Controls physics torque 
    [SerializeField]
    private CircularGravity cgfTorque;
    public CircularGravity CgfTorque
    {
        get { return cgfTorque; }
        set { cgfTorque = value; }
    }

    //Controls physics hovering 
    [SerializeField]
    private CircularGravity cgfHover;
    public CircularGravity CgfHover
    {
        get { return cgfHover; }
        set { cgfHover = value; }
    }

    //Rotation slerp speed
    [SerializeField]
    private float rotationSlerpSpeed = 4f;
    public float RotationSlerpSpeed
    {
        get { return rotationSlerpSpeed; }
        set { rotationSlerpSpeed = value; }
    }

    //Used moving to idle active location
    [SerializeField, Header("Cubie Targets Settings:")]
    private Transform idleActiveLocation;
    public Transform IdleActiveLocation
    {
        get { return idleActiveLocation; }
        set { idleActiveLocation = value; }
    }

    //Used moving to idle location
    [SerializeField]
    private List<Transform> idleLocation;
    public List<Transform> IdleLocation
    {
        get { return idleLocation; }
        set { idleLocation = value; }
    }

    //Used moving to idle active location
    [SerializeField]
    private Transform idleNonActiveLocation;
    public Transform IdleNonActiveLocation
    {
        get { return idleNonActiveLocation; }
        set { idleNonActiveLocation = value; }
    }

    //Used moving to idle explode location
    [SerializeField]
    private Transform idleExplodeLocation;
    public Transform IdleExplodeLocation
    {
        get { return idleExplodeLocation; }
        set { idleExplodeLocation = value; }
    }

    //Min cubes target distance
    [SerializeField]
    private float minCubieTargetDistance = .5f;
    public float MinCubieTargetDistance
    {
        get { return minCubieTargetDistance; }
        set { minCubieTargetDistance = value; }
    }

    //Stop at target flag
    [SerializeField, Header("Cubie Vote Settings:")]
    private bool stopAtTarget = false;
    public bool StopAtTarget
    {
        get { return stopAtTarget; }
        set { stopAtTarget = value; }
    }

    //Cubies speed
    [SerializeField]
    private float speed = 60f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    //Slowdown speed
    [SerializeField]
    private float slowdownSpeed = 5f;
    public float SlowdownSpeed
    {
        get { return slowdownSpeed; }
        set { slowdownSpeed = value; }
    }

    //Slows down when reachs this distance
    [SerializeField]
    private float slowdownTargetDistance = 10f;
    public float SlowdownTargetDistance
    {
        get { return slowdownTargetDistance; }
        set { slowdownTargetDistance = value; }
    }

    //Stop target distance
    [SerializeField]
    private float stopTargetDistance = 2f;
    public float StopTargetDistance
    {
        get { return stopTargetDistance; }
        set { stopTargetDistance = value; }
    }

    //Min target distance
    [SerializeField]
    private float minTargetDistance = .5f;
    public float MinTargetDistance
    {
        get { return minTargetDistance; }
        set { minTargetDistance = value; }
    }

    //Mode of Cubie
    public int mode = 0;
    public int Mode
    {
        get { return mode; }
        set { mode = value; }
    }

    //Cubies target
    [SerializeField]
    private Vector3 target;
    public Vector3 Target
    {
        get { return target; }
        set { target = value; }
    }

    //Explode flag
    [SerializeField]
    private bool explode = false;
    public bool Explode
    {
        get { return explode; }
        set { explode = value; }
    }

    //Used for scatterplot part 2
    [SerializeField]
    private bool part2 = false;
    public bool Part2
    {
        get { return part2; }
        set { part2 = value; }
    }

    private Animator animator;
    
    private Rigidbody rigid;
    private bool stop = false;

    //Idle Awake time
    [SerializeField]
    private float idleAwakeSeconds = 8f;
    public float IdleAwakeSeconds
    {
        get { return idleAwakeSeconds; }
        set { idleAwakeSeconds = value; }
    }

    public float idleAwakeCountDown = 0f;
    public float idleAwakeTimeStamp = 0f;

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (this.GetComponent<Animator>() != null)
        {
            animator = this.GetComponent<Animator>();
        }
    }

    void Update()
    {
        SetupMode();
        ManageForces();
        PointForceTowardsTarget();

        if (animator != null)
        {
            Ppc.emit = (Mode != 0);
            Ppc_Core.emit = (Mode != 0);
            animator.SetInteger("Mode", Mode);
        }

        if(Core.Instance._questionManager._questionState == QuestionManager.QuestionState.Poll ||
           Core.Instance._questionManager._questionState == QuestionManager.QuestionState.MultipleChoice)
        {
            Ppc_Explode.emit = Explode;
            animator.SetBool("Explode", Explode);
        }

        if (!Part2)
        {
            StopAtTarget = true;
            Physics.IgnoreCollision(this.GetComponent<Collider>(), Core.Instance.Floor.GetComponent<Collider>(), false);
        }
        else
        {
            StopAtTarget = false;
            Physics.IgnoreCollision(this.GetComponent<Collider>(), Core.Instance.Floor.GetComponent<Collider>(), true);
        }
    }

    //Sets up Cubies settings for whatever mode the wall state is in
    void SetupMode()
    {
        if (!EnableScatter)
        {
            if (Core.Instance._state == Core.WallState.Ambient)
            {
                if (Core.Instance._playerManager.ActivePlayers.Count > 0)
                {
                    //Resets Timer
                    idleAwakeTimeStamp = 0f;

                    Target = Core.Instance._playerManager.AvgActivePlayerVector;
                    Mode = 2;

                    SpeachText.text = slogan;
                }
                else
                {
                    SyncActivePlayerTimer();

                    Target = IdleActiveLocation.position;
                }
            }
            else if (Core.Instance._state == Core.WallState.Question)
            {
                if (Core.Instance._playerManager.ActivePlayers.Count > 0)
                {
                    if (Core.Instance._questionManager._questionState == QuestionManager.QuestionState.ScatterPlot)
                    {
                        Target = Core.Instance._playerManager.AvgActivePlayerVector;

                        Mode = 2;
                    }
                    else
                    {
                        Target = IdleExplodeLocation.position;

                        Explode = true;

                        Mode = 0;
                    }
                }
                else
                {
                    if (Core.Instance._questionManager._questionState == QuestionManager.QuestionState.ScatterPlot)
                    {
                        Target = IdleActiveLocation.position;
                        Mode = 0;
                    }
                    else
                    {
                        Target = IdleNonActiveLocation.position;



                        Mode = 0;
                    }
                }
            }
        }
        else //If in scatter plot
        {
            if (!Part2)
            {
                //float pivot_x = -35f;
                //float z = 32.5f;
                //
                //Target = new Vector3(pivot_x + (70f * Core.Instance._questionManager.AnswerScatterPlotVote.x), Random.Range(9f, 15f), z);
            }
            else
            {
                float pivot_x = -32f;
                float pivot_z = -12f;
                float z = 32.5f;

                Target = new Vector3(pivot_x + (64f * Core.Instance._questionManager.AnswerScatterPlotVote.x), pivot_z + (24f * Core.Instance._questionManager.AnswerScatterPlotVote.y), z);
            }

            Mode = 0;
        }
    }

    //Manages the physics forces
    void ManageForces()
    {
        if (!EnableScatter)
        {
            this.GetComponent<Rigidbody>().useGravity = true;

            CgfForce.ForcePower = 8f;
            CgfTorque.ForcePower = .5f;
            CgfTorque._forceTypeProperties.TorqueMaxAngularVelocity = 1f;

            if (MinCubieTargetDistance < Vector3.Distance(this.transform.position, Target))
                CgfForce.Enable = true;
            else
                CgfForce.Enable = false;

            CgfTorque.Enable = true;
            CgfHover.Enable = true;
        }
        else
        {
            if (!stop)
            {
                rigid.useGravity = false;

                if (StopAtTarget == true)
                {
                    if (StopTargetDistance > Vector3.Distance(this.transform.position, Target))
                    {
                        RigidbodyScatterPlotStop();

                        stop = true;
                    }
                }

                if (MinTargetDistance < Vector3.Distance(this.transform.position, Target))
                {
                    if (SlowdownTargetDistance < Vector3.Distance(this.transform.position, Target))
                    {
                        CgfForce.ForcePower = Speed;
                    }
                    else
                    {
                        CgfForce.ForcePower = SlowdownSpeed;
                    }

                    CgfForce.Enable = true;
                }
                else
                {
                    CgfForce.Enable = false;
                }

                PointForceTowardsTarget();
            }

            CgfTorque.Enable = false;
            CgfHover.Enable = false;
        }
    }

    //Points cubie towards target
    void PointForceTowardsTarget()
    {
        var flatVectorToTarget = Vector3.zero;

        flatVectorToTarget = forcePivot.position - Target;

        var newRotation = Quaternion.LookRotation(flatVectorToTarget);
        var rotation = Quaternion.Slerp(forcePivot.rotation, newRotation, Time.deltaTime * RotationSlerpSpeed);

        forcePivot.rotation = rotation;
    }

    //Resets Cubie
    public void ResetCubie()
    {
        //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().tag = "Untagged";

        stop = false;
        Part2 = false;

        RigidbodyReset();

        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        EnableScatter = false;

        this.transform.position = IdleNonActiveLocation.position;
    }

    //Resets Cubies rigibody
    void RigidbodyReset()
    {
        rigid.useGravity = true;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.mass = 1.0f;
        rigid.drag = 2.0f;
        rigid.angularDrag = .5f;
        Explode = false;
        Ppc_Explode.emit = false;
        animator.SetBool("Explode", false);
    }

    //Sets the rigidbody settings for when in a scatter plot
    void RigidbodyScatterPlotStop()
    {
        rigid.useGravity = true;
        rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rigid.drag = 0f;
        rigid.angularDrag = 0.05f;
        rigid.velocity = new Vector3(0, 0, 0);
        rigid.angularVelocity = Vector3.zero;
    }

    void SyncActivePlayerTimer()
    {
        if (Core.Instance._playerManager.ActivePlayers.Count == 0)
        {
            SyncSpawnTime();

            if (idleAwakeCountDown > IdleAwakeSeconds)
            {
                Mode = 1;

                idleAwakeTimeStamp = 0;
            }
        }
        else
        {
            Mode = 0;
            idleAwakeTimeStamp = 0;
        }
    }

    void SyncSpawnTime()
    {
        if (idleAwakeTimeStamp == 0)
        {
            idleAwakeTimeStamp = Time.time;
        }

        idleAwakeCountDown = (Time.time - idleAwakeTimeStamp);
    }

    #endregion
}
