using System.Collections;
using System.Collections.Generic;
using GridSystem;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator;

    public static ColorManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ChangeColor(Color newColor)
    {
        if(gridGenerator == null) { Debug.Log("NO GRID GENERATOR!!"); return; }
        gridGenerator.ChangeColor(newColor);
    }

    public void Test()
    {

    }

}
