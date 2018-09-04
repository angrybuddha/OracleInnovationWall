/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Adds some tools to the toolbar.
*******************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorCamera_Editor : EditorWindow
{
    #region ToolBar

    [MenuItem("Tools/Move to Obj %m")]
    static void MoveToObj()
    {
        if (SceneView.lastActiveSceneView != null && Selection.activeGameObject != null)
        {
            SceneView.lastActiveSceneView.LookAt(Selection.activeGameObject.transform.position, Selection.activeGameObject.transform.rotation, 0f);
        }
    }

    [MenuItem("Tools/Look at Obj %l")]
    static void LookAtObj()
    {
        if (SceneView.lastActiveSceneView != null && Selection.activeGameObject != null)
        {
            var relativePos = Selection.activeGameObject.transform.position - SceneView.lastActiveSceneView.camera.transform.position;
            var rotation = Quaternion.LookRotation(relativePos);

            SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.camera.transform.position, rotation, 0f);
        }
    }

    #endregion
}
