// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using LitJson;
using System.Collections.Generic;
using UnityEngine;

public static class LitJsonExtension
{
    public static void AddRange(this JsonData jsonData, List<string> list)
    {
        jsonData = new JsonData();
        for (int i = 0; i < list.Count; i++)
        {
            jsonData.Add(list[i]);
        }
    }

    /// <summary>
    /// 对jsondata 直接赋值列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonData"></param>
    /// <param name="list"></param>
    public static void AddRange<T>(this JsonData jsonData, List<T> list)
    {
        jsonData = new JsonData();
        for (int i = 0; i < list.Count; i++)
        {
            jsonData.Add(JsonMapper.ToJson(list[i]));
        }
    }

    /// <summary>
    /// jsondata 赋值指定key的列表
    /// </summary>
    /// <param name="jsonData"></param>
    /// <param name="key"></param>
    /// <param name="list"></param>
    public static void AddRange(this JsonData jsonData, string key, List<string> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogErrorFormat("list is null or count=0. key '{0}'. ", key);
            return;
        }
        jsonData[key] = new JsonData();
        for (int i = 0; i < list.Count; i++)
        {
            jsonData[key].Add(list[i]);
        }
    }

    public static void AddRange(this JsonData jsonData, string key, Dictionary<string, string> dic)
    {
        if (dic == null || dic.Count == 0)
        {
            Debug.LogErrorFormat("dictionary is null or count=0. key  '{0}'.", key);
            return;
        }
        jsonData[key] = new JsonData();
        foreach (var item in dic)
        {
            jsonData[key][item.Key] = item.Value;
        }
    }

    #region ... JsonData To 
    public static List<T> ToList<T>(this JsonData jsonData)
    {
        if (jsonData == null)
            return null;
        string jsonStr = jsonData.ToJson();
        if (string.IsNullOrEmpty(jsonStr))
            return null;
        List<T> list = JsonMapper.ToObject<List<T>>(jsonStr);
        return list;
    }
    public static T[] ToArray<T>(this JsonData jsonData)
    {
        List<T> list = jsonData.ToList<T>();
        return list != null ? list.ToArray() : null;
    }
    #endregion

    public static bool ToBool(this JsonData jsonData, bool defaultValue = default(bool))
    {
        if (jsonData != null)
        {
            bool value;
            if (bool.TryParse(jsonData.ToString(), out value))
            {
                return value;
            }
        }
        return defaultValue;
    }

    public static int ToInt(this JsonData jsonData, int defaultValue = default(int))
    {
        if (jsonData != null)
        {
            int value;
            if (int.TryParse(jsonData.ToString(), out value))
            {
                return value;
            }
        }
        return defaultValue;
    }
}