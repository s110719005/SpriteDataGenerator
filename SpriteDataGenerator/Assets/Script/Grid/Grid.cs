using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GridSystem
{
    [System.Serializable]
    public class Grid
    {
        private int width;
        public int Width => width;
        private int height;
        public int Height => height;
        private float cellSize;
        public float CellSize => cellSize;
        private Sprite gridSprite;
        public Sprite GridSprite => gridSprite;
        private GameObject parent;
        public GameObject Parent => parent;
        private int[,] gridArray;
        public int[,] GridArray => gridArray;
        private SpriteRenderer[,] gridSprites;
        public SpriteRenderer[,] GridSprites => gridSprites;

        public Grid(int width, int height, float cellSize, Sprite gridSprite, GameObject parent)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.parent = parent;
            this.gridSprite = gridSprite;

            InitGrid();
        }

        public Grid(GridDefinition gridDefinition, GameObject parent)
        {
            this.width = gridDefinition.GridWidth;
            this.height = gridDefinition.GridHeight;
            this.cellSize = gridDefinition.CellSize;
            this.gridSprite = gridDefinition.GridSprite;
            this.parent = parent;
       
            InitGrid();
        }

        private void InitGrid()
        {
            gridArray = new int[width, height];
            gridSprites = new SpriteRenderer[width, height];
            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for(int y = 0; y < gridArray.GetLength(1); y++)
                {
                    GameObject gameObject = new GameObject("gridObject", typeof(SpriteRenderer));
                    gridSprites[x, y] = gameObject.GetComponent<SpriteRenderer>();
                    gridSprites[x, y].sprite = gridSprite;
                    gridSprites[x, y].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    Transform transform = gameObject.transform;
                    transform.SetParent(parent.transform, false);
                    transform.localPosition = GetWorldPosition(x, y) + new Vector3(cellSize / 2, 0, cellSize / 2);
                    transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                    transform.localScale = new Vector3(10, 10);
                    gameObject.name = "gridObject: " + x + ", " + y;

                    gridArray[x, y] = 0;
                }
            }
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * cellSize;
        } 

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            if(parent)
            {
                x = Mathf.FloorToInt((worldPosition.x - parent.transform.position.x) / cellSize);
                y = Mathf.FloorToInt((worldPosition.z - parent.transform.position.z) / cellSize);
            }
            else
            {
                x = Mathf.FloorToInt(worldPosition.x / cellSize);
                y = Mathf.FloorToInt(worldPosition.z / cellSize);
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            if(x < 0 || x >= width) { return; }
            if(y < 0 || y >= height) { return; }
            SetValue(x, y, value);
        }
        private void SetValue(int x, int y, int value)
        {
            gridArray[x, y] = value;
        }

        public bool SetSpriteColor(Vector3 worldPosition, Color color, out int positionX, out int positionY)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            positionX = x;
            positionY = y;

            if(x < 0 || x >= width) { return false; }
            if(y < 0 || y >= height) { return false; }
            return SetSpriteColor(x, y, color);
        }

        public bool SetSpriteColor(int x, int y, Color color)
        {
            if(gridSprites[x, y].color == color) { return false; }
            gridSprites[x, y].color = color;
            return true;
            
        }


        public void Reset()
        {
            width = 0;
            height = 0;
            cellSize = 0;
            parent = null;
            gridSprite = null;
            gridArray = null;
            gridSprites = null;
        }

        public void SetColor(GridDefinition gridDefinition)
        {
            int count = 0;
            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for(int y = 0; y < gridArray.GetLength(1); y++)
                {
                    if(count >= gridDefinition.GridColorDatas.Count) { return; }
                    gridSprites[x, y].color = gridDefinition.GridColorDatas[count].color;
                    count++;
                }
            }
        }
    }
}
