// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using Kit;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kit.Runtime
{
    public class Timer : MonoBehaviour
    {
        private static WaitForSeconds WaitNextOneSeconds = new WaitForSeconds(1);   // 下一秒 迭代器
        private static WaitForEndOfFrame FrameWait = new WaitForEndOfFrame();       // 下一帧迭代器

        public int TotalTime;
        public Text Text;
        public string TextFormat;
        public UnityAction EndAction;
        public UnityAction<int> UpdateAction;

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
            timer.TotalTime = totalTime;
            timer.Text = text ? text : go.GetComponent<Text>();
            timer.TextFormat = textFormat;
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
            timer.EndAction = endAction;
            timer.UpdateAction = updateAction;
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
            timer.TotalTime = totalTime;
            timer.EndAction = endAction;
            timer.UpdateAction = updateAction;
            if (isStartTimer)
            {
                timer.StartTimer();
            }
            return timer;
        }

        #endregion

        #region ... Timer API

        public void StartTimer()
        {
            isTiming = true;
            timerIEnumerator = TimerIEnumerator();
            StartCoroutine(timerIEnumerator);
        }

        public void StopTimer()
        {
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
        /// 效果计时器
        /// </summary>
        public void DestroyTimer()
        {
            StopTimer();
            Destroy(this);
        }

        IEnumerator TimerIEnumerator()
        {
            while (TotalTime >= 0)
            {
                if (!isTiming)
                {
                    yield return FrameWait;
                }
                if (Text)
                {
                    Text.text = string.IsNullOrEmpty(TextFormat) ? TotalTime.ToString() : Utility.Text.Format(TextFormat, TotalTime);
                }
                yield return WaitNextOneSeconds;
                TotalTime--;
                if (UpdateAction != null)
                {
                    UpdateAction(TotalTime);
                }
            }
            if (EndAction != null)
            {
                EndAction();
                EndAction = null;
            }
            timerIEnumerator = null;
        }

        #endregion



        public static Timer Creator(GameObject go, int totalTime = 60, UnityAction endAction = null,
            UnityAction<int> updateAction = null, Text text = null, string textFormat = null)
        {
            Timer timer = go.GetComponent<Timer>();
            if (timer == null)
                timer = go.AddComponent<Timer>();
            timer.TotalTime = totalTime;
            timer.Text = text ? text : go.GetComponent<Text>();
            timer.EndAction = endAction;
            timer.UpdateAction = updateAction;
            timer.StartCountDown();
            return timer;
        }

        public void StartCountDown()
        {
            StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        {
            while (TotalTime >= 0)
            {
                Text.text = TotalTime.ToString() + "秒后重新获取";
                yield return new WaitForSeconds(1);
                TotalTime--;
                if (UpdateAction != null)
                {
                    UpdateAction.Invoke(TotalTime);
                }
            }
            if (EndAction != null)
            {
                EndAction.Invoke();
                EndAction = null;
            }
        }
    }
}
