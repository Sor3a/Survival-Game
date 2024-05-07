//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using EditorGUITable;
//using System.IO;

//[CustomEditor(typeof(spawner1))]
//[CanEditMultipleObjects]
//public class spawnerEditor1 : Editor
//{
//    GUITableState spawnerTable, enemysTable;
//    SerializedProperty layer;
//    private void OnEnable()
//    {
//        spawnerTable = new GUITableState("spawnerTable");
//        enemysTable = new GUITableState("enemysTable");
//        layer = serializedObject.FindProperty("Layer");
//    }
//    public override void OnInspectorGUI()
//    {
//        spawner1 Spawner = (spawner1)target;
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
//        EditorGUILayout.Space(40);
//        enemysTable = GUITableLayout.DrawTable(enemysTable, serializedObject.FindProperty("enemysPrefabs"));
//        EditorGUILayout.Space(20);
//        EditorGUILayout.BeginHorizontal();
//        if (GUILayout.Button("+"))
//        {
//            Spawner.AddEnemy();
//        }
//        if (GUILayout.Button("-"))
//        {
//            Spawner.RemoveEnemy();
//        }
//        if (GUILayout.Button("+B"))
//        {
//            Spawner.addBeg();
//        }
//        EditorGUILayout.EndHorizontal();

//    }
//}
