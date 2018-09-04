using UnityEngine;
using System.Collections;

public class SM_ScatterPolt_Part1_Results : StateMachineBehaviour {

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Core.Instance.Floor.transform.position = new Vector3(0,-12f,0);

        Core.Instance._questionManager.GatherScatterPlotVote();
        Core.Instance._questionManager._voteState = QuestionManager.VoteState.None;

        //Poll Answer Particles
        //foreach (var par in Core.Instance._questionManager.PollParticles)
        //{
        //    par.emit = true;
        //}

        //UI
        Core.Instance._questionManager.ScatterPlotPart1UI.GetComponent<Animator>().SetTrigger("Enable");
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Core.Instance._spawnManager.SyncCubeVotes(QuestionManager.QuestionState.ScatterPlot);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
