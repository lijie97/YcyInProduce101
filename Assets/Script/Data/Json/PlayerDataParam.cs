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
    public int curDays =1;//当前天数
    public TimeType curTimeType = TimeType.Morning;//当前时间点
    public int talentPoint = 0;//才艺点
    public float songProficiency = 0;//公演曲熟练度
    public int countDownTime = 100;//倒计时（距离公演日期）
    public int curStoryIndex = 1;
    public List<PropertyParam> propertyList = new List<PropertyParam>() { new PropertyParam(PropertyType.Mood, 100), new PropertyParam(PropertyType.Vigor,100),new PropertyParam(PropertyType.TopicValue,100), new PropertyParam(PropertyType.Talent, 0), new PropertyParam(PropertyType.SongProficiency, 0) };
    public bool isMusicGame = false;
    public int curMaxScore = 0;
}

public class PropertyParam
{
    public PropertyType propertyType;
    public int curValue;

    public PropertyParam(PropertyType propertyType,int value)
    {
        this.propertyType = propertyType;
        this.curValue = value;
    }
}

public enum TimeType
{
    Morning =1,
    Afternoon,
    Night,
    LateAtNight
}

public enum BehaviorType
{
    Go2Class=1,//上课
    Rehearsal,//排练
    Interact,//互动
    Rest//休息

}

public enum PropertyType
{
    Mood = 1,//心情
    Vigor,//精力
    TopicValue,//话题度
    Talent,//才艺点
    SongProficiency//公演歌曲熟练度
}
#endregion

