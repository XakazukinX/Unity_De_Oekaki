using System.Collections;
using System.Collections.Generic;
using Es.InkPainter;
using Es.InkPainter.Sample;
using UnityEngine;
using UnityEngine.UI;

public class ColorSetter : MonoBehaviour
{
    [SerializeField] private RawImage colorApplyImage;

    [SerializeField] private MousePainter painter;
    private Brush playerBrush;

    [SerializeField] private Color defaultColor = Color.black;


    private void Start()
    {
        playerBrush = painter.brush;
        playerBrush.Color = defaultColor;
    }


    public void OnDrop()
    {
        playerBrush.Color = colorApplyImage.color;;
    }
    
}
