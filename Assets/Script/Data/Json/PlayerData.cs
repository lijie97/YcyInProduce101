using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerData : IJsonData<PlayerData>
{
    public PlayerDataParam playerData;

    #region 初始化

    public PlayerData()
    {
        string jsonStr = base.InitData("PlayerData", true, false);
        playerData = JsonConvert.DeserializeObject<PlayerDataParam>(jsonStr);
    }

    public void SaveData()
    {
        base.SaveData(JsonConvert.SerializeObject(playerData));
    }


    public void ClearAction()
    {
    }

    #endregion


    #region PlayerInfo
    /// <summary>
    /// 获得人物属性参数
    /// </summary>
    /// <returns>The property parameter.</returns>
    /// <param name="propertyID">Property identifier.</param>
    public PropertyParam GetPropertyParam(int propertyID)
    {
        for (int i = 0; i < playerData.playerInfo.propertyList.Count; i++)
        {
            PropertyType type = (PropertyType)propertyID;
            if(type == playerData.playerInfo.propertyList[i].propertyType)
            {
                return playerData.playerInfo.propertyList[i];
            }
        }
        return null;
    }

    #endregion

}
