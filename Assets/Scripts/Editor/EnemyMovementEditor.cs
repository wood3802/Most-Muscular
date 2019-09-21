using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy_Movement))]
public class EnemyMovementEditor : Editor
{
    private void OnSceneGUI()
    {
        Enemy_Movement enemy_Movement = (Enemy_Movement)target;
        Handles.color = Color.red;
        foreach(Vector3 node in enemy_Movement.path)
        {
            Handles.DrawWireCube(node, new Vector3(1, 1, 1));
        }
    }
}
