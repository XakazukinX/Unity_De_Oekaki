using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChecker : MonoBehaviour
{
    [SerializeField] private Slider colorValue_R;
    [SerializeField] private Slider colorValue_G;
    [SerializeField] private Slider colorValue_B;

    [SerializeField] private RawImage colorApplyImage;
    private Color applyColor = Color.black;

    private void Start()
    {
        applyColor = colorApplyImage.color;
    }

    private void Update()
    {
        applyColor = new Color(colorValue_R.value, colorValue_G.value,colorValue_B.value);
        colorApplyImage.color = applyColor;
    }
    
}
