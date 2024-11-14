using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GridSystem;
using System.IO;

[CustomEditor(typeof(GridDefinition))]
public class GridDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        

        serializedObject.Update();

        DrawInspector();

        
        if (GUILayout.Button("Save"))
        {
            AssetDatabase.SaveAssets();
            Undo.RecordObject(target, "Setting Value");
            EditorUtility.SetDirty(target);
        }    

        if (GUILayout.Button("Generate LUA from this template"))
        {
            string folderPath = "Assets/LUA/"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created
            
            var fileName = target.name + ".lua"; // put your data format here
            using (StreamWriter sw = File.CreateText(folderPath + "/" + fileName))
            {
                //LUA content here
                sw.WriteLine("This is a new lua file!");
                var gridDatas = serializedObject.FindProperty("gridColorDatas");
                for (int i = 0; i < gridDatas.arraySize; i++)
                {
                    var gridData = gridDatas.GetArrayElementAtIndex(i);
                    // get x
                    int x = gridData.FindPropertyRelative("x").intValue;
                    // get y
                    int y = gridData.FindPropertyRelative("y").intValue;
                    // get color
                    Color color = gridData.FindPropertyRelative("color").colorValue;
                    
                    if (color.a > 0.6f)
                    {
                        sw.WriteLine("PIXEL FOUND");
                    }
                }
                sw.WriteLine("second line");
            }
            Debug.Log("Generate : " + fileName + " at " + folderPath);
            AssetDatabase.Refresh();
        }           

        serializedObject.ApplyModifiedProperties();

        SceneView.RepaintAll();
    }

        

    private void DrawInspector()
    {
        EditorGUILayout.LabelField("Grid Definition", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        {
            var gridWidth = serializedObject.FindProperty("gridWidth");
            EditorGUILayout.PropertyField(gridWidth);
            var gridHeight = serializedObject.FindProperty("gridHeight");
            EditorGUILayout.PropertyField(gridHeight);
            var cellSize = serializedObject.FindProperty("cellSize");
            EditorGUILayout.PropertyField(cellSize);
            var gridSprite = serializedObject.FindProperty("gridSprite");
            EditorGUILayout.PropertyField(gridSprite);
            var templateSprite = serializedObject.FindProperty("templateSprite");
            EditorGUILayout.PropertyField(templateSprite);
            var usedColors = serializedObject.FindProperty("usedColors");
            EditorGUILayout.PropertyField(usedColors);
            var gridColorDatas = serializedObject.FindProperty("gridColorDatas");
            EditorGUILayout.PropertyField(gridColorDatas);
        }
        GUILayout.EndVertical();
        


    }
}
