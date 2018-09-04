/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 12-20-13
* Last Updated: 06-15-15
*******************************************************************************************/
using UnityEditor;
using UnityEngine;
using System.Collections;
using CircularGravityForce;

[CustomEditor(typeof(CircularGravity2D)), CanEditMultipleObjects]
public class CircularGravity2D_Editor : Editor
{
    private CircularGravity2D cgf;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        cgf = (CircularGravity2D)target;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void OnSceneGUI()
    {
        cgf = (CircularGravity2D)target;

        Color mainColor;
        Color tranMainColor;
        if (cgf.Enable)
        {
            if (cgf.ForcePower == 0)
            {
                mainColor = Color.white;
                tranMainColor = Color.white;
            }
            else if (cgf.ForcePower > 0)
            {
                mainColor = Color.green;
                tranMainColor = Color.green;
            }
            else
            {
                mainColor = Color.red;
                tranMainColor = Color.red;
            }
        }
        else
        {
            mainColor = Color.white;
            tranMainColor = Color.white;
        }

        tranMainColor.a = .1f;

        Handles.color = mainColor;

        float gizmoSize = 0f;
        float gizmoOffset = 0f;

        if (mainColor == Color.green)
        {
            gizmoSize = (cgf.Size / 8f);
            if (gizmoSize > .5f)
                gizmoSize = .5f;
            else if (gizmoSize < -.5f)
                gizmoSize = -.5f;
            gizmoOffset = -gizmoSize / 2f;
        }
        else if (mainColor == Color.red)
        {
            gizmoSize = -(cgf.Size / 8f);
            if (gizmoSize > .5f)
                gizmoSize = .5f;
            else if (gizmoSize < -.5f)
                gizmoSize = -.5f;
            gizmoOffset = gizmoSize / 2f;
        }

        Quaternion qUp = cgf.transform.transform.rotation;
        qUp.SetLookRotation(cgf.transform.rotation * Vector3.up);
        Quaternion qDown = cgf.transform.transform.rotation;
        qDown.SetLookRotation(cgf.transform.rotation * Vector3.down);
        Quaternion qLeft = cgf.transform.transform.rotation;
        qLeft.SetLookRotation(cgf.transform.rotation * Vector3.forward);
        Quaternion qRight = cgf.transform.transform.rotation;
        qRight.SetLookRotation(cgf.transform.rotation * Vector3.back);
        Quaternion qForward = cgf.transform.transform.rotation;
        qForward.SetLookRotation(cgf.transform.rotation * Vector3.right);
        Quaternion qBack = cgf.transform.transform.rotation;
        qBack.SetLookRotation(cgf.transform.rotation * Vector3.left);

        float dotSpace = 10f;

        switch (cgf._shape2D)
        {
            case CircularGravity2D.Shape2D.Sphere:

                Handles.color = tranMainColor;
                Handles.color = mainColor;

                if (cgf._forceType2D == CircularGravity2D.ForceType2D.ForceAtPosition || cgf._forceType2D == CircularGravity2D.ForceType2D.GravitationalAttraction)
                {
                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.left, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.right, cgf.Size, 1), cgf.transform.position, dotSpace);

                    Handles.ConeCap(0, GetVector(Vector3.up, cgf.Size + gizmoOffset, 1f), qUp, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.down, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.left, cgf.Size + gizmoOffset, 1f), qBack, gizmoSize);
                }
                else if (cgf._forceType2D == CircularGravity2D.ForceType2D.Torque)
                {
                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.Size, 1), cgf.transform.position, dotSpace);


                    Handles.ConeCap(0, GetVector(Vector3.up, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.down, cgf.Size + gizmoOffset, 1f), qBack, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.right, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.left, cgf.Size + gizmoOffset, 1f), qUp, gizmoSize);

                }
                else
                {
                    Handles.ConeCap(0, GetVector(Vector3.left, cgf.Size + gizmoOffset, 1f), qBack, -gizmoSize);
                }

                Handles.DrawDottedLine(GetVector(Vector3.right, cgf.Size, 1), cgf.transform.position, dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.left, cgf.Size, 1), cgf.transform.position, dotSpace);

                if (cgf._forceType2D != CircularGravity2D.ForceType2D.Torque)
                {
                    Handles.ConeCap(0, GetVector(Vector3.right, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                }

                Handles.CircleCap(0, cgf.transform.position, qLeft, cgf.Size);

                break;
            case CircularGravity2D.Shape2D.RayCast:

                Handles.DrawDottedLine(cgf.transform.position + ((cgf.transform.rotation * Vector3.right) * cgf.Size), cgf.transform.position, dotSpace);

                if (cgf._forceType2D != CircularGravity2D.ForceType2D.Torque)
                {

                    Handles.ConeCap(0, GetVector(Vector3.right, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);

                }
                else
                {

                    Handles.ConeCap(0, GetVector(Vector3.right, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);

                }

                break;
        }

        if (cgf._forceType2D == CircularGravity2D.ForceType2D.ForceAtPosition || cgf._forceType2D == CircularGravity2D.ForceType2D.GravitationalAttraction)
        {
            Handles.SphereCap(0, cgf.transform.position, cgf.transform.rotation, gizmoSize/2f);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    Vector3 GetVector(Vector3 vector, float size, float times)
    {
        return cgf.transform.position + (((cgf.transform.rotation * vector) * size) / times);
    }
}
