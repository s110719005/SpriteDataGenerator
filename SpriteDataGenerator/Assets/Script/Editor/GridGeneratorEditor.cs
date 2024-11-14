using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GridSystem;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        

        serializedObject.Update();

        DrawInspector();

        GridGenerator gridGenerator = target as GridGenerator;

        if (GUILayout.Button("Add empty grid into Scene"))
        {
            gridGenerator.DEBUG_GenerateGrid();
        }           
        if (GUILayout.Button("Load grid SO"))
        {
            gridGenerator.DEBUG_GenerateGridTemplate();
        }           
        if (GUILayout.Button("Save grid to loaded SO"))
        {
            gridGenerator.DEBUG_UpdateGridDefinition();

        }  
        if (GUILayout.Button("Reset Grid"))
        {
            gridGenerator.DEBUG_ResetGrid();
        }
        GUILayout.Space(10);     
        if (GUILayout.Button("Create new grid Asset"))
        {
            gridGenerator.DEBUG_CreateGridAsset();
        }      

        serializedObject.ApplyModifiedProperties();

        SceneView.RepaintAll();
    }

        

    private void DrawInspector()
    {
        EditorGUILayout.LabelField("Grid Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        {
            var currentColor = serializedObject.FindProperty("currentColor");
            EditorGUILayout.PropertyField(currentColor);
            var defaultGrid = serializedObject.FindProperty("defaultGrid");
            EditorGUILayout.PropertyField(defaultGrid);
            var gridDefinitionsToLoad = serializedObject.FindProperty("gridDefinitionsToLoad");
            EditorGUILayout.PropertyField(gridDefinitionsToLoad);
            var DEBUG_hasGenerate = serializedObject.FindProperty("DEBUG_hasGenerate");
            EditorGUILayout.PropertyField(DEBUG_hasGenerate);
            var DEBUG_canMouseInput = serializedObject.FindProperty("DEBUG_canMouseInput");
            EditorGUILayout.PropertyField(DEBUG_canMouseInput);
        }
        GUILayout.EndVertical();


    }
}


