using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Oekaki.Brush
{
    public class BrushSwitcher : MonoBehaviour
    {
        public BrushMode targetBrushMode;
        
        public void OnClick()
        {
            BrushManager.Instance.changeBrush(targetBrushMode);
        }
    }
}
