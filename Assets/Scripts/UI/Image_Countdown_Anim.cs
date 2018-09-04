/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Binds the image countdown animation for a player cube.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Image_Countdown_Anim : MonoBehaviour
{
    private Animator animator;

    private Image image;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float fillAmount = (Core.Instance._questionManager.VoteTimer - Core.Instance._questionManager.VoteCountDown) / Core.Instance._questionManager.VoteTimer;

        if (fillAmount == 0)
        {
            fillAmount = 1f;
        }

        image.fillAmount = fillAmount;

        animator.SetBool("Disable", Core.Instance._questionManager._voteState != QuestionManager.VoteState.Voting);
    }
}
