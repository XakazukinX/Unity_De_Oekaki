using System.Collections;
using System.Collections.Generic;
using Es.InkPainter;
using Es.InkPainter.Sample;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeSetter : MonoBehaviour
{
    [SerializeField] private MousePainter painter;
    [SerializeField] private Slider brushScaleSlider;
    
    [SerializeField] private float defaultScale = 0.1f;
    
    private Brush playerBrush;


    private void Start()
    {
        playerBrush = painter.brush;
        playerBrush.Scale = defaultScale;
    }


    public void OnDrop()
    {
        playerBrush.Scale = brushScaleSlider.value;
    }
}
