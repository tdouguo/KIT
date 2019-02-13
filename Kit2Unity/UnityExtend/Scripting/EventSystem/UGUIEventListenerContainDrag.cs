// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kit
{
    /// <summary>
    ///  UGUI 事件 监听 并且包含拖拽
    /// </summary>
    public class UGUIEventListenerContainDrag : MonoBehaviour,
                                   IPointerClickHandler,
                                   IPointerDownHandler,
                                   IPointerEnterHandler,
                                   IPointerExitHandler,
                                   IPointerUpHandler,
                                   IBeginDragHandler,
                                   IDragHandler,
                                   IEndDragHandler
    {
        public delegate void VoidDelegate(GameObject go);
        public delegate void VoidDelegateDrag(GameObject go, PointerEventData eventData);
        public string mAudioType;

        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onDragStart;
        public VoidDelegateDrag onDrag;
        public VoidDelegate onDragEnd;
        public VoidDelegate onLongPress;

        private bool isUp = true;   // 与长按配合使用
        private float time = 0;

        #region ... Unity Api

        private void Update()
        {
            //长按
            if (!isUp && onLongPress != null && Time.realtimeSinceStartup - time > 1)
            {
                onLongPress(gameObject);
                isUp = true;
            }
        }

        #endregion

        #region ... IPointerHandler
        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                if (!string.IsNullOrEmpty(mAudioType) && GetComponent<Button>() != null && GetComponent<Button>().interactable)
                {
                    // TODO : 播放声音 mAudioType 
                }
                if (GetComponent<Button>() == null || GetComponent<Button>().interactable)
                    onClick(gameObject);
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
            if (isUp) time = Time.realtimeSinceStartup;
            isUp = false;
        }
        public void OnPointerEnter(PointerEventData eventData) { if (onEnter != null) onEnter(gameObject); }
        public void OnPointerExit(PointerEventData eventData) { if (onExit != null) onExit(gameObject); }
        public void OnPointerUp(PointerEventData eventData) { if (onUp != null) onUp(gameObject); isUp = true; time = 0; }
        #endregion

        #region ... IDragHandler
        public void OnBeginDrag(PointerEventData eventData) { if (onDragStart != null) onDragStart(gameObject); }
        public void OnDrag(PointerEventData eventData) { if (onDrag != null) onDrag(gameObject, eventData); }
        public void OnEndDrag(PointerEventData eventData) { if (onDragEnd != null) onDragEnd(gameObject); }
        #endregion

        public static UGUIEventListenerContainDrag Get(GameObject go, string clickAudio = "BtnClick")
        {
            UGUIEventListenerContainDrag listener = go.GetComponent<UGUIEventListenerContainDrag>();
            if (listener == null) listener = go.AddComponent<UGUIEventListenerContainDrag>();
            listener.mAudioType = clickAudio;
            return listener;
        }

    }
}
