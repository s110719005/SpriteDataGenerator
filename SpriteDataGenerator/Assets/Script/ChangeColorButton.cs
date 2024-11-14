using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorButton : MonoBehaviour
{
    private Button button;
    private Image image;

	void Start () 
    {
		button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
    {
        ColorManager.instance.ChangeColor(image.color);
	}
}
