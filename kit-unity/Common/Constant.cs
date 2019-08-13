// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Kit
{
    public static partial class Constant
    {

        #region ... Constant

        /// <summary>
        /// 等待当前帧结束
        /// </summary>
        public static WaitForEndOfFrame WaitFPSEnd = new WaitForEndOfFrame();

        /// <summary>
        /// 等待一秒
        /// </summary>
        public static WaitForSeconds WaitNextOneSeconds = new WaitForSeconds(1); 

        #endregion

    }
}