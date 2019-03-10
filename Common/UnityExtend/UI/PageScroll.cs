// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kit
{
    public class PageScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private ScrollRect scrollRect;
        private float targetHorizontalPosition = 0f;
        private bool isDraging = false;
        [SerializeField] private float[] pageRatio = null;

        public float speed = 5f;
        public int maxPage = 1;
        public int page = 0;
        public UnityAction<PageScroll> onSwitchPageEvent = null;

        private void Awake()
        {
            scrollRect = transform.GetComponent<ScrollRect>();

            float _meanRatio = 1f / (maxPage - 1);
            pageRatio = new float[maxPage];
            float _tempRatio = 0;
            for (int i = 0; i < pageRatio.Length; i++)
            {
                pageRatio[i] = _tempRatio;
                _tempRatio += _meanRatio;
            }
        }
        private void Update()
        {
            //如果不判断。当在拖拽的时候要也会执行插值，所以会出现闪烁的效果
            //这里只要在拖动结束的时候。在进行插值
            if (!isDraging)
            {
                scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition,
                    targetHorizontalPosition, Time.deltaTime * speed);
            }
        }

        #region .... EventSystem
        /// <summary>
        /// 拖动开始
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDraging = true;
        }
        #endregion

        /// &lt;summary&gt;
        /// 拖拽结束
        /// &lt;/summary&gt;
        /// &lt;param name="eventData"&gt;&lt;/param&gt;
        public void OnEndDrag(PointerEventData eventData)
        {
            isDraging = false;
            // 得到 水平滑动的 值  （0-1）
            float posX = scrollRect.horizontalNormalizedPosition;
            int index = 0;
            float offset = Mathf.Abs(posX - pageRatio[index]);
            // 与 前后比较 距离最短
            for (int i = 1; i < pageRatio.Length; i++)
            {
                // 距离 最短
                float offsetTemp = Mathf.Abs(posX - pageRatio[i]);
                if (offset > offsetTemp)
                {
                    index = i;
                    offset = offsetTemp;
                }
            }
            targetHorizontalPosition = pageRatio[index];
            //ToggleArray[index].isOn = true;
        }
    }
}
