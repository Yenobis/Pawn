using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossAI))]
public class FieldOfViewEditorBoss : Editor
{
    private void OnSceneGUI()
    {
        BossAI fov = (BossAI)target;
        Handles.color = Color.magenta;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fovRadius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.black;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.fovRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.fovRadius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.cyan;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
