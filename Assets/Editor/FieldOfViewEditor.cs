using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RomanTristan.Lab5
{
    [CustomEditor (typeof (FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        void OnSceneGUI()
        {
            FieldOfView fow = (FieldOfView)target;
            Handles.color = Color.white;
            Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
            Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

            Handles.DrawWireArc(fow.transform.localPosition, Vector3.up, viewAngleA, fow.viewAngle, fow.viewRadius);
            Handles.DrawLine(fow.transform.localPosition + viewAngleA, fow.transform.position + viewAngleA * fow.viewRadius);
            Handles.DrawLine(fow.transform.localPosition + viewAngleB, fow.transform.position + viewAngleB * fow.viewRadius);

            Handles.color = Color.red;
            foreach(Transform visibleTarget in fow.visibleTargets)
            {
                Handles.DrawLine(fow.transform.position, visibleTarget.position);
            }

            Handles.color = Color.blue;
            foreach (Transform visibleObject in fow.visibleObjects)
            {
                Handles.DrawLine(fow.transform.position, visibleObject.position);
            }
        }
    }
}
