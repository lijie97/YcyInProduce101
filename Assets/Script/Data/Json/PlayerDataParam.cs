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
    public List<PropertyParam> propertyList = new List<PropertyParam>() { new PropertyParam(PropertyType.Mood, 100), new PropertyParam(PropertyType.Vigor,100),new PropertyParam(PropertyType.TopicValue,100)};
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
#endregion

