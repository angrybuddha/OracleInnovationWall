/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used of controling the animation the player cube icon.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class UICanvas_CubeIcon_Bind : MonoBehaviour 
{
    [SerializeField]
    private Player_Old playerRef;
    public Player_Old PlayerRef
    {
        get { return playerRef; }
        set { playerRef = value; }
    }

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRef != null)
            animator.SetBool(
                "Enable", 
                PlayerRef.Mode == Player_Old.PlayerMode.Engaged && 
                Core.Instance._state == Core.WallState.Question && 
                Core.Instance._questionManager._questionState == QuestionManager.QuestionState.ScatterPlot &&
                Core.Instance._questionManager._voteState != QuestionManager.VoteState.None
                );
    }
}
