using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class PlayerDataParam
{
    public PlayerInfoParam playerInfo = new PlayerInfoParam();
}
#region 玩家数据
public class PlayerInfoParam
{
    public int coin = 0;
    public int jewel = 0;
    public int hpRecoverProp = 1;//血量回复道具

    public string lastQuitPos = "(0,0,0)";//上次退出地点

    public string lastDeadPos = "(0,0,0)";//上次死亡地点

    public string lastGetGiftDateTime;//上次领取礼包的时间

    public string lastLoginDateTime;//上次登录时间

    public int dailyGiftTimes = 0;

    public string languageType;
}
#endregion

