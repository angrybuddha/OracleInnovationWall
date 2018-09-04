/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used of controling the animation the help text.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class UICanvas_Help_Bind : MonoBehaviour
{
    void OnEnable()
    {
        this.GetComponent<Animator>().SetTrigger("Enable");
    }
}
