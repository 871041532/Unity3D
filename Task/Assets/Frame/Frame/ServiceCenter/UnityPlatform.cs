// -----------------------------------------------------------------
// File:    Platform.cs
// Author:  mouguangyi
// Date:    2016.04.07
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameBox.Framework
{
    /// <summary>
    /// @details Unity路径工具，根据平台来获取相应的路径格式。
    /// </summary>
    public static class PathUtility
    {
        /// <summary>
        /// App的外部数据目录。
        /// </summary>
        public static string DataFolder
        {
            get {
                return Application.persistentDataPath + "/";
            }
        }

        /// <summary>
        /// 获取App内数据文件路径。
        /// </summary>
        /// <param name="fileRelativePath">文件相对路径。</param>
        /// <returns>返回生成的Application内全路径。</returns>
        public static string ComposeAppPath(string fileRelativePath)
        {
            var folder = "";
            switch (Application.platform) {
            case RuntimePlatform.Android:
                folder = Application.dataPath + "!assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                folder = Application.dataPath + "/Raw/";
                break;
            default:
                folder = Application.dataPath + "/StreamingAssets/";
                break;
            }

            return folder + fileRelativePath;
        }

        /// <summary>
        /// 获取App内数据URL路径。
        /// </summary>
        /// <param name="fileRelativePath">文件相对路径。</param>
        /// <returns>返回生成的Application内URL。</returns>
        public static string ComposeAppUrl(string fileRelativePath)
        {
            var url = "";
            switch (Application.platform) {
            case RuntimePlatform.Android:
                url = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                url = "file://" + Application.dataPath + "/Raw/";
                break;
            default:
                url =  "file://" + Application.dataPath + "/StreamingAssets/";
                break;
            }

            return url + fileRelativePath;
        }

        /// <summary>
        /// 获取App外数据文件路径。
        /// </summary>
        /// <param name="fileRelativePath">文件相对路径。</param>
        /// <returns>返回生成的Application外的存储全路径。</returns>
        public static string ComposeDataPath(string fileRelativePath)
        {
            return DataFolder + fileRelativePath;
        }

        /// <summary>
        /// 获取App外数据URL路径。
        /// </summary>
        /// <param name="fileRelativePath">文件相对路径。</param>
        /// <returns>返回生成的Application外的存储URL。</returns>
        public static string ComposeDataUrl(string fileRelativePath)
        {
            return "file:///" + DataFolder + fileRelativePath;
        }
    }

    /// <summary>
    /// @details EventTrigger工具，提供一种通用的鼠标/手指触发UI响应的流程。
    /// </summary>
    public static class EventTriggerUtility
    {
        /// <summary>
        /// 为指定的EventTrigger添加指定EventTriggerType类型的响应函数。
        /// </summary>
        /// <param name="target">EventTrigger的目标。</param>
        /// <param name="type">EventTrigger类型。</param>
        /// <param name="action">处理该消息的函数句柄。</param>
        public static void Add(EventTrigger target, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            if (null == target) {
                return;
            }

            if (null == target.triggers) {
                target.triggers = new List<EventTrigger.Entry>();
            }

            var entry = new EventTrigger.Entry {
                eventID = type,
                callback = new EventTrigger.TriggerEvent(),
            };
            entry.callback.AddListener(action);
            target.triggers.Add(entry);
        }

        /// <summary>
        /// 为指定的EventTrigger删除指定EventTriggerType类型的响应函数。
        /// </summary>
        /// <param name="target">EventTrigger的目标。</param>
        /// <param name="type">EventTrigger类型。</param>
        /// <param name="action">处理该消息的函数句柄。</param>
        public static void Remove(EventTrigger target, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            if (null == target || null == target.triggers) {
                return;
            }

            for (var i = target.triggers.Count - 1; i >= 0; --i) {
                var entry = target.triggers[i];
                if (entry.eventID == type) {
                    entry.callback.RemoveListener(action);
                    target.triggers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 清理指定的EventTrigger上的响应函数。
        /// </summary>
        /// <param name="target">EventTrigger的目标。</param>
        public static void Clear(EventTrigger target)
        {
            if (null == target) {
                return;
            }

            if (null != target.triggers) {
                target.triggers.Clear();
            }
        }
    }

    /// <summary>
    /// @details 密码相关工具。
    /// </summary>
    public static class CryptoUtility
    {
        /// <summary>
        /// 生成MD5码。
        /// </summary>
        /// <param name="bytes">字节流。</param>
        /// <returns>返回MD5码。</returns>
        public static string ComputeMD5Code(byte[] bytes)
        {
            var md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(bytes, 0, bytes.Length);

            var builder = new StringBuilder();
            for (var i = 0; i < result.Length; ++i) {
                builder.Append(result[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
