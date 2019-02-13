// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Kit
{
    /// <summary>
    /// 
    /// </summary>
    public class ToggleGradientColor : MonoBehaviour
    {
        public Toggle target;
        public Graphic[] graphics;
        public Color targetGraphicColor, graphicColor;
        private void Reset()
        {
            target = GetComponent<Toggle>();
        }
        private void Awake()
        {
            target.onValueChanged.AddListener(OnValueChangedCallback);
            OnValueChangedCallback(target.isOn);
        }

        private void OnValueChangedCallback(bool value)
        {
            SetColor(value ? graphicColor : targetGraphicColor);
        }
        private void SetColor(Color color)
        {
            if (graphics != null && graphics.Length > 0)
            {
                for (int i = 0; i < graphics.Length; i++)
                {
                    if (graphics[i] != null)
                        graphics[i].color = color;
                }
            }
        }
    }
}