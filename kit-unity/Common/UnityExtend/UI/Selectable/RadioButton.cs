// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kit
{
    /// <summary>
    /// 当前按钮点击后控制 另外2个按钮显示隐藏
    /// </summary>
    public class RadioButton : Selectable
    {
        public bool isTrue;
        public GameObject trueOption;
        public GameObject falseOption;
        public UnityAction<bool> onValueChange = null;
        protected override void Awake()
        {
            UGUIEventListener.Get(gameObject).onClick += delegate
            {
                isTrue = !isTrue;
                if (trueOption)
                    trueOption.SetActive(isTrue);
                if (falseOption)
                    falseOption.SetActive(!isTrue);
                if (onValueChange != null)
                    onValueChange(isTrue);
            };
        }

        protected override void Start()
        {
            if (trueOption)
                trueOption.SetActive(isTrue);
            if (falseOption)
                falseOption.SetActive(!isTrue);
        }

        public void SetValue(bool isTrue)
        {
            this.isTrue = isTrue;
            if (trueOption)
                trueOption.SetActive(isTrue);
            if (falseOption)
                falseOption.SetActive(!isTrue);
            if (onValueChange != null)
                onValueChange(isTrue);
        }
    }
}

