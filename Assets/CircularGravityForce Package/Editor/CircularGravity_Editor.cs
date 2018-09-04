/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Version: 3.07
* Created Date: 11-27-14
* Last Updated: 06-15-15
*******************************************************************************************/
using UnityEditor;
using UnityEngine;
using System.Collections;
using CircularGravityForce;

[CustomEditor(typeof(CircularGravity)), CanEditMultipleObjects]
public class CircularGravity_Editor : Editor
{
    private CircularGravity cgf;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        cgf = (CircularGravity)target;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void OnSceneGUI()
    {
        cgf = (CircularGravity)target;

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
        qLeft.SetLookRotation(cgf.transform.rotation * Vector3.left);
        Quaternion qRight = cgf.transform.transform.rotation;
        qRight.SetLookRotation(cgf.transform.rotation * Vector3.right);
        Quaternion qForward = cgf.transform.transform.rotation;
        qForward.SetLookRotation(cgf.transform.rotation * Vector3.forward);
        Quaternion qBack = cgf.transform.transform.rotation;
        qBack.SetLookRotation(cgf.transform.rotation * Vector3.back);

        float dotSpace = 10f;

        switch (cgf._shape)
        {
            case CircularGravity.Shape.Sphere:

                Handles.color = tranMainColor;
                Handles.SphereCap(0, cgf.transform.position, cgf.transform.rotation, cgf.Size * 2f);
                Handles.color = mainColor;

                if (cgf._forceType == CircularGravity.ForceType.ExplosionForce || cgf._forceType == CircularGravity.ForceType.ForceAtPosition || cgf._forceType == CircularGravity.ForceType.GravitationalAttraction)
                {
                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.left, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.right, cgf.Size, 1), cgf.transform.position, dotSpace);

                    Handles.ConeCap(0, GetVector(Vector3.up, cgf.Size + gizmoOffset, 1f), qUp, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.down, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.left, cgf.Size + gizmoOffset, 1f), qLeft, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.right, cgf.Size + gizmoOffset, 1f), qRight, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.back, cgf.Size + gizmoOffset, 1f), qBack, gizmoSize);

                    Handles.CircleCap(0, cgf.transform.position, qUp, cgf.Size);
                    Handles.CircleCap(0, cgf.transform.position, qForward, cgf.Size);
                }
                else if (cgf._forceType == CircularGravity.ForceType.Torque)
                {
                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.Size, 1), cgf.transform.position, dotSpace);

                    Handles.ConeCap(0, GetVector(Vector3.up, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.down, cgf.Size + gizmoOffset, 1f), qBack, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                    Handles.ConeCap(0, GetVector(Vector3.back, cgf.Size + gizmoOffset, 1f), qUp, gizmoSize);

                }
                else
                {
                    Handles.ConeCap(0, GetVector(Vector3.back, cgf.Size + gizmoOffset, 1f), qBack, -gizmoSize);
                }

                Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1), cgf.transform.position, dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.back, cgf.Size, 1), cgf.transform.position, dotSpace);

                if (cgf._forceType != CircularGravity.ForceType.Torque)
                {
                    Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                }

                Handles.CircleCap(0, cgf.transform.position, qLeft, cgf.Size);

                break;
            case CircularGravity.Shape.Capsule:

                Handles.DrawDottedLine(GetVector(Vector3.up, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.down, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.left, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.right, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);

                Handles.DrawDottedLine(cgf.transform.position, GetVector(Vector3.forward, cgf.Size, 1), dotSpace);

                Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.up) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.down) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.left) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);
                Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.right) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);

                if (cgf._forceType != CircularGravity.ForceType.Torque)
                {
                    Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                }
                else
                {
                    Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                }

                Handles.CircleCap(0, cgf.transform.position, qForward, cgf.CapsuleRadius);
                Handles.CircleCap(0, GetVector(Vector3.forward, cgf.Size, 1), qForward, cgf.CapsuleRadius);

                break;
            case CircularGravity.Shape.RayCast:

                Handles.DrawDottedLine(cgf.transform.position + ((cgf.transform.rotation * Vector3.forward) * cgf.Size), cgf.transform.position, dotSpace);

                if (cgf._forceType != CircularGravity.ForceType.Torque)
                {
                    Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                }
                else
                {
                    Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                }

                break;
        }

        if (cgf._forceType == CircularGravity.ForceType.ExplosionForce || cgf._forceType == CircularGravity.ForceType.ForceAtPosition || cgf._forceType == CircularGravity.ForceType.GravitationalAttraction)
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
