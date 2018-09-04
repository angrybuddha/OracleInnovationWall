/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Binds the player avg for the scatter plot slider.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slider_Scatterplot_Bind : MonoBehaviour
{
    private Slider slider;

    // Use this for initialization
    void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Core.Instance._questionManager._voteState == QuestionManager.VoteState.Waiting || 
            Core.Instance._questionManager._voteState == QuestionManager.VoteState.Voting)
            slider.value = Core.Instance._playerManager.AvgScatterValue;
    }
}
