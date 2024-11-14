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
            string folderPath = "Assets/LUA/"; // the path of LUA folder
            //string folderPath = @"C:\EAE\2024_Fall\GameEngineeringII\GraphicProject\MyGame_\Content\Meshes"; //direct path

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created
            
            var fileName = target.name + ".lua"; // put your data format here
            using (StreamWriter streamWriter = File.CreateText(folderPath + "/" + fileName))
            {
                //LUA content here
                var gridDatas = serializedObject.FindProperty("gridColorDatas");
                List<Vector3> vertexs = new List<Vector3>();
                List<int> indeces = new List<int>();
                List<Color> colors = new List<Color>();
                int currentIndex = 0;
                for (int i = 0; i < gridDatas.arraySize; i++)
                {
                    var gridData = gridDatas.GetArrayElementAtIndex(i);
                    // get color
                    Color color = gridData.FindPropertyRelative("color").colorValue;
                    
                    //if opacity > 0.6 means it's not an empty pixel (default is 0.5)
                    if (color.a > 0.6f)
                    {
                        // get x
                        int x = gridData.FindPropertyRelative("x").intValue;
                        // get y
                        int y = gridData.FindPropertyRelative("y").intValue;
                        
                        //z = 0 since it's 2D
                        Vector3 vertex0 = new Vector3(x - 0.5f, y - 0.5f, 0);
                        Vector3 vertex1 = new Vector3(x + 0.5f, y + 0.5f, 0);
                        Vector3 vertex2 = new Vector3(x + 0.5f, y - 0.5f, 0);
                        Vector3 vertex3 = new Vector3(x - 0.5f, y + 0.5f, 0);

                        //TODO: may need to resize somewhere
                        vertex0 = vertex0 * 0.1f;
                        vertex1 = vertex1 * 0.1f;
                        vertex2 = vertex2 * 0.1f;
                        vertex3 = vertex3 * 0.1f;
                        vertexs.Add(vertex0);
                        vertexs.Add(vertex1);
                        vertexs.Add(vertex2);
                        vertexs.Add(vertex3);
                        indeces.Add(currentIndex);
                        indeces.Add(currentIndex + 1);
                        indeces.Add(currentIndex + 2);
                        indeces.Add(currentIndex);
                        indeces.Add(currentIndex + 3);
                        indeces.Add(currentIndex + 1);
                        colors.Add(color);
                        colors.Add(color);
                        colors.Add(color);
                        colors.Add(color);

                        currentIndex += 4;
                    }
                }



                streamWriter.WriteLine("return");
                streamWriter.WriteLine("{");
                {
                    streamWriter.WriteLine("    vertex = ");
                    streamWriter.WriteLine("    {");
                    foreach(var vertex in vertexs)
                    {
                        streamWriter.WriteLine("        {" + vertex.x + ", " + vertex.y + ", " + vertex.z + " },");
                    }
                    streamWriter.WriteLine("    },");
                }
                {
                    streamWriter.WriteLine("    indices = ");
                    streamWriter.WriteLine("    {");
                    for(int i = 0; i < indeces.Count; i+=3) // this is just to make it more human readable
                    {
                        streamWriter.WriteLine("        " + indeces[i] + ", " + indeces[i + 1] + ", "+ indeces[i + 2] + ", ");
                    }
                    streamWriter.WriteLine("    },");
                }
                {
                    streamWriter.WriteLine("    color = ");
                    streamWriter.WriteLine("    {");
                    foreach(var color in colors)
                    {
                        streamWriter.WriteLine("        {" + color.r + ", " + color.g + ", " + color.b + ", " + color.a + " },");
                    }
                    streamWriter.WriteLine("    },");
                }
                streamWriter.WriteLine("}");
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
