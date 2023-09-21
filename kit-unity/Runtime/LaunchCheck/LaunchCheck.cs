using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TeamPackage
{


    public class LaunchCheck : MonoBehaviour
    {
        // 多语言
        private const string LANG_NET_ERR_TITLE = "网络错误";
        private const string LANG_NET_ERR_CONTEXT = "网络异常,请检查网络连接!";
        private const string LANG_QUIT_TITLE = "退出游戏";
        private const string LANG_QUIT_CONTEXT = "确定要退出游戏吗?"; // 可以调整大小 <size=68>确定要退出游戏吗?</size>

        // UI
        private DialogUI dialogUI;


        public void LoadRemoteConfig(Action<RemoteConfig> callback)
        {
            string url = $"https://cdn-oss.dawenyu.net/Config/NightmareWorld/{Application.version}/config.json";
            Tool.RequestText(url, (text) =>
            {
                RemoteConfig config = JsonConvert.DeserializeObject<RemoteConfig>(text);
                callback?.Invoke(config);
            });
        }

        #region 方法

        /// <summary>
        /// 检查更新
        ///     如果 isUpdate= true 打开更新对话框
        ///         如果 isForce = true 打开强制更新对话框
        ///             只有 立即更新按钮 点击走更新逻辑
        ///         如果 isForce = false 打开非强制更新对话框
        ///             立即更新按钮
        ///             跳过更新按钮或者X 关闭对话框
        /// </summary>
        /// <param name="data"></param>
        private void CheckForceUpdate(ConfigVersionData data)
        {
            if (data.IsUpdate)
            {
                if (data.IsForceUpdate)
                {
                    dialogUI.OpenDialog(data.UpdateTitle, data.UpdateContext,
                        () => { Application.OpenURL(data.UpdateUrl); },
                        "立即更新", null);
                }
                else
                {
                    dialogUI.OpenDialog(data.UpdateTitle, data.UpdateContext,
                        () => { Application.OpenURL(data.UpdateUrl); },
                        "立即更新",
                        () => { dialogUI.Close(); }, "跳过更新", dialogUI.Close);
                }
            }
        }

        /// <summary>
        /// 检查网络
        ///     如果有网，则进行回掉
        ///     如果没网，弹出没网对话框（只显示 重新尝试按钮和X）
        /// 没网对话框
        ///     点击重新尝试 走 检查网络逻辑
        ///     点击X 打开退出对话框
        /// 退出对话框
        ///     左边 退出游戏按钮、右边 继续游戏按钮、右上方X
        ///     点击退出游戏 退出游戏
        ///     点击继续游戏或X 走 检查网络逻辑
        /// </summary>
        /// <param name="callback"></param>
        public void CheckNet(Action callback)
        {
            if (Tool.IsNetworkReachability())
            {
                callback?.Invoke();
            }
            else
            {
                OpenNetErrorDialog(() =>
                {
                    CloseNetErrorDialog();
                    CheckNet(callback);
                }, true);
            }
        }

        public void CheckTime()
        {
            GetNetTime(netTime =>
            {
                // 当前时间戳和服务器时间戳 差3分钟以上则判断 时间错误
                long nowTime = Tool.GetTimeStampByDateTime(DateTime.Now);
                bool isTimeErr = (netTime - nowTime) >= 3 * 60 * 1000;
#if GAME_DEBUG||UNITY_EDITOR
                isTimeErr = false;
#endif
                if (isTimeErr)
                {
                    dialogUI.OpenDialog("时间错误", "系统时间与网络时间误差较大\r\n请前往修改系统时间!",
                        () =>
                        {
                            dialogUI.Close();
                            CheckTime();
                        }, "重新验证", null);
                }
            });
        }

        #endregion

        #region 对话框扩展

        // 网络错误 对话框
        public bool isOpenNetError;

        private void OpenNetErrorDialog(Action confirmCallback, bool isCloseOpenQuit)
        {
            isOpenNetError = true;
            dialogUI.OpenDialog(LANG_NET_ERR_TITLE, LANG_NET_ERR_CONTEXT,
                confirmCallback, "重新尝试",
                () =>
                {
                    // 点就右上角关闭按钮，弹出退出游戏提示
                    CloseNetErrorDialog();
                    if (isCloseOpenQuit)
                    {
                        OpenQuitDialog(confirmCallback, confirmCallback); // 打开退出对话框
                    }
                }
            );
        }

        private void CloseNetErrorDialog()
        {
            isOpenNetError = false;
            dialogUI.Close();
        }


        // 退出 对话框
        public bool isOpenQuit = false;

        private void OpenQuitDialog(Action continueAction, Action closeAction)
        {
            isOpenQuit = true;
            dialogUI.OpenDialog(LANG_QUIT_TITLE, LANG_QUIT_CONTEXT,
                Tool.QuitApp, "退出游戏",
                () =>
                {
                    CloseQuitDialog();
                    continueAction?.Invoke();
                }, "继续游戏",
                () =>
                {
                    CloseQuitDialog();
                    closeAction?.Invoke();
                }
            );
        }

        private void CloseQuitDialog()
        {
            isOpenQuit = false;
        }

        #endregion

        #region Tool

        private void GetNetTime(Action<long> onCompleted)
        {
            CheckNet(() =>
            {
                Tool.GetNetTime((time) =>
                {
                    if (time > 0)
                    {
                        onCompleted?.Invoke(time);
                    }
                    else
                    {
                        dialogUI.OpenDialog("时间错误", "获取时间错误,请稍后重试!",
                            () =>
                            {
                                dialogUI.Close();
                                GetNetTime(onCompleted);
                            }, "重试", null);
                    }
                });
            });
        }

        #endregion
    }
}