using UnityEngine;
using System.Collections;

public class SM_Feed : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //UI
        Core.Instance._questionManager.FactUI.GetComponent<Animator>().SetTrigger("Enable");
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Cleanup Votes
        /*Core.Instance._questionManager.Answer1Vote = 0;
        Core.Instance._questionManager.Answer2Vote = 0;
        Core.Instance._questionManager.Answer3Vote = 0;
        Core.Instance._spawnManager.ResetAllSpawnedObjects();

        Core.Instance._cms.OpenConnection();
        Core.Instance._cms.GetTableRecords(CMS_Controler.Question_Table);
        Core.Instance._cms.CloseConnection();
        Core.Instance._cms_Controler.LoadQuestionMedia();

        //UI
        Core.Instance._questionManager.FactUI.GetComponent<Animator>().SetTrigger("Exit");
        Core.Instance._questionManager.LogoUI.GetComponent<Animator>().SetTrigger("Enable");*/
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
