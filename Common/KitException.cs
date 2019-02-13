// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Kit
{
    /// <summary>
    /// 框架异常类。
    /// </summary>
    [Serializable]
    public class KitException : Exception
    {
        /// <summary>
        /// 初始化框架异常类的新实例。
        /// </summary>
        public KitException()
            : base()
        {

        }

        /// <summary>
        /// 使用指定错误消息初始化框架异常类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public KitException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 使用指定错误消息和对作为此异常原因的内部异常的引用来初始化游戏框架异常类的新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="innerException">导致当前异常的异常。如果 innerException 参数不为空引用，则在处理内部异常的 catch 块中引发当前异常。</param>
        public KitException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 用序列化数据初始化游戏框架异常类的新实例。
        /// </summary>
        /// <param name="info">存有有关所引发异常的序列化的对象数据。</param>
        /// <param name="context">包含有关源或目标的上下文信息。</param>
        protected KitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
