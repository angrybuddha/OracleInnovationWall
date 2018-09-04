using UnityEngine;
using System.Collections;

public class SM_CubieAwake : StateMachineBehaviour {

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*string question = string.Empty;

        int totalQuestions = Core.Instance._cms.GetRecordCount(CMS_Controler.Question_Table, (int)CMS_Controler.Question_Columns.Question1);

        question = Core.Instance._cms.GetRecord(CMS_Controler.Question_Table, (int)CMS_Controler.Question_Columns.Question1, Random.Range(0,totalQuestions));

        //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().SpeachText.text = question;

        int ranLocation = Random.Range(0, 2);
        if(ranLocation == 0)
        {
            //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().IdleActiveLocation.position = Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().IdleLocation[0].position;
        }
        else if (ranLocation == 1)
        {
            //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().IdleActiveLocation.position = Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().IdleLocation[1].position;
        }*/
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().Mode = 0;
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
