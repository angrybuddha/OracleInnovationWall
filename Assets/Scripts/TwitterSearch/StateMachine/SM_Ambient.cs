using UnityEngine;
using System.Collections;
using System.Linq;

public class SM_Ambient : StateMachineBehaviour 
{
    [SerializeField]
    private float activePlayerSeconds = 8f;
    public float ActivePlayerSeconds
    {
        get { return activePlayerSeconds; }
        set { activePlayerSeconds = value; }
    }

    private float activePlayerCountDown = 0f;
    private float activePlayerTimeStamp = 0f;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Resets Timer
        activePlayerTimeStamp = 0f;

        Core.Instance._playerManager.ResetAllPlayerModeTime();

        //Enable Triggers, and CGF Areas
        Core.Instance._questionManager.EnableTriggerAreas(QuestionManager.QuestionState.None);
        Core.Instance._questionManager.EnableCGFAreas(QuestionManager.QuestionState.None);
        Core.Instance._questionManager.EnableCGFScatterAreas(false);

        //Sets State to Ambient, and reset vote State
        Core.Instance._state = Core.WallState.Ambient;
        Core.Instance._questionManager._questionState = QuestionManager.QuestionState.None;
        Core.Instance._questionManager._voteState = QuestionManager.VoteState.None;

        //Resets Cubie Location
        //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().ResetCubie();
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SyncActivePlayerTimer();
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerIndex)
    {
        Core.Instance._playerManager.ResetAllPlayerModeTime();
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    void SyncActivePlayerTimer()
    {
        var list =
            from p in Core.Instance._playerManager.ActivePlayers
            where p.Mode == Player_Old.PlayerMode.Engaged
            select p;

        if (list.Count() > 0)
        {
            SyncSpawnTime();

            if (activePlayerCountDown > ActivePlayerSeconds)
            {
                Core.Instance._state = Core.WallState.Question;
            }
        }
        else
        {
            activePlayerTimeStamp = 0;
        }
    }

    void SyncSpawnTime()
    {
        if (activePlayerTimeStamp == 0)
        {
            activePlayerTimeStamp = Time.time;
        }

        activePlayerCountDown = (Time.time - activePlayerTimeStamp);
    }
}
