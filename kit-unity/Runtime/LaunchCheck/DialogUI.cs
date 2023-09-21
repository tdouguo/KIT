using System;
using UnityEngine;
using UnityEngine.UI;

namespace TeamPackage
{
      public class DialogUI : MonoBehaviour
    {
        public Button confirmBtnUI = null;
        public Text confirmBtnTextUI = null;

        public Button cancelBtnUI = null;
        public Text cancelBtnTextUI = null;

        public Button closeBtnUI = null;


        private Action confirmBtnCallback;
        private Action cancelBtnCallback;
        private Action closeBtnCallback;

        private void Awake()
        {
            confirmBtnUI.onClick.AddListener(OnConfirmBtnCallback);
            cancelBtnUI.onClick.AddListener(OnCancelBtnCallback);
            closeBtnUI.onClick.AddListener(OnCloseBtnCallback);
        }

        #region btn Callback

        private void OnCloseBtnCallback()
        {
            closeBtnCallback?.Invoke();
        }

        private void OnCancelBtnCallback()
        {
            cancelBtnCallback?.Invoke();
        }

        private void OnConfirmBtnCallback()
        {
            confirmBtnCallback?.Invoke();
        }

        #endregion


        /// <summary>
        /// 打开对话框，如果按钮
        /// </summary>
        /// <param name="title"></param>
        /// <param name="context"></param>
        /// <param name="_confirmBtnCallback">确定按钮回掉，如果为空则不显示按钮</param>
        /// <param name="confirmBtnText">确定按钮文本</param>
        /// <param name="_cancelBtnCallback">取消按钮回掉，如果为空则不限时按钮</param>
        /// <param name="cancelBtnText">取消按钮文本</param>
        /// <param name="isEnableCloseBtn">是否显示关闭按钮</param>
        public void OpenDialog(string title, string context,
            Action _confirmBtnCallback, string confirmBtnText,
            Action _cancelBtnCallback, string cancelBtnText,
            Action _closeBtnCallback)
        {
            if (_confirmBtnCallback == null && _cancelBtnCallback == null && _closeBtnCallback == null)
            {
                Debug.LogErrorFormat("逻辑错误，当前没有显示按钮，也没有显示关闭");
                return;
            }

            // 确认按钮
            if (_confirmBtnCallback != null)
            {
                confirmBtnTextUI.text = confirmBtnText;
                confirmBtnCallback = _confirmBtnCallback;
            }
            else
            {
                confirmBtnUI.gameObject.SetActive(false);
            }


            // 取消按钮
            if (_cancelBtnCallback != null)
            {
                confirmBtnTextUI.text = confirmBtnText;
                confirmBtnCallback = _cancelBtnCallback;
            }
            else
            {
                confirmBtnUI.gameObject.SetActive(false);
            }


            // 右上角关闭按钮
            closeBtnUI.gameObject.SetActive(_closeBtnCallback != null);
            closeBtnCallback = _closeBtnCallback;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            confirmBtnCallback = null;
            cancelBtnCallback = null;
            closeBtnCallback = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开1个按钮的对话框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="context"></param>
        /// <param name="_confirmBtnCallback">确定按钮回掉</param>
        /// <param name="confirmBtnText">确定按钮</param>
        /// <param name="isEnableCloseBtn">是否显示关闭按钮</param>
        public void OpenDialog(string title, string context,
            Action _confirmBtnCallback, string confirmBtnText, Action closeCallback)
        {
            OpenDialog(title, context,
                _confirmBtnCallback, confirmBtnText,
                null, string.Empty,
                closeCallback
            );
        }
    }
}