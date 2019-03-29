using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

/// <summary>
/// 转换操作工具类
///	by 浩深
/// </summary>
public class ConvertTool {
	
	/// <summary>
	/// 字符串转换为任意类型数组
	/// </summary>
	/// <returns>指定类型数组.</returns>
	/// <param name="str">要转换的字符串.</param>
	/// <param name="split">分割字符.</param>
	/// <typeparam name="T">任意类型.</typeparam>
	public static T[] StringToAnyTypeArray<T>(string str, char split)
	{
		if(string.IsNullOrEmpty(str))
			return null;
		
		string[] strArray = str.Split(split);
		T[] convertArray = new T[strArray.Length];
		for(int i= 0; i < strArray.Length; i++)
		{
			convertArray[i] = (T)Convert.ChangeType(strArray[i],typeof(T));
		}
		
		return convertArray;
	}
	
	/// <summary>
	/// 字符串转换为任意类型列表
	/// </summary>
	/// <returns>The to any type list.</returns>
	/// <param name="str">String.</param>
	/// <param name="split">Split.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static List<T> StringToAnyTypeList<T>(string str, char split)
	{
		if(string.IsNullOrEmpty(str))
			return null;
		
		string[] strArray = str.Split(split);
		List<T> convertList = new List<T>();
		
		for(int i= 0; i < strArray.Length; i++)
		{
			convertList.Add((T)Convert.ChangeType(strArray[i],typeof(T)));
		}
		
		return convertList;
		
	}

    /// <summary>
    /// 字符串转为任意类型的枚举
    /// </summary>
    /// <returns>The to any enum list.</returns>
    /// <param name="str">String.</param>
    /// <param name="split">Split.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static List<T> StringToAnyEnumList<T>(string str, char split)
    {
        if(string.IsNullOrEmpty(str))
            return null;

        string[] strArray = str.Split(split);
        List<T> convertList = new List<T>();

        for(int i= 0; i < strArray.Length; i++)
        {
            convertList.Add((T)Enum.Parse(typeof(T),strArray[i]));
        }

        return convertList;

    }

    /// <summary>
    /// 字符串转换为任意类型列表
    /// </summary>
    public static HashSet<T> StringToAnyTypeHash<T>(string str, char split)
    {
        if(string.IsNullOrEmpty(str))
            return null;

        string[] strArray = str.Split(split);
        HashSet<T> convertList = new HashSet<T>();

        for(int i= 0; i < strArray.Length; i++)
        {
            convertList.Add((T)Convert.ChangeType(strArray[i],typeof(T)));
        }

        return convertList;

    }
	
	/// <summary>
	/// 任意类型转为字符串.
	/// </summary>
	/// <returns>转换后的字符串.</returns>
	/// <param name="tArray">任意类型数组.</param>
	/// <param name="split">分割符.</param>
	/// <typeparam name="T">任意类型.</typeparam>
	public static string AnyTypeArrayToString<T>(T[] tArray, string split)
	{
		if(tArray == null)
			return null;
		
		StringBuilder sbTemp = new StringBuilder();
		int length = tArray.Length;
		for(int i = 0; i < length; i ++)
		{
			if(i != 0)
				sbTemp.Append(split);
			sbTemp.Append(tArray[i].ToString());
		}
		return sbTemp.ToString();
	}

	/// <summary>
	/// 任意类型数组转为字符串数组
	/// </summary>
	/// <returns>The type array to string array.</returns>
	/// <param name="tArray">T array.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static string[] AnyTypeArrayToStringArray<T>(T[] tArray)
	{
		if(tArray == null)
			return null;
		string[] strArray = new string[tArray.Length];
		for (int i=0; i<strArray.Length; ++i) 
		{
			strArray[i] = tArray[i].ToString();
		}

		return strArray;
	}
	
	/// <summary>
	/// 任意类型数组转换为列表.
	/// </summary>
	/// <returns>The type array to list.</returns>
	/// <param name="tArray">T array.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static List<T> AnyTypeArrayToList<T>(T[] tArray)
	{
		if(tArray == null)
			return null;
			
		List<T> listTemp = new List<T>();
		int length = tArray.Length;
		for(int i = 0; i < length; i++)
		{
			listTemp.Add(tArray[i]);
		}
		
		return listTemp;
	}
	
	/// <summary>
	/// 任意类型列表转为字符串.
	/// </summary>
	/// <returns>The type list to string.</returns>
	/// <param name="tList">T list.</param>
	/// <param name="split">Split.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static string AnyTypeListToString<T>(List<T> tList, string split)
	{
		if(tList == null)
			return null;
			
		int count = tList.Count;
		StringBuilder sbTemp = new StringBuilder();
		for(int i = 0; i < count; i++)
		{
			if(i != 0)
				sbTemp.Append(split);
			sbTemp.Append(tList[i].ToString());
		}
		
		return sbTemp.ToString();
	}
	
