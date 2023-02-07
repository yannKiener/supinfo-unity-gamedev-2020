using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonSkin : MonoBehaviour
{
    public Color colorToggled;

    private Button button;
    private bool isToggled = false;
    private ColorBlock colorBlockOn;
    private ColorBlock colorBlockOff;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        colorBlockOff = button.colors;
        colorBlockOn = button.colors;
        colorBlockOn.normalColor = colorToggled;
        colorBlockOn.highlightedColor = colorToggled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnToggle()
    {
        isToggled = !isToggled;
        button.colors = isToggled ? colorBlockOn : colorBlockOff;
    }
}
