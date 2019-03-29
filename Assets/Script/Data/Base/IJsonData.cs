using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// Json数据的读写基类
/// </summary>
public class IJsonData<T>: SingletonBase<T> where T : new()
{

    private const string path = "Data/JSON/";

	#region 文件操作.
	protected string DataFileName;
	
    protected bool IsCreateAnotherData = false; //是否创建另外的数据文件进行读写,false为只读操作
    protected bool IsEncryptData = false;  //是否加密
    string jsonStr;
	
    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="fileName">文件名.</param>
    /// <param name="isCreateAnotherData">是否生成新的副本，用于读写</param>
    /// <param name="isEncryptData">是否加密.</param>
    public string InitData(string fileName, bool isCreateAnotherData = false, bool isEncryptData = false)
	{
        this.DataFileName = fileName;
        this.IsCreateAnotherData = isCreateAnotherData;
        this.IsEncryptData = isEncryptData;

        //检查是否有另外的数据文件
        if (IsCreateAnotherData && FileTool.IsFileExists(DataFileName))
        {
            jsonStr = FileTool.ReadAllFile(DataFileName);
            jsonStr = IsEncryptData ? DesCode.DecryptDES(jsonStr, DesCode.PassWord) : jsonStr;
        }
        else
        {
            TextAsset textAsset = Resources.Load (path + DataFileName) as TextAsset;
            if (textAsset == null)
                return "";

            jsonStr = textAsset.text;

            if (IsCreateAnotherData)
            {
                FileTool.createORwriteFile(DataFileName, IsEncryptData ? DesCode.EncryptDES(jsonStr, DesCode.PassWord) : jsonStr);
            }
        }

        return jsonStr;
	}

    public void InitData(string fileName, string json, bool isEncryptData = false)
    {
        this.DataFileName = fileName;
        this.IsEncryptData = isEncryptData;
        this.IsCreateAnotherData = true;
        this.jsonStr = json;

        FileTool.createORwriteFile(DataFileName, IsEncryptData ? DesCode.EncryptDES(jsonStr, DesCode.PassWord) : jsonStr);

    }

    // <summary>
    /// 本地数据保存
    /// </summary>
    public virtual void SaveData(string jsonStr)
    {
        if (!IsCreateAnotherData)
            return;
        
        FileTool.createORwriteFile(DataFileName, IsEncryptData ? DesCode.EncryptDES(jsonStr, DesCode.PassWord) : jsonStr);
    }

	#endregion

}
