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
    ///  UGUI 事件监听
    /// </summary>
    public class UGUIEventListener : MonoBehaviour,
                                    IPointerClickHandler,
                                    IPointerDownHandler,
                                    IPointerEnterHandler,
                                    IPointerExitHandler,
                                    IPointerUpHandler
    {
        public delegate void VoidDelegate(GameObject go);

        public string mAudioType = string.Empty;
        public object[] Args = null;
        public VoidDelegate onClick = null;
        public VoidDelegate onDown = null;
        public VoidDelegate onEnter = null;
        public VoidDelegate onExit = null;
        public VoidDelegate onUp = null;
        public VoidDelegate onLongPress = null;

        private bool isUp = true;     // 与长按配合使用
        private float time = 0;

        #region ... Unity API
        private void Update()
        { // 长按 
            if (!isUp && onLongPress != null && Time.realtimeSinceStartup - time > 1)
            {
                onLongPress(gameObject);
                isUp = true;
            }
        }

        #endregion

        #region ... PointerHandler
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

        public static UGUIEventListener Get(GameObject go, string clickAudio = "BtnClick", params object[] args)
        {
            UGUIEventListener listener = go.GetComponent<UGUIEventListener>();
            if (listener == null) listener = go.AddComponent<UGUIEventListener>();
            listener.mAudioType = clickAudio;
            listener.Args = args;
            return listener;
        }

        public static void AddClicks(GameObject[] gos, VoidDelegate onClick, string clickAudio = "BtnClick")
        {
            if (gos != null && gos.Length > 0)
            {
                for (int i = 0; i < gos.Length; i++)
                {
                    if (gos[i])
                        Get(gos[i], clickAudio).onClick = onClick;
                }
            }
        }
    }
}
