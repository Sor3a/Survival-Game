using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using EditorGUITable;
using System.IO;

//[CustomEditor(typeof(spawnerh))]
//[CanEditMultipleObjects]
//public class spawnerEditor : Editor
//{
//    GUITableState spawnerTable;
//    SerializedProperty layer;
//    private void OnEnable()
//    {
//        spawnerTable = new GUITableState("spawnerTable");
//        layer = serializedObject.FindProperty("Layer");
//    }
//    public override void OnInspectorGUI()
//    {
//        spawnerh Spawner = (spawnerh)target;
//        EditorGUILayout.PropertyField(layer, new GUIContent("Layer"));
//        spawnerTable = GUITableLayout.DrawTable(spawnerTable, serializedObject.FindProperty("spawnList"));
//        EditorGUILayout.Space(20);
//        EditorGUILayout.BeginHorizontal();
//        if (GUILayout.Button("+"))
//        {
//            Spawner.AddSpawner();
//        }
//        if (GUILayout.Button("-"))
//        {
//            Spawner.RemoveSpawner();
//        }
//        EditorGUILayout.EndHorizontal();

//    }
//}
