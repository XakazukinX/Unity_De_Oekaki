using Es.InkPainter;
using Es.InkPainter.Sample;
using UnityEngine;
using UnityEngine.UI;

namespace Oekaki.Brush
{
    public class BrushManager : SingletonMonoBehaviour<BrushManager>
    {
        [SerializeField] private MousePainter playerPainter;
        [SerializeField] private Image targetImageUI;
        [SerializeField] private Sprite penSprite;
        [SerializeField] private Sprite eraserSprite;

        public BrushMode brushMode;


        private void Start()
        {
            playerPainter.erase = false;
            brushMode = BrushMode.PEN;
            targetImageUI.sprite = penSprite;
        }


        internal void changeBrush(BrushMode nextMode)
        {
            switch (nextMode)
            {
                case BrushMode.PEN:
                    brushChangeToPen();
                    break;
                case BrushMode.ERASER:
                    brushChangeToEraser();
                    break;
                default:
                    break;
            }
        }

        private void brushChangeToPen()
        {
            //すでにペンが選択されていたら何もしない
            if (brushMode == BrushMode.PEN)
            {
                return;
            }
            playerPainter.erase = false;
            brushMode = BrushMode.PEN;
            targetImageUI.sprite = penSprite;
        }
    
        private void brushChangeToEraser()
        {
            //すでに消しゴムが選択されていたら何もしない
            if (brushMode == BrushMode.ERASER)
            {
                return;
            }
            playerPainter.erase = true;
            brushMode = BrushMode.ERASER;
            targetImageUI.sprite = eraserSprite;
        }
    }
}
