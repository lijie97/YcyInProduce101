using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class SettingData : IJsonData<SettingData>
{

    public Action<DevicesQualityLevel> QualityOnChange;

    public bool IsOpenMusic
    {
        get
        { 
            return true;
        }
    }

    public bool IsOpenSound
    {
        get
        { 
            return true;
        }
    }

    public SettingDataParam settingData;


    public SettingData()
    {
        string jsonStr = "";

        this.DataFileName = "SettingJsonData";
        this.IsCreateAnotherData = true;
        this.IsEncryptData = false;

        //检查是否有另外的数据文件
        if (IsCreateAnotherData && FileTool.IsFileExists(DataFileName))
        {
            jsonStr = FileTool.ReadAllFile(DataFileName);
            jsonStr = IsEncryptData ? DesCode.DecryptDES(jsonStr, DesCode.PassWord) : jsonStr;
            settingData = JsonConvert.DeserializeObject<SettingDataParam>(jsonStr);
        }
        else
        {
            settingData = new SettingDataParam();

            jsonStr = JsonConvert.SerializeObject(settingData);
            if (IsCreateAnotherData)
            {
                FileTool.createORwriteFile(DataFileName, IsEncryptData ? DesCode.EncryptDES(jsonStr, DesCode.PassWord) : jsonStr);
            }
        }

    }

    public void SaveData()
    {
        base.SaveData(JsonConvert.SerializeObject(settingData));
    }

    public void ChangeScene()
    {
        this.QualityOnChange = null;
    }

    //系统检测的品质
    public DevicesQualityLevel DetectQualityLevel
    {
        get { return settingData.detectQualityLevel; }
        set
        { 
            //首次设置
            if (settingData.detectQualityLevel == DevicesQualityLevel.UnKnow)
                PlayerSettingQualityLevel = value;

            settingData.detectQualityLevel = value;
        }
    }

    //玩家手动设置的品质
    public DevicesQualityLevel PlayerSettingQualityLevel
    {
        get { return settingData.forceSetQualityLevel; }
        set
        {
            settingData.forceSetQualityLevel = value; 
            if (QualityOnChange != null)
                QualityOnChange(value);
        }
    }

    //用于读取当前品质，此统一获取接口用于方便修改
    public DevicesQualityLevel QualityLevel
    {
        get{ return PlayerSettingQualityLevel; }
    }
}

public class SettingDataParam
{
	public float musicVol = 1f;
	public float soundVol = 1f;

    //检测的品质
    public DevicesQualityLevel detectQualityLevel = DevicesQualityLevel.UnKnow;
    //手动设置的品质
    public DevicesQualityLevel forceSetQualityLevel = DevicesQualityLevel.High;

    public CameraVisual cameraVisual = CameraVisual.Free;
}

//设备品质层级
public enum DevicesQualityLevel
{
    Low = 0,
    Mid = 1,
    High = 2,
    UnKnow,
}

//视角
public enum CameraVisual
{
    Free,
    Lock,
}