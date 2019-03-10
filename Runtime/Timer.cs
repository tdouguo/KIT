// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kit.Runtime
{
    public class Timer : MonoBehaviour
    { 
        public int totalTime;
        public Text text;
        public string textFormat;
        public UnityAction endAction;
        public UnityAction<int> updateAction;

        private IEnumerator timerIEnumerator;
        private bool isTiming = false;

        #region ... Creator API

        /// <summary>
        /// 创建计时器 (Text)
        /// </summary>
        /// <param name="go">timer 依赖对象</param>
        /// <param name="totalTime">总时间</param>
        /// <param name="text">文本对象 如空会在go对象上挂着Text组件</param>
        /// <param name="textFormat">text 变化时 format字符串</param>
        /// <returns></returns>
        public static Timer CreatorTextTimer(GameObject go, int totalTime, Text text, string textFormat = null, bool isStartTimer = true)
        {
            Timer timer = go.GetOrAddComponent<Timer>();
            timer.totalTime = totalTime;
            timer.text = text ? text : go.GetComponent<Text>();
            timer.textFormat = textFormat;
            if (isStartTimer)
            {
                timer.StartTimer();
            }
            return timer;
        }

        /// <summary>
        /// 创建计时器 (Text)
        /// </summary>
        /// <param name="go"></param>
        /// <param name="totalTime"></param>
        /// <param name="text"></param>
        /// <param name="textFormat"></param>
        /// <param name="endAction"></param>
        /// <param name="updateAction"></param>
        /// <param name="isStartTimer"></param>
        /// <returns></returns>
        public static Timer CreatorTextTimer(GameObject go, int totalTime, Text text, string textFormat, UnityAction endAction,
            UnityAction<int> updateAction, bool isStartTimer = true)
        {
            Timer timer = CreatorTextTimer(go, totalTime, text, textFormat, false);
            timer.endAction = endAction;
            timer.updateAction = updateAction;
            if (isStartTimer)
            {
                timer.StartTimer();
            }
            return timer;
        }

        /// <summary>
        /// 创建计时器
        /// </summary>
        /// <param name="go">依赖对象</param>
        /// <param name="totalTime">总时间</param>
        /// <param name="endAction">停止回掉</param>
        /// <param name="updateAction">每帧更新回掉</param>
        /// <param name="isStartTimer">是否默认开始</param>
        /// <returns></returns>
        public static Timer CreatorTimer(GameObject go, int totalTime, UnityAction endAction,
            UnityAction<int> updateAction, bool isStartTimer = true)
        {
            Timer timer = go.GetOrAddComponent<Timer>();
            timer.totalTime = totalTime;
            timer.endAction = endAction;
            timer.updateAction = updateAction;
            if (isStartTimer)
            {
                timer.StartTimer();
            }
            return timer;
        }

        #endregion

        #region ... Timer API

        /// <summary>
        /// 开始计时
        /// </summary>
        public void StartTimer()
        {
            isTiming = true;
            timerIEnumerator = TimerBody();
            StartCoroutine(timerIEnumerator);
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void StopTimer()
        {
            isTiming = false; 
            if (timerIEnumerator != null)
            {
                StopCoroutine(timerIEnumerator);
                timerIEnumerator = null;
            } 
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        public void PauseTime()
        {
            isTiming = false;
        }

        /// <summary>
        /// 继续计时（取消暂停）
        /// </summary>
        public void ContinueTime()
        {
            isTiming = true;
        }

        /// <summary>
        /// 销毁计时器
        /// </summary>
        public void DestroyTimer()
        {
            StopTimer();
            Destroy(this);
        }

        IEnumerator TimerBody()
        {
            while (totalTime >= 0)
            {
                if (!isTiming)
                {
                    yield return Constant.WaitFPSEnd;
                }
                if (text)
                {
                    text.text = string.IsNullOrEmpty(textFormat) ? totalTime.ToString() : Utility.Text.Format(textFormat, totalTime);
                }
                yield return Constant.WaitNextOneSeconds;
                totalTime--;
                if (updateAction != null)
                {
                    updateAction(totalTime);
                }
            }
            if (endAction != null)
            {
                endAction();
                endAction = null;
            }
            timerIEnumerator = null;
        }

        #endregion
          
    }
}
