/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used of controling the animation for the Ambient canvas.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class UICanvas_Ambient : MonoBehaviour
{
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Enable", (Core.Instance._state == Core.WallState.Ambient));
    }
}
