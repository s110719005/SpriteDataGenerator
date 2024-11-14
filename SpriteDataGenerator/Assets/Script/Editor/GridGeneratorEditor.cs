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
        if (GUILayout.Button("Record grid color into SO"))
        {
            gridGenerator.DEBUG_RecordColor();

        }  
        if (GUILayout.Button("Reset Grid"))
        {
            gridGenerator.DEBUG_ResetGrid();
        }
        GUILayout.Space(10);     
        if (GUILayout.Button("Create new grid Asses"))
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
            var gridDefinitions = serializedObject.FindProperty("gridDefinitions");
            EditorGUILayout.PropertyField(gridDefinitions);
            // var accuracyText = serializedObject.FindProperty("accuracyText");
            // EditorGUILayout.PropertyField(accuracyText);
            // var templateImage = serializedObject.FindProperty("templateImage");
            // EditorGUILayout.PropertyField(templateImage);
            // var endingTemplateImage = serializedObject.FindProperty("endingTemplateImage");
            // EditorGUILayout.PropertyField(endingTemplateImage);
            var DEBUG_hasGenerate = serializedObject.FindProperty("DEBUG_hasGenerate");
            EditorGUILayout.PropertyField(DEBUG_hasGenerate);
            var DEBUG_canMouseInput = serializedObject.FindProperty("DEBUG_canMouseInput");
            EditorGUILayout.PropertyField(DEBUG_canMouseInput);
        }
        GUILayout.EndVertical();


    }
}


