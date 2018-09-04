using UnityEngine;
using System.Collections;

public class SM_ScatterPlotTransition : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Results 1 Code//
        Core.Instance.Floor.transform.position = new Vector3(0, -12f, 0);
        Core.Instance._questionManager.GatherScatterPlotVote();
        Core.Instance._questionManager._voteState = QuestionManager.VoteState.None;
        //////////////////

        //Core.Instance._questionManager.EnableCGFScatterAreas(true);

        //UI
        Core.Instance._questionManager.ScatterPlotPart1UI.GetComponent<Animator>().SetTrigger("Exit");
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Core.Instance.Floor.transform.position = new Vector3(0, -3f, 0);

        //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().Part2 = true;

        //UI
        Core.Instance._questionManager.ScatterPlotPart2UI.GetComponent<Animator>().SetTrigger("Enable");
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
