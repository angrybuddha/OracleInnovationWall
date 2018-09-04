using UnityEngine;
using System.Collections;
using System.Linq;

public class SM_Poll : StateMachineBehaviour
{
    [SerializeField]
    private float nonActivePlayerSeconds = 10f;
    public float NonActivePlayerSeconds
    {
        get { return nonActivePlayerSeconds; }
        set { nonActivePlayerSeconds = value; }
    }

    private float nonActivePlayerCountDown = 0f;
    private float nonActivePlayerTimeStamp = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Core.Instance._questionManager.EnableTriggerAreas(QuestionManager.QuestionState.Poll);
        Core.Instance._questionManager.EnableCGFAreas(QuestionManager.QuestionState.Poll);

        Core.Instance._questionManager._voteState = QuestionManager.VoteState.Waiting;

        //UI
        Core.Instance._questionManager.PollUI.GetComponent<Animator>().SetTrigger("Enable");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SyncActivePlayerTimer();

        Core.Instance._questionManager.SyncVoting();
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Core.Instance._state == Core.WallState.Ambient)
        {
            Core.Instance._questionManager.PollUI.GetComponent<Animator>().SetTrigger("Exit");

            Core.Instance._questionManager._voteState = QuestionManager.VoteState.None;
        }
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
            from g in Core.Instance._playerManager.ActivePlayers
            select g;

        if (list.Count() == 0)
        {
            SyncSpawnTime();

            if (nonActivePlayerCountDown > NonActivePlayerSeconds)
            {
                Core.Instance._state = Core.WallState.Ambient;
            }
        }
        else
        {
            nonActivePlayerTimeStamp = 0;
        }
    }

    void SyncSpawnTime()
    {
        if (nonActivePlayerTimeStamp == 0)
        {
            nonActivePlayerTimeStamp = Time.time;
        }

        nonActivePlayerCountDown = (Time.time - nonActivePlayerTimeStamp);
    }
}