	/// <summary>
	/// 字符串转为 vector3. haveBrackets = true 表示字条串包含括号
	/// </summary>
	/// <returns>The to vector3.</returns>
	/// <param name="str">String.</param>
	/// <param name="split">Split.</param>
	/// <param name="haveBrackets">If set to <c>true</c> have brackets.</param>
	public static Vector3 StringToVector3(string str, char split, bool haveBrackets = true)
	{
		//去除（x^y^z）中的括号
		if(haveBrackets)
		{
			str = str.Remove (0, 1);
			str = str.Remove (str.Length - 1, 1);
		}

		string[] strTemp = str.Split(split);
		float x = float.Parse(strTemp[0]);
		float y = float.Parse(strTemp[1]);
		float z = float.Parse(strTemp[2]);
		Vector3 vecTemp = new Vector3(x,y,z);
		return vecTemp;
	}
	
	/// <summary>
	/// 字符串转换为Vector3数组
	/// </summary>
	/// <returns>The to vector3 array.</returns>
	/// <param name="str">String.</param>
	/// <param name="split">Split.</param>
	public static Vector3[] StringToVector3Array(string str, char split, char vectorSplit)
	{
		if(string.IsNullOrEmpty(str))
			return null;
	
		string[] strTemps = str.Split(split);
		int length = strTemps.Length;
		Vector3[] vecTemps = new Vector3[length];
		for(int i = 0; i < length; i++)
		{
			vecTemps[i] = StringToVector3(strTemps[i], vectorSplit);
		}
		return vecTemps;
	}
	
	/// <summary>
	/// 向任意类型数组后面增加一个元素.
	/// </summary>
	/// <returns>The add item.</returns>
	/// <param name="array">Array.</param>
	/// <param name="newItem">New item.</param>
	public static T[] AnyTypeArrayAddItem<T>(T[] array, T newItem)
	{
		T[] newArray;
		if(array == null)
		{
			newArray = new T[1];
			newArray[0] = newItem;
		}
		else
		{
			int length = array.Length, i;
			newArray = new T[length + 1];
			for(i = 0; i < array.Length; i ++)
				newArray[i] = array[i];
			newArray[i] = newItem;
		}
		return newArray;
	}

	/// <summary>
	/// 判断任意类型数组是否包含某个值
	/// </summary>
	/// <returns><c>true</c>, if type array contain item was anyed, <c>false</c> otherwise.</returns>
	/// <param name="array">Array.</param>
	/// <param name="item">Item.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static bool AnyTypeArrayContainItem<T>(T[] array, T item)
	{
		List<T> tList = AnyTypeArrayToList<T> (array);
		if(tList != null && tList.Contains(item))
		   return true;
		else
		   return false;
	}

	/// <summary>
	/// 向任意类型数组中删除一个特定的元素（条件：数组中的元素全都不同）
	/// </summary>
	/// <returns>The add item.</returns>
	/// <param name="array">Array.</param>
	/// <param name="newItem">New item.</param>
	public static T[] AnyTypeArrayRemoveItem<T>(T[] array, T oldItem)
	{
		T[] newArray;
		if(array == null)
			return null;

		int length = array.Length, i, j;
		newArray = new T[length - 1];
		for(i = 0, j = 0; i < array.Length; i ++)
		{
			if(array[i].Equals(oldItem))
				continue;
			else
				newArray[j++] = array[i];
		}
		return newArray;
	}
	
	/// <summary>
	/// 根据下标删除任意类型数组的元素
	/// </summary>
	/// <returns>The type array remove item by index.</returns>
	/// <param name="array">Array.</param>
	/// <param name="index">Index.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T[] AnyTypeArrayRemoveItemByIndex<T>(T[] array, int index)
	{
		if(array == null || index < 0 || index > array.Length)
			return array;
			
		T[] newArray = new T[array.Length - 1];
		for(int i = 0, j = 0; i < array.Length; i++)
		{
			if(i == index)
				continue;
			newArray[j] = array[i];
			j++;
		}
		return newArray;
	}
	
	/// <summary>
	/// 删除列表中的特定一个元素
	/// </summary>
	/// <param name="list">List.</param>
	/// <param name="oldItem">Old item.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void AnyTypeListRemoveItem<T>(List<T> list, T oldItem)
	{
		for(int i = 0; i < list.Count; i++)  
		{  
			if(list[i].Equals(oldItem))  
			{  
				list.RemoveAt(i);
				break;
			}  
		}
	}
}
