using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
    float quad_w = Screen.width / 4;
    float quad_x;
    float quad_h = Screen.height;

    void Update()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.mousePosition.x <= Screen.width/4)
        {
            //print(hit.collider.name);
            print("in quadrant 01 " + Input.mousePosition.x);
            quad_x = 0;
        }
        else if((Input.mousePosition.x > Screen.width/4) && (Input.mousePosition.x <= Screen.width * 2 / 4))
        {

            print("in quadrant 02 " + Input.mousePosition.x);
            quad_x = Screen.width/4;
        }
        else if ((Input.mousePosition.x > Screen.width / 2) && (Input.mousePosition.x <= Screen.width * 3 / 4))
        {

            print("in quadrant 02 " + Input.mousePosition.x);
            quad_x = Screen.width / 2;
            
        }
        else if ((Input.mousePosition.x > Screen.width *3 / 4) && (Input.mousePosition.x <= Screen.width))
        {

            print("in quadrant 02 " + Input.mousePosition.x);
            quad_x = Screen.width * 3 / 4;
            
        }
        else
        {
            print("NO QUADRANT");
        }
    }

    void OnGUI()
    {
        //GUIStyle style = new GUIStyle();

        Rect rect = new Rect(quad_x, 0, quad_w, quad_h);
        GUI.Box(rect, "");
    }
}