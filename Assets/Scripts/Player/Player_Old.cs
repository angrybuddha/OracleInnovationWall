/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for active player object.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using CircularGravityForce;
using System.Collections.Generic;

public class Player_Old : MonoBehaviour
{
    #region Enume

    //Player Mode
    public enum PlayerMode
    {
        None,
        Engaged,
        Observer,
    }

    #endregion

    #region Properties

    //Player id fomr the mqtt signal
    [SerializeField, Header("Settings:")]
    private string id = "-1";
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    //Used for the current player mode
    [SerializeField]
    private PlayerMode mode = PlayerMode.None;
    public PlayerMode Mode
    {
        get { return mode; }
        set 
        {
            ResetModeTime();

            TimeOutCountdown = 0;
            timeOutStamp = 0;

            mode = value;
        }
    }

    //Follows target
    [SerializeField]
    private Transform followTarget;
    public Transform FollowTarget
    {
        get { return followTarget; }
        set { followTarget = value; }
    }

    //Hand transform
    [SerializeField]
    private Transform hand;
    public Transform Hand
    {
        get { return hand; }
        set { hand = value; }
    }

    //Player object selected
    [SerializeField]
    private GameObject selected;
    public GameObject Selected
    {
        get { return selected; }
        set { selected = value; }
    }

    //Player speed
    [SerializeField]
    private float speed = 6f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    //Mode time
    [SerializeField, Header("Timers: ")]
    private float modeTime = 0f;
    public float ModeTime
    {
        get { return modeTime; }
        set { modeTime = value; }
    }

    //Timeout seconds
    [SerializeField]
    private float timeOutSeconds = 1.5f;
    public float TimeOutSeconds
    {
        get { return timeOutSeconds; }
        set { timeOutSeconds = value; }
    }

    //Timeout countdown
    [SerializeField]
    private float timeOutCountdown = 0f;
    public float TimeOutCountdown
    {
        get { return timeOutCountdown; }
        set { timeOutCountdown = value; }
    }

    //Player answer value
    [SerializeField, Header("Answer Feedback: ")]
    private int playerAnswer = 0;
    public int PlayerAnswer
    {
        get { return playerAnswer; }
        set { playerAnswer = value; }
    }

    private float timeModeStamp = 0f;
    private float timeOutStamp = 0f;

    #endregion

    #region Gizmo Functions

    //Draws player icons for editor based on the players state
    void OnDrawGizmos()
    {
        switch (Mode)
        {
            case Player_Old.PlayerMode.None:
                Gizmos.DrawIcon(this.transform.position, "Player - None.png");
                break;
            case Player_Old.PlayerMode.Engaged:
                Gizmos.DrawIcon(this.transform.position, "Player - Engaged.png");
                break;
            case Player_Old.PlayerMode.Observer:
                Gizmos.DrawIcon(this.transform.position, "Player - Observer.png");
                break;
        }

        //Draws hand in editor
        if(Mode != PlayerMode.None)
            Gizmos.DrawWireSphere(hand.position, .25f);
    }

    #endregion

    #region Unity Functions

    void Start()
    {
        
    }

    void Update()
    {
        if (FollowTarget != null)
        {
            SyncModeTime();

            SyncTimeOut();

            MoveToTarget();

            if (Core.Instance._questionManager._questionState == QuestionManager.QuestionState.Poll ||
                Core.Instance._questionManager._questionState == QuestionManager.QuestionState.MultipleChoice ||
                Core.Instance._questionManager._questionState == QuestionManager.QuestionState.ScatterPlot)
            {
                SyncCubeVote();
            }
        }
    }

    #endregion

    #region Functions

    //Syncs the mode time
    void SyncModeTime()
    {
        ModeTime = (Time.time - timeModeStamp);
    }

    //Syncs the time out
    void SyncTimeOut()
    {
        if (!Core.Instance._mqttListener.DoesIdExist(Id))
        {
            if (timeOutStamp == 0)
            {
                timeOutStamp = Time.time;
            }

            TimeOutCountdown = (Time.time - timeOutStamp);
        }
        else
        {
            timeOutStamp = 0;
        }

        if (timeOutCountdown > TimeOutSeconds)
        {
            Mode = PlayerMode.None;
            FollowTarget.localPosition = Vector3.zero;
        }
    }

    //Moves to the target
    void MoveToTarget()
    {
        if (followTarget != null)
        {
            transform.position = Vector3.Lerp(this.transform.position, followTarget.transform.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(this.transform.position, Core.Instance._playerManager.ParentPlayers.transform.position, Time.deltaTime * speed);
        }
    }

    //Syncs up the cube vote
    void SyncCubeVote()
    {
        if (Core.Instance._state == Core.WallState.Question && Core.Instance._questionManager._voteState != QuestionManager.VoteState.None)
        {
            if (Core.Instance._questionManager._questionState == QuestionManager.QuestionState.Poll ||
                Core.Instance._questionManager._questionState == QuestionManager.QuestionState.MultipleChoice)
            {
                if (Selected == null && Mode == PlayerMode.Engaged)
                {
                    if (ModeTime > .2f)
                    {
                        ActivateCubeVote(true, "Untagged");
                    }
                }
                else if (Selected != null && Mode == PlayerMode.None)
                {
                    ActivateCubeVote(false, "Untagged");
                }
            }
        }
        else
        {
            if (Selected != null)
            {
                ActivateCubeVote(false, "Untagged");
            }
        }
    }

    //Activates the cube vote
    public void ActivateCubeVote(bool active, string spawnCubeTag)
    {
        if (active)
        {
            Core.Instance._spawnManager.PlayerCubes.SpawnAtPoint(new Vector3(hand.position.x, 5f, hand.position.z));

            Selected = Core.Instance._spawnManager.PlayerCubes.LastSpawned;

            Selected.GetComponent<Cube>().gameObject.tag = spawnCubeTag;
            Selected.GetComponent<Cube>().Target = Hand.transform;
            Selected.GetComponent<Cube>().CgfForce.Enable = true;
            Selected.GetComponent<Cube>().CgfTorque.Enable = true;
            Selected.GetComponent<Cube>().CgfHover.Enable = true;
        }
        else
        {
            Selected.GetComponent<Cube>().gameObject.tag = spawnCubeTag;
            Selected.GetComponent<Cube>().Target = null;
            Selected.GetComponent<Cube>().CgfForce.Enable = false;
            Selected.GetComponent<Cube>().CgfTorque.Enable = false;
            Selected.GetComponent<Cube>().CgfHover.Enable = false;

            Selected = null;
        }
    }

    //Resets the mode time
    public void ResetModeTime()
    {
        ModeTime = 0.0f;
        timeModeStamp = Time.time;
    }

    #endregion
}
