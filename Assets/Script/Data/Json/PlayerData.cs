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

    public Action<int> CoinChangeEvent;
    public Action<int> JewelChangeEvent;
    public Action<int> ExpChangeEvent;
    public Action<int> LevelChangeEvent;

    public void ClearAction()
    {
        CoinChangeEvent = null;
        JewelChangeEvent = null;
        ExpChangeEvent = null;
        LevelChangeEvent = null;
    }

    #endregion


    #region PlayerInfo

    public int Coin
    {
        get { return playerData.playerInfo.coin; }
        set
        {
            playerData.playerInfo.coin = value;

            if (CoinChangeEvent != null && Application.isPlaying)
                CoinChangeEvent(value);
        }
    }

    public int Jewel
    {
        get { return playerData.playerInfo.jewel; }
        set
        {
            int preJewel = playerData.playerInfo.jewel;
            int newJewel = value;

            playerData.playerInfo.jewel = value;

            try
            {
                if (JewelChangeEvent != null && Application.isPlaying)
                    JewelChangeEvent(value);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + " " + e.StackTrace);
            }
        }
    }

    #endregion

}
