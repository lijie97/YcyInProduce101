using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// 数据读取的基类
/// </summary>
public class ICSVData<T> : SingletonBase<T> where T : new()
{
    private const string path = "Data/CSV/";

    //存数数据
    protected Hashtable DataTable;
    protected string[][] levelArray;
    protected List<string> idList;
	
    protected int DataRow;
	
    //设置读取数据表的名称
    protected void InitData(string fileName)
    {
        if (DataTable == null)
            DataTable = new Hashtable();
        else
            DataTable.Clear();

        if (idList == null)
            idList = new List<string>();
        else
            idList.Clear();
        
        //Debug.Log(fileName);
        TextAsset binAsset = Resources.Load(path + fileName, typeof(TextAsset)) as TextAsset;
			
        //读取每一行的内容
        string[] lineArray = binAsset.text.Split('\r');
        if (lineArray.Length <= 1)
            lineArray = binAsset.text.Split('\n');
        //创建二维数组
        levelArray = new string [lineArray.Length][];
        //把csv中的数据储存在二位数组中
        for (int i = 0; i < lineArray.Length; i++)
        {
            levelArray[i] = lineArray[i].Split(',');
        }

        //将数据存储到哈希表中，存储方法：Key为name+id，Value为值
        int nRow = levelArray.Length;
        int nCol = levelArray[0].Length;
		
        DataRow = nRow - 1;

        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < levelArray.Length; ++i)
        {
            if (levelArray[i][0] == "\n" || levelArray[i][0] == "")
            {
                nRow--;
                DataRow = nRow - 1;
                continue;
            }
				
            string id = levelArray[i][0].Trim();
            idList.Add(id);
            for (int j = 1; j < nCol; ++j)
            {  
                sb.Append(levelArray[0][j]);
                sb.Append("_");
                sb.Append(id);
                DataTable.Add(sb.ToString(), levelArray[i][j]);

                sb.Length = 0;
            }
        }
    }

    /// <summary>
    /// Gets the data row.
    /// 返回表格的行数
    /// </summary>
    /// <returns>
    /// The data row.
    /// </returns>
    public virtual int GetDataRow()
    {
        return DataRow;
    }

    //判断是否是数字 用于忽略第一行的中文注释
    private bool IsId(string str)
    {
        str = str.Trim();
        if (Regex.IsMatch(str, @"[\u4e00-\u9fa5]"))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 获取CSV第一列的所有数据
    /// </summary>
    /// <returns>The identifier list.</returns>
    public virtual List<string> GetIDList()
    {
        return idList;
    }
	
    //根据name和id获取相关属性，返回string类型
    protected virtual string GetProperty(string name, int id)
    {
        return GetProperty(name, id.ToString());
    }

    StringBuilder keysb = new StringBuilder();

    protected virtual string GetProperty(string name, string id)
    {
        keysb.Length = 0;
        keysb.Append(name);
        keysb.Append("_");
        keysb.Append(id);
        if (DataTable.ContainsKey(keysb.ToString()))
            return DataTable[keysb.ToString()].ToString();
        else
            return "";
    }

    public void JustInit()
    {
    }


    public int GetPropertyToInt(string name, int id)
    {
        return GetPropertyToInt(name, id.ToString());
    }

    public int GetPropertyToInt(string name, string id)
    {
        string str = GetProperty(name, id).ToString();

        if (string.IsNullOrEmpty(str))
            return 0;

        int iData = int.Parse(str);
        return iData;
    }

    public float GetPropertyToFloat(string name, string id)
    {
        string str = GetProperty(name, id).ToString();

        if (string.IsNullOrEmpty(str))
            return 0f;

        float fData = float.Parse(str);
        return fData;
    }

    public float GetPropertyToFloat(string name, int id)
    {
        return this.GetPropertyToFloat(name, id.ToString());
    }
}
