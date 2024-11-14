using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;






#if UNITY_EDITOR
using UnityEditor;
#endif


namespace GridSystem
{
    public class GridGenerator : MonoBehaviour
    {
        private Grid grid;
        [SerializeField]
        private GridDefinition defaultGrid;
        [SerializeField]
        private GridDefinition gridDefinitionsToLoad;
        private GridDefinition currentGridDefinition;
        public GridDefinition CurrentGridDefinition => currentGridDefinition;
        [SerializeField]
        private Color currentColor = Color.white;
        // [SerializeField]
        // private TextMeshProUGUI accuracyText;
        // [SerializeField]
        // private Image templateImage;
        // [SerializeField]
        // private Image endingTemplateImage;

        //private int correctCell = 0;

        public bool DEBUG_hasGenerate = false;
        public bool DEBUG_canMouseInput = false;
        
        void Awake()
        {
            if(gridDefinitionsToLoad != null)
            {
                currentGridDefinition = gridDefinitionsToLoad;
            }
            else
            {
                currentGridDefinition = defaultGrid;
            }
            DEBUG_ResetGrid();
            GenerateGrid();
        }

        void Update()
        {
            CheckMouseInput();
        }

        public void GenerateGrid()
        {
            //grid = new Grid(gridDefinition, gameObject);
            if(grid != null)
            {
                grid = new Grid(currentGridDefinition.GridWidth, currentGridDefinition.GridHeight, currentGridDefinition.CellSize, currentGridDefinition.GridSprite, gameObject);
            }
            // int count = 0;
            // for(int x = 0; x < grid.GridArray.GetLength(0); x++)
            // {
            //     for(int y = 0; y < grid.GridArray.GetLength(1); y++)
            //     {
            //         count = x * (currentGridDefinition.GridHeight - 1) + y;
            //         if(grid.GridSprites[x, y].color == currentGridDefinition.GridColorDatas[count].color)
            //         {
            //             correctCell ++;
            //             grid.SetCorrect(x, y, true);
            //         }
            //     }
            // }
            // UpdateAccuracyText();
        }

        private void CheckMouseInput()
        {
            if(DEBUG_canMouseInput)
            {
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        UpdateGridColor(hit.point, currentColor);
                        Debug.Log("HIT");
                    }
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
                }
            }
        }

        public void ChangeColor(Color newColor)
        {
            currentColor = newColor;
        }

        
        public bool UpdateGridColor(Vector3 position, Color colorToChange)
        {
            int x, y;
            if(grid.SetSpriteColor(position, colorToChange, out x, out y))
            {
                //UpdateAccuracy(x, y);
                return true;
            }
            return false;
        }
        // public void UpdateAccuracyText()
        // {
        //     if(accuracyText == null) { return; }
        //     float percentage = (float)correctCell / (float)(currentGridDefinition.GridWidth * currentGridDefinition.GridHeight);
        //     percentage = percentage * 100;
        //     Debug.Log("cell: " + correctCell + "width: " + currentGridDefinition.GridWidth + "height: " + currentGridDefinition.GridHeight + "%: " + percentage);
        //     accuracyText.text = percentage.ToString("0.#\\%");// + "%";
        // }

        // private void UpdateAccuracy(int x, int y)
        // {
        //     int count = x * (currentGridDefinition.GridHeight - 1) + y;
        //     //bad :(
        //     if(grid.GridSprites[x, y].color == currentGridDefinition.GridColorDatas[count].color &&
        //        grid.Value[x, y] == false)
        //     {
        //         correctCell++;
        //         grid.SetCorrect(x, y, true);
        //     }
        //     else if(grid.GridSprites[x, y].color != currentGridDefinition.GridColorDatas[count].color &&
        //             grid.Value[x, y] == true)
        //     {
        //         correctCell--;
        //         grid.SetCorrect(x, y, false);
        //     }
        //     UpdateAccuracyText();
        // }
#if UNITY_EDITOR
        public void DEBUG_GenerateGrid()
        {
            if(DEBUG_hasGenerate) { DEBUG_ResetGrid(); }
            grid = new Grid(defaultGrid.GridWidth, defaultGrid.GridHeight, defaultGrid.CellSize, defaultGrid.GridSprite, gameObject);
            DEBUG_hasGenerate = true;
        }

        public void DEBUG_GenerateGridTemplate()
        {
            if(DEBUG_hasGenerate) { DEBUG_ResetGrid(); }
            if(gridDefinitionsToLoad == null)
            {
                Debug.Log("THERE IS NO GRID TO LOAD!");
                return;
            }
            grid = new Grid(gridDefinitionsToLoad, gameObject);
            grid.SetColor(gridDefinitionsToLoad);
            DEBUG_hasGenerate = true;
        }

        public void DEBUG_UpdateGridDefinition()
        {
            gridDefinitionsToLoad.ResetColorData();
            if(grid != null)
            {
                for(int x = 0; x < grid.GridArray.GetLength(0); x++)
                {
                    for(int y = 0; y < grid.GridArray.GetLength(1); y++)
                    {
                        var recordColor = grid.GridSprites[x, y].color;
                        if(!gridDefinitionsToLoad.UsedColors.Contains(recordColor))
                        {
                            gridDefinitionsToLoad.AddUsedColor(recordColor);
                        }
                        gridDefinitionsToLoad.SetGridSpritesColor(x, y, grid.GridSprites[x, y].color);
                    }
                }
            }
            else
            {
                Debug.Log("THERE IS NO GRID!!!");
            }
        }

        public void DEBUG_ResetGrid()
        {
            if(grid == null) { return; }
            if(grid.Width != 0)
            {
                for(int x = 0; x < grid.GridArray.GetLength(0); x++)
                {
                    for(int y = 0; y < grid.GridArray.GetLength(1); y++)
                    {
                        DestroyImmediate(grid.GridSprites[x, y].gameObject);
                    }
                }
                grid.Reset();
                grid = null;
            }
            else
            {
                var children = GetComponentsInChildren<SpriteRenderer>();
                foreach (var child in children)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
            DEBUG_hasGenerate = false;
        }

        public void DEBUG_CreateGridAsset()
        {
            GridDefinition newGrid = ScriptableObject.CreateInstance<GridDefinition>();
            newGrid.name = "gridTemplate";
            newGrid.Duplicate(grid);
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObject/GridTemplate.asset");
            AssetDatabase.CreateAsset(newGrid, uniqueFileName);
        }
#endif
    }

}
