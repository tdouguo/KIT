// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kit
{
    /// <summary>
    ///  国际化语言文本
    ///     根据 <L>符合内容</L> 自动匹配对应的多语言内容
    /// </summary>
    public class I18NText : Text
    {
        public bool isAutoConversion = true;
        const string I18N_BEGIN_KEY = "<L>";
        const string I18N_END_KEY = "</L>";

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
#endif
                if (isAutoConversion)
                {
                    RefreshContent();
                }
        }

        /// <summary>
        /// 根据多语言，刷新文本内容
        /// </summary>
        public void RefreshContent()
        { 
            text =  Utility.Misc.ParseText(text, I18N_BEGIN_KEY, I18N_END_KEY,
             (key) =>
             {
                 string i18nValue = "NoValue : " + key;
                 return i18nValue;
             });
        }

        /// <summary>
        /// 动态设置信息
        /// </summary>
        /// <param name="_str"></param>
        public void SetText(string text)
        {
            base.text = text;
            RefreshContent();
        }

        /// <summary>
        /// 设置多语言 key
        /// </summary>
        /// <param name="key"></param>
        public void SetI18N(string key)
        {
            key = I18N_BEGIN_KEY + key + I18N_END_KEY;
            SetText(key);
        }

    }
}
